using System;
using System.Collections.Generic;
using DotSpatial.Data;
using DotSpatial.Projections;
using DotSpatial.Topology;

namespace DotSpatial.Plugins.BruTileLayer.Reprojection
{
    public static class ReprojectExtensions
    {
        public static Coordinate Reproject(this Coordinate c, ProjectionInfo source, ProjectionInfo target)
        {
            var ordinates = new[] { c.X, c.Y };
            var z = new[] {double.IsNaN(c.Z) ? 0 : c.Z};
            Projections.Reproject.ReprojectPoints(ordinates, z, source, target, 0, 1);
            return new Coordinate(ordinates);
        }

        public static IList<Coordinate> Reproject(this IList<Coordinate> seq, ProjectionInfo source, ProjectionInfo target)
        {
            var ords = new double[seq.Count * 2];
            var z = new Double[seq.Count];
            var j = 0;
            for (var i = 0; i < seq.Count; i++)
            {
                var c = seq[i];
                ords[j++] = c.X;
                ords[j++] = c.Y;
                z[i] = double.IsNaN(c.Z) ? 0 : c.Z;
            }

            Projections.Reproject.ReprojectPoints(ords, z, source, target, 0, seq.Count);

            var lst = new List<Coordinate>(seq.Count);
            j = 0;
            for (var i = 0; i < seq.Count; i++)
                lst.Add(new Coordinate(ords[j++], ords[j++]));

            return lst;
        }

        public static ILinearRing Reproject(this ILinearRing ring, ProjectionInfo source, ProjectionInfo target)
        {
            var seq = Reproject(ring.Coordinates.Densify(36), source, target);
            return ring.Factory.CreateLinearRing(seq);
        }

        private static IList<Coordinate> Densify(this IList<Coordinate> self, int intervals)
        {
            if (self.Count < 2)
                return self;

            var res = new List<Coordinate>(intervals*(self.Count - 1) + 1);
            var start = self[0];

            for (var i = 1; i < self.Count; i++)
            {
                res.Add(start);
                var end = self[i];
                
                var dx = (end.X - start.X)/intervals;
                var dy = (end.Y - start.Y)/intervals;

                for (var j = 0; j < intervals-1; j++)
                {
                    start = new Coordinate(start.X + dx, start.Y + dy);
                    res.Add(start);
                }
                res.Add(end);
                start = end;
            }
            return res;

        }

        public static IPolygon Reproject(this IPolygon polygon, ProjectionInfo source, ProjectionInfo target)
        {
            var shell = Reproject(polygon.Shell, source, target);
            ILinearRing[] holes = null;
            if (polygon.NumHoles > 0)
            {
                holes = new ILinearRing[polygon.NumHoles];
                var i = 0;
                foreach (var hole in polygon.Holes)
                    holes[i++] = Reproject(hole, source, target);
            }

            return polygon.Factory.CreatePolygon(shell, holes);
        }

        public static Extent Reproject(this Extent extent, ProjectionInfo source, ProjectionInfo target, int depth = 0)
        {
            var xy = ToSequence(extent);
            Projections.Reproject.ReprojectPoints(xy, null, source, target, 0, xy.Length / 2);
            var res = ToExtent(xy);

            return res;
        }

        static double[] ToSequence(Extent extent)
        {
            const int horizontal = 72;
            const int vertical = 36;
            var res = new double[horizontal * vertical * 2];

            var dx = extent.Width / (horizontal-1);
            var dy = extent.Height / (vertical-1);

            var minY = extent.MinY;
            var k = 0;
            for (var i = 0; i < vertical; i++)
            {
                var minX = extent.MinX;
                for (var j = 0; j < horizontal; j++)
                {
                    res[k++] = minX;
                    res[k++] = minY;
                    minX += dx;
                }
                minY += dy;
            }

            return res;
        }

        private static Extent ToExtent(double[] xyOrdinates)
        {
            double minX = double.MaxValue, maxX = double.MinValue;
            double minY = double.MaxValue, maxY = double.MinValue;

            var i = 0;
            while (i < xyOrdinates.Length)
            {
                if (!double.IsNaN(xyOrdinates[i]) &&
                    (double.MinValue < xyOrdinates[i] && xyOrdinates[i] < double.MaxValue))
                {
                    if (minX > xyOrdinates[i]) minX = xyOrdinates[i];
                    if (maxX < xyOrdinates[i]) maxX = xyOrdinates[i];
                }
                i += 1;
                if (!double.IsNaN(xyOrdinates[i]) &&
                    (double.MinValue < xyOrdinates[i] && xyOrdinates[i] < double.MaxValue))
                {
                    if (minY > xyOrdinates[i]) minY = xyOrdinates[i];
                    if (maxY < xyOrdinates[i]) maxY = xyOrdinates[i];
                }
                i += 1;
            }
            return new Extent(minX, minY, maxX, maxY);
        }


    }
}