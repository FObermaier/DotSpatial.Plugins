using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using DotSpatial.Controls;
using DotSpatial.Data;
using DotSpatial.Projections;

namespace DotSpatial.Plugins.SharpMapLayer
{
    public class SharpMapLayer : Symbology.Layer, IMapLayer
    {
        /// <summary>
        /// The map
        /// </summary>
        private readonly SharpMap.Map _map;
        
        /// <summary>
        /// The map symbol
        /// </summary>
        private Image _mapSymbol;

        /// <summary>
        /// Function to create a formatter that can serialize or deserialize the <see cref="_map"/>
        /// </summary>
        /// <returns>A formatter</returns>
        private static IFormatter GetSharpMapFormatter()
        {
            var formatter = new BinaryFormatter();
            if (formatter.SurrogateSelector == null)
                formatter.SurrogateSelector = new SurrogateSelector();
            formatter.SurrogateSelector.ChainSelector(SharpMap.Utilities.Surrogates.GetSurrogateSelectors());
            BruTile.Utility.AddBruTileSurrogates(formatter);
            return formatter;
        }

        public SharpMapLayer()
            :this(new SharpMap.Map())
        {
        }

        /// <summary>
        /// Creates a new SharpMapLayer
        /// </summary>
        /// <param name="map">The map object</param>
        public SharpMapLayer(SharpMap.Map map)
        {
            _map = map;
            Wire();
        }


        private void Wire()
        {
            _map.Layers.ListChanged += HandleLayersListChanged;
            HandleLayersListChanged(null, new ListChangedEventArgs(ListChangedType.ItemMoved, 0));
        }

        private void HandleLayersListChanged(object sender, ListChangedEventArgs e)
        {
            using (var m = _map.Clone())
            {
                m.DisposeLayersOnDispose = false;
                m.Size = new Size(24, 24);
                m.ZoomToExtents();
                _mapSymbol = m.GetMap();
                MyExtent = GeoAPI.Geometries.GeometryConverter.ToDotSpatial(m.GetExtents());
            }
        }

        private void UnWire()
        {
            _map.Layers.ListChanged -= HandleLayersListChanged;
        }

        /// <summary>
        /// Creates a new SharpMapLayer
        /// </summary>
        /// <param name="base64EncMap">The Base64 encoded map object</param>
        public SharpMapLayer(string base64EncMap)
        {
            using (var m = new MemoryStream(Convert.FromBase64String(base64EncMap)))
            {
                var bf = GetSharpMapFormatter();
                _map = (SharpMap.Map) bf.Deserialize(m);
                Wire();
            }
        }

        protected override void Dispose(bool disposeManagedResources)
        {
            UnWire();
            _map.Dispose();
            base.Dispose(disposeManagedResources);
        }

        [Serialization.Serialize("Base64EncMap, 0")]
        public string Base64EncMap
        {
            get
            {
                using (var m = new MemoryStream())
                {
                    var bf = GetSharpMapFormatter();
                    bf.Serialize(m, _map);

                    return Convert.ToBase64String(m.ToArray());
                }
            }
        }

        /// <summary>
        /// Gets the SharpMap map object
        /// </summary>
        public SharpMap.Map Map { get { return _map; } }

        public void DrawRegions(MapArgs args, List<Extent> regions)
        {
            // Assert that the map has the same size as the DotSpatial map
            var size = args.ImageRectangle.Size;
            _map.Size = size;
            
            // Make sure the map is zoomed to the same extent
            var env = Topology.GeometryConverter.ToGeoAPI(args.GeographicExtents);
            _map.ZoomToBox(env);

            // Always render the whole map to the device
            _map.RenderMap(args.Device);

            /*
             * This is not suitable because we might draw to printer/plotter 
             * which will make for a huge Image
             */
            #region
            //using (var mapImage = _map.GetMap())
            //{
            //    foreach (var region in regions)
            //    {
            //        var loc = args.ProjToPixel(region);
            //        args.Device.DrawImage(mapImage, loc, 
            //                              loc.X, loc.Y, loc.Width, loc.Height, 
            //                              GraphicsUnit.Pixel);
            //    }
            //}
            #endregion
        }

        public override void Reproject(ProjectionInfo targetProjection)
        {
            var ctFac = new ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory();
            var csFac = new ProjNet.CoordinateSystems.CoordinateSystemFactory();

            var csTarget = csFac.CreateFromWkt(targetProjection.ToEsriString());

            foreach (var layer in EnumerateLayers(_map))
            {
                var lLayer = layer as SharpMap.Layers.Layer;
                if (lLayer == null) continue;

                GeoAPI.CoordinateSystems.ICoordinateSystem csSource = null;
                if (lLayer.CoordinateTransformation != null)
                {
                    csSource = lLayer.CoordinateTransformation.SourceCS;
                }
                if (!string.IsNullOrEmpty(layer.Proj4Projection))
                {
                    csSource = csFac.CreateFromWkt(ProjectionInfo.FromProj4String(layer.Proj4Projection).ToEsriString());
                }
                else if (layer.SRID != 0)
                {
                    csSource = csFac.CreateFromWkt(ProjectionInfo.FromEpsgCode(layer.SRID).ToEsriString());
                }
                var ctF = ctFac.CreateFromCoordinateSystems(csSource, csTarget);
                var ctR = ctFac.CreateFromCoordinateSystems(csTarget, csSource);

                lLayer.CoordinateTransformation = ctF;
                lLayer.ReverseCoordinateTransformation = ctR;

            }
            
            throw new InvalidOperationException("Cannot Setup CoordinateTransformation as long as ProjectionInfo does not maintain SRID values.");
        }

        static IEnumerable<SharpMap.Layers.ILayer> EnumerateLayers(SharpMap.Map map)
        {
            foreach (var layer in EnumerateLayers(map.BackgroundLayer))
                yield return layer;
            foreach (var layer in EnumerateLayers(map.VariableLayers))
                yield return layer;
            foreach (var layer in EnumerateLayers(map.Layers))
                yield return layer;
        }

        static IEnumerable<SharpMap.Layers.ILayer> EnumerateLayers(IEnumerable<SharpMap.Layers.ILayer> layerCollection)
        {
            foreach (var layer in layerCollection)
            {
                var lg = layer as SharpMap.Layers.LayerGroup;
                if (lg != null)
                {
                    foreach (var tmp in EnumerateLayers(lg.Layers))
                    {
                        yield return tmp;
                    }
                }
                else
                {
                    yield return layer;
                }
            }    
        }

        /// <summary>
        /// Draws the symbol for this specific category to the legend
        /// </summary>
        /// <param name="g"></param>
        /// <param name="box"></param>
        public override void LegendSymbol_Painted(Graphics g, Rectangle box)
        {
            if (_mapSymbol != null)
                g.DrawImageUnscaled(_mapSymbol, box.X, box.Y, box.Width, box.Height);
            base.LegendSymbol_Painted(g, box);
        }
    }
}
