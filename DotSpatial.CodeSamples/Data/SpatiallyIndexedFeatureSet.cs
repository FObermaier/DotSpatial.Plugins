#define QueryOnly

#if QueryOnly
using SbnTree = SharpSbn.SbnQueryOnlyTree;
#else
using SbnTree = SharpSbn.SbnTree
#endif

namespace DotSpatial.Data
{

    /// <summary>
    /// A class that associates a ShapeFile spatial index (SBN) with a <see cref="DotSpatial.Data.IFeatureSet"/>.
    /// </summary>
    public class SpatiallyIndexedFeatureSet : System.IDisposable
    {
        private readonly SbnTree _tree;
        private readonly IFeatureSet _featureSet;
        
        /// <summary>
        /// Creates an instance of this class
        /// </summary>
        /// <param name="featureSet">The feature set to choose</param>
        public SpatiallyIndexedFeatureSet(IFeatureSet featureSet)
        {
            System.Diagnostics.Debug.Assert(featureSet != null);

            _featureSet = featureSet;
            _tree = TestShapefileTree(featureSet);

            if (_tree == null)
            {
                var z = GetInterval(featureSet.Z);
                var m = GetInterval(featureSet.M);
                var tree = SharpSbn.SbnTree.Create(GetFeaturesBoundingBoxes(featureSet), z, m);
                var sbnFilename = System.IO.Path.ChangeExtension(System.IO.Path.GetTempFileName(), "sbn");
                tree.Save(sbnFilename);
#if QueryOnly
                _tree = SbnTree.Open(sbnFilename);
#else
                _tree = tree;
#endif
            }
        }

        private static SharpSbn.DataStructures.Interval GetInterval(double[] values)
        {
            if (values == null)
                return SharpSbn.DataStructures.Interval.Create();

            var min = double.MaxValue;
            var max = double.MinValue;

            for (var i = 0; i < values.Length; i++)
            {
                if (values[i] < min) min = values[i];
                if (values[i] > max) max = values[i];
            }

            return SharpSbn.DataStructures.Interval.Create(min, max);
        }

        /// <summary>
        /// Class finalizer
        /// </summary>
        ~SpatiallyIndexedFeatureSet()
        {
            Dispose(false);
        }

        /// <summary>
        /// Method to dispose the feature set (if it is not locked=
        /// </summary>
        /// <param name="disposing">Disposing?</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!_featureSet.IsDisposeLocked)
                    _featureSet.Dispose();
            }
        }

        void System.IDisposable.Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Test if <paramref name="featureSet"/> is a <see cref="DotSpatial.Data.Shapefile"/> 
        /// and if so, try to read tree for it. Otherwise a new one is created.
        /// </summary>
        /// <param name="featureSet">The feature set</param>
        /// <returns>An index</returns>
        private static SbnTree TestShapefileTree(IFeatureSet featureSet)
        {
            // Is this really a shapefile?
            var shapefile = featureSet as Shapefile;
            if (shapefile == null) return null;

            // Is there a sbn file floating around?
            if (!string.IsNullOrEmpty(shapefile.FilePath))
            {
                var sbnFilename = System.IO.Path.ChangeExtension(shapefile.FilePath, ".sbn");
                if (!System.IO.File.Exists(sbnFilename))
                {
                    var h = shapefile.Header;
                    var tree = SharpSbn.SbnTree.Create(GetFeaturesBoundingBoxes(featureSet),
                        SharpSbn.DataStructures.Interval.Create(h.Zmin, h.Zmax),
                        SharpSbn.DataStructures.Interval.Create(h.Mmin, h.Mmax));
                    
                    tree.Save(sbnFilename);
                }
#if QueryOnly
                return SbnTree.Open(sbnFilename);
#else
                return SbnTree.Load(sbnFilename);
#endif
            }
            return null;
        }

        private static System.Collections.Generic.ICollection<System.Tuple<uint, SharpSbn.DataStructures.Envelope>>
            GetFeaturesBoundingBoxes(IFeatureSet featuresSet)
        {
            var res = new System.Collections.Generic.List<System.Tuple<uint, SharpSbn.DataStructures.Envelope>>(
                    featuresSet.NumRows());

            for (var i = 0; i < featuresSet.NumRows(); i++)
            {
                var feature = featuresSet.GetFeature(i);
                var min = feature.Envelope.Minimum;
                var max = feature.Envelope.Maximum;
                res.Add(System.Tuple.Create((uint) feature.Fid, new SharpSbn.DataStructures.Envelope(
                    min.X, max.X, min.Y, max.Y)));
            }
            return res;
        }

        /// <summary>
        /// Method to query all features that intersect a given <see cref="extent"/>
        /// </summary>
        /// <param name="extent">The area used to look for features</param>
        /// <returns>An enumeration of features</returns>
        public System.Collections.Generic.IEnumerable<IFeature> Intersect(Extent extent)
        {
            var env = new SharpSbn.DataStructures.Envelope(extent.MinX, extent.MaxX, extent.MinY, extent.MaxY);
            foreach (var queryFid in _tree.QueryFids(env))
            {
                var fid = (int) queryFid - 1;
                yield return _featureSet.GetFeature(fid);
            }
        }

        /// <summary>
        /// Method to query all features that intersect a given <see cref="geometry"/>
        /// </summary>
        /// <param name="geometry">The geometry used to look for features</param>
        /// <returns>An enumeration of features</returns>
        public System.Collections.Generic.IEnumerable<IFeature> Intersect(Topology.IGeometry geometry)
        {
            var dsEnv = geometry.EnvelopeInternal.ToExtent();
            var env = new SharpSbn.DataStructures.Envelope(dsEnv.MinX, dsEnv.MaxX, dsEnv.MinY, dsEnv.MaxY);
            foreach (var queryFid in _tree.QueryFids(env))
            {
                var fid = (int) queryFid - 1;
                var f = _featureSet.GetFeature(fid);
                if (geometry.Intersects(Topology.Geometry.FromBasicGeometry(f)))
                    yield return f;
            }
        }

        public static System.Collections.Generic.IEnumerable<IFeature> PointsInPolygon(IFeatureSet polygons, IFeatureSet points,
            double bufferPolygon = 0d)
        {
            var sishp = new SpatiallyIndexedFeatureSet(points);
            foreach (var feature in polygons.Features)
            {
                var geom = Topology.Geometry.FromBasicGeometry(feature.BasicGeometry);
                if (bufferPolygon > 0) geom = geom.Buffer(bufferPolygon);

                var candidates = sishp.Intersect(geom);
                foreach (var candidate in candidates)
                {
                    if (geom.Contains(Topology.Geometry.FromBasicGeometry(candidate.BasicGeometry)))
                    {
                        yield return candidate;
                    }
                }
            }
        }
    }
}
