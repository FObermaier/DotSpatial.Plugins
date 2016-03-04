﻿// ********************************************************************************************************
// Product Name: DotSpatial.Plugins.BruTileLayer
// Description:  Adds BruTile functionality to DotSpatial
// ********************************************************************************************************
// The contents of this file are subject to the GNU Library General Public License (LGPL). 
// You may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and 
// limitations under the License. 
//
// The Initial Developer of this Original Code is Felix Obermaier. Created 2011.01.05 
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
// |      Name            |    Date     |                                Comments
// |----------------------|-------------|-----------------------------------------------------------------
// | Felix Obermaier      |  2011.01.05 | Initial commit
// | Felix Obermaier      |  2013.02.15 | All sorts of configuration code and user interfaces,
// |                      |             | making BruTileLayer work in sync- or async mode
// | Felix Obermaier      |  2013.09.19 | Making deserialization work by adding AssemblyResolve event,
// |                      |             | handler
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using BruTile;
using BruTile.Predefined;
using BtExtent = BruTile.Extent;
using DotSpatial.Controls;
using DotSpatial.Data.Forms;
using DotSpatial.Plugins.BruTileLayer.Configuration;
using DotSpatial.Plugins.BruTileLayer.Configuration.Forms;
using DotSpatial.Plugins.BruTileLayer.Properties;
using DotSpatial.Plugins.BruTileLayer.Reprojection;
using DotSpatial.Projections;
using DotSpatial.Projections.AuthorityCodes;
using DotSpatial.Serialization;
using DotSpatial.Symbology;
using DotSpatial.Topology;
using DsExtent = DotSpatial.Data.Extent;

namespace DotSpatial.Plugins.BruTileLayer
{
    /// <summary>
    /// BruTile Layer for DotSpatial
    /// </summary>
    [Export,Serializable]
    public class BruTileLayer : Layer, IMapLayer
    {
        /// <summary>
        /// Creates a Bing Maps Roads layer using the "Staging" url
        /// </summary>
        [Obsolete]
        public static BruTileLayer CreateBingRoadsLayer()
        {
            return CreateBingRoadsLayer(String.Empty);
        }

        /// <summary>
        /// Creates a Bing Maps Roads layer using the standard url. You need to have a valid token
        /// </summary>
        /// <param name="token">The token for Bing Maps access</param>
        [Obsolete]
        public static BruTileLayer CreateBingRoadsLayer(string token)
        {
            var ktc = string.IsNullOrEmpty(token) ? KnownTileSource.BingRoadsStaging : KnownTileSource.BingRoads;
            return CreateKnownLayer(ktc, token);
        }

        /// <summary>
        /// Creates a Bing Maps hybird layer using the "Staging" url
        /// </summary>
        [Obsolete]
        public static BruTileLayer CreateBingHybridLayer()
        {
            return CreateBingHybridLayer(String.Empty);
        }
        /// <summary>
        /// Creates a Bing Maps hybrid layer using the standard url. You need to have a valid token
        /// </summary>
        /// <param name="token">The token for Bing Maps access</param>
        [Obsolete]
        public static BruTileLayer CreateBingHybridLayer(string token)
        {
            var ktc = string.IsNullOrEmpty(token) ? KnownTileSource.BingHybridStaging : KnownTileSource.BingHybrid;
            return CreateKnownLayer(ktc, token);
        }


        /// <summary>
        /// Creates a Bing Maps aerial layer using the "Staging" url
        /// </summary>
        [Obsolete]
        public static BruTileLayer CreateBingAerialLayer()
        {
            return CreateBingAerialLayer(String.Empty);
        }

        /// <summary>
        /// Creates a Bing Maps aerial layer using the standard url. You need to have a valid token
        /// </summary>
        /// <param name="token">The token for Bing Maps access</param>
        [Obsolete]
        public static BruTileLayer CreateBingAerialLayer(string token)
        {
            var ktc = string.IsNullOrEmpty(token) ? KnownTileSource.BingAerialStaging : KnownTileSource.BingAerial;
            return CreateKnownLayer(ktc, token);
        }

        /// <summary>
        /// Creates a layer displaying a known tile source
        /// </summary>
        /// <param name="source">The known tile source</param>
        /// <param name="apiKey">The api key</param>
        /// <returns>The layer</returns>
        public static BruTileLayer CreateKnownLayer(KnownTileSource source, string apiKey)
        {
            var config = new KnownTileLayerConfiguration(null, source, apiKey);
            return new BruTileLayer(config);
        }

        /// <summary>
        /// Creates an OpenStreetMap layer
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public static BruTileLayer CreateOsmMapnicLayer()
        {
            var config = new KnownTileLayerConfiguration(null, KnownTileSource.OpenStreetMap, string.Empty);
            return new BruTileLayer(config);
        }

        #region Fields

        private ProjectionInfo _sourceProjection;

        [Serialize("sourceProjection")]
        private string SourceProjectionEsriString
        {
            get
            {
                return _sourceProjection.ToEsriString();
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    _sourceProjection = ProjectionInfo.FromAuthorityCode("EPSG", 3758);
                else
                    _sourceProjection = ProjectionInfo.FromEsriString(value);
            }
        }

        [Serialize("targetProjection")]
        private string TargetProjectionEsriString {
            get
            {
                return _targetProjection == null ? string.Empty : _targetProjection.ToEsriString();
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    _targetProjection = null;
                else
                    _targetProjection = ProjectionInfo.FromEsriString(value);
                Projection = _targetProjection ?? _sourceProjection;
            }
        }

        private ProjectionInfo _targetProjection;
        [Serialize("config", ConstructorArgumentIndex = 0)]
        private readonly IConfiguration _configuration;

        [NonSerialized]
        private Image _legendImage;
        private int _transparency;
        
        [NonSerialized]
        private readonly TileFetcher _tileFetcher;

        [NonSerialized]
        private readonly Stopwatch _stopwatch = new Stopwatch();

        [NonSerialized] private string _level;

        [NonSerialized]
        private readonly ImageAttributes _imageAttributes;

        #endregion

        /// <summary>
        /// Variable indicating whether BruTileLayer should generate an error image tile when BruTile fails to get the tile.
        /// This prevents BruTile from querying for that tile again.
        /// </summary>
        [Serialize("showErrorInTile")]
        public bool ShowErrorInTile = true;

        /// <summary>
        /// Creates an instance of this class with an OpenStreetMap tile source and
        /// a MemoryCache.
        /// </summary>
        public BruTileLayer()
            : this(new KnownTileLayerConfiguration(null, KnownTileSource.OpenStreetMap, string.Empty))
        { }

        /// <summary>
        /// Creates an instanc of this class using the given <paramref name="configuration"/>
        /// </summary>
        /// <param name="configuration">The configuration</param>
        public BruTileLayer(IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException("configuration");

            var data = base.ContextMenuItems.Find(m => m.Name == "Data");
            if (data != null) base.ContextMenuItems.Remove(data);

            /*
            data = base.ContextMenuItems.Find(m => m.Name.StartsWith("Attrib"));
            if (data != null) base.ContextMenuItems.Remove(data);
             */

            // Initialize the configuration prior to usage
            configuration.Initialize();

            // 
            _configuration = configuration;
            var tileSource = configuration.TileSource;
            // for wmts this might be some crude value (urn)
            var authorityCode = ToAuthotityCode(tileSource.Schema.Srs);
            if (!string.IsNullOrWhiteSpace(authorityCode))
                _sourceProjection = AuthorityCodeHandler.Instance[authorityCode];
            else
            {
                ProjectionInfo p;
                if (!TryParseProjectionEsri(tileSource.Schema.Srs, out p))
                    if (!TryParseProjectionProj4(tileSource.Schema.Srs, out p))
                        p = AuthorityCodeHandler.Instance["EPSG:3857"];
                _sourceProjection = p;
            }

            if (_sourceProjection == null)
                _sourceProjection = AuthorityCodeHandler.Instance["EPSG:3857"];
            
            // WebMercator: set datum to WGS1984 for better accuracy 
            if (tileSource.Schema.Srs == "EPSG:3857") 
                _sourceProjection.GeographicInfo.Datum = KnownCoordinateSystems.Geographic.World.WGS1984.GeographicInfo.Datum;
            
            Projection = _sourceProjection;
            var extent = tileSource.Schema.Extent;
            MyExtent = new DsExtent(extent.MinX, extent.MinY, extent.MaxX, extent.MaxY);

            base.LegendText = configuration.LegendText;
            base.LegendItemVisible = true;
            base.IsVisible = base.Checked = true;

            base.LegendSymbolMode = SymbolMode.Symbol;
            LegendType = LegendType.Custom;
            
            _tileFetcher = configuration.TileFetcher;
            _tileFetcher.TileReceived += HandleTileReceived;
            _tileFetcher.QueueEmpty += HandleQueueEmpty;

            //Set the wrap mode
            _imageAttributes = new ImageAttributes();
            _imageAttributes.SetWrapMode(WrapMode.TileFlipXY);
        }

        private static bool TryParseProjectionProj4(string proj4, out ProjectionInfo projectionInfo)
        {
            try{
                projectionInfo = ProjectionInfo.FromProj4String(proj4);
            }
            catch {
                projectionInfo = null;
            }
            return projectionInfo != null;
        }

        private static bool TryParseProjectionEsri(string esriWkt, out ProjectionInfo projectionInfo)
        {
            try
            {
                projectionInfo = ProjectionInfo.FromEsriString(esriWkt);
            }
            catch
            {
                projectionInfo = null;
            }
            return projectionInfo != null;
        }

        private static string ToAuthotityCode(string srs)
        {
            if (string.IsNullOrWhiteSpace(srs))
                throw new ArgumentNullException("srs");

            // no colon
            if (srs.IndexOf(":",StringComparison.CurrentCulture) < 0 )
            {
                int value;
                if (!int.TryParse(srs, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out value))
                    throw new ArgumentException("srs");
                return string.Format("EPSG:{0}", value);
            }

            // break the string in parts
            var srsParts = srs.Split(':');

            // one colon => assume Authority:Code
            if (srsParts.Length == 2)
                return srs;

            // urn:ogc:def:crs:EPSG:6.18.3:3857
            if (srsParts.Length > 4)
                return string.Format("{0}:{1}", srsParts[4], srsParts[srsParts.Length-1]);

            return "";
        }

        private void HandleTileReceived(object sender, TileReceivedEventArgs e)
        {
            var i = e.TileInfo.Index;
            if (i.Level != _level) return;
            
            //System.Diagnostics.Debug.WriteLine("Tile received (Index({0}, {1}, {2}))", i.Level, i.Row, i.Col);
// some timed refreshes if the server becomes slooow...
            if (_stopwatch.Elapsed.Milliseconds > 250 && ! _tileFetcher.Ready())
            {
                _stopwatch.Reset();
                MapFrame.Invalidate();
                _stopwatch.Restart();
                return;
            }

            var ext = ToBrutileExtent(MapFrame.ViewExtents);
            if (ext.Intersects(e.TileInfo.Extent))
            {
                MapFrame.Invalidate(FromBruTileExtent(e.TileInfo.Extent));
            }
        }

        private void HandleQueueEmpty(object sender, EventArgs empty)
        {
            _stopwatch.Reset();
            MapFrame.Invalidate();
        }

        protected override void OnShowProperties(HandledEventArgs e)
        {
            using (var frm = new BruTileLayerDisplayProperties())
            {
                frm.BruTileLayer = this;
                if (frm.ShowDialog() == DialogResult.OK)
                    frm.BruTileLayer.MapFrame.Invalidate();
            }
        }
        
        private static DsExtent FromBruTileExtent(BtExtent extent)
        {
            return new DsExtent(extent.MinX, extent.MinY, extent.MaxX, extent.MaxY);
        }
        
        /// <summary>
        /// Erstellt ein neues Objekt, das eine Kopie der aktuellen Instanz darstellt.
        /// </summary>
        /// <returns>
        /// Ein neues Objekt, das eine Kopie dieser Instanz darstellt.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public object Clone()
        {
            return new BruTileLayer(_configuration.Clone()) { ShowErrorInTile = ShowErrorInTile, Transparency = Transparency };
        }

        /// <summary>
        /// Tests the specified legend item to determine whether or not
        /// it can be dropped into the current item.
        /// </summary>
        /// <param name="item">Any object that implements ILegendItem</param>
        /// <returns>Boolean that is true if a drag-drop of the specified item will be allowed.</returns>
        public override bool CanReceiveItem(ILegendItem item)
        {
            return false;
        }

        private Image GetLegendImage()
        {
            const int height = 16; //px

            try
            {

                var schema = _configuration.TileSource.Schema;
                var resolution = schema.Resolutions.Values.First();
                var tiles = new List<TileInfo>(schema.GetTileInfos(schema.Extent, resolution.UnitsPerPixel));

                if (tiles.Count <= 4)
                {
                    var tmpTileWidth = resolution.TileWidth > 0 ? resolution.TileWidth : 256;
                    var tmpTileHeight = resolution.TileWidth > 0 ? resolution.TileWidth : 256;

                    //Ratio of width to height for each tile 
                    var ratio = (double)tmpTileWidth / tmpTileHeight;
                    var width = (int)Math.Round(height * ratio, MidpointRounding.ToEven);
                    var result = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                    var tileHeight = tiles.Count == 4 ? height / 2 : height;
                    var tileWidth = tiles.Count == 4 ? width / 2 : width;
                    using (var g = Graphics.FromImage(result))
                    {
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                        var ia = new ImageAttributes();
                        ia.SetWrapMode(WrapMode.TileFlipXY);

                        foreach (var ti in tiles)
                        {
                            var image = GetTileImage(ti);
                            if (image == null) continue;

                            switch (schema.YAxis)
                            {
                                case YAxis.OSM:
                                    g.DrawImage(image, ti.Index.Col * tileWidth, ti.Index.Row * tileHeight, tileWidth,
                                                tileHeight);
                                    break;
                                case YAxis.TMS:
                                    g.DrawImage(image, ti.Index.Col * tileWidth - 1, height - (ti.Index.Row + 1) * tileHeight - 1, tileWidth,
                                                tileHeight);
                                    break;

                            }
                        }

                        ia.Dispose();
                    }
                    return result;
                }

            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch { }

            return new Bitmap(Resources.BruTileLogoSmall);

        }

        private Image GetTileImage(TileInfo tileInfo)
        {
            var buffer = _configuration.TileSource.GetTile(tileInfo);
            return buffer != null ? Image.FromStream(new MemoryStream(buffer)) : null;
        }

        /// <summary>
        /// Instructs this legend item to perform custom drawing for any symbols.
        /// </summary>
        /// <param name="g">A Graphics surface to draw on</param>
        /// <param name="box">The rectangular coordinates that confine the symbol.</param>
        public override void LegendSymbol_Painted(Graphics g, Rectangle box)
        {
            var image = _legendImage ?? (_legendImage = GetLegendImage());
            g.DrawImage(image, box, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, _imageAttributes);
        }

        /// <summary>
        /// Gets the size of the symbol to be drawn to the legend
        /// </summary>
        public override Size GetLegendSymbolSize()
        {
            return new Size(25, 25);
        }

        /// <summary>
        /// Gets whatever the child collection is and returns it as an IEnumerable set of legend items
        /// in order to make it easier to cycle through those values.
        /// </summary>
        public override IEnumerable<ILegendItem> LegendItems
        {
            get { yield break; }
        }

        /// <summary>
        /// Obtains an <see cref="Extent"/> in world coordinates that contains this object
        /// </summary>
        /// <returns></returns>
        public override DsExtent Extent
        {
            get
            {
                return _targetProjection != null 
                    ? MyExtent.Reproject(_sourceProjection, _targetProjection) 
                    : MyExtent;
            }
        }

        /// <summary>
        /// Führt anwendungsspezifische Aufgaben durch, die mit der Freigabe, der Zurückgabe oder dem Zurücksetzen von nicht verwalteten Ressourcen zusammenhängen.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        protected override void Dispose(bool managedResources)
        {
            if (IsDisposeLocked) 
                return;

            if (managedResources)
            {
            	((IDisposable)_configuration.TileFetcher).Dispose();
                _imageAttributes.Dispose();
            }

            base.Dispose(managedResources);
        }

        /// <summary>
        /// Reprojects all of the in-ram vertices of vectors, or else this
        /// simply updates the "Bounds" of image and raster objects
        /// This will also update the projection to be the specified projection.
        /// </summary>
        /// <param name="targetProjection">
        /// The projection information to reproject the coordinates to.
        /// </param>
        public override void Reproject(ProjectionInfo targetProjection)
        {
            base.Reproject(targetProjection);

            if (targetProjection != null)
            {
                //Set the target projection if necessary
                _targetProjection = targetProjection.Matches(_sourceProjection)
                    ? null
                    : targetProjection;
            }
            else
            {
                _targetProjection = null;
            }

            // Adjusting the projection
            Projection = _targetProjection ?? _sourceProjection;

            //Is this necessary?
            //MapFrame.Invalidate(Extent);
        }

        /// <summary>
        /// Gets or sets the transparency of the layer
        /// </summary>
        [Serialize("transparency")]
        public int Transparency
        {
            get { return _transparency; }
            set
            {
                if (value != _transparency)
                {
                    _transparency = value;
                    var ca = new ColorMatrix {Matrix33 = 1f - (value/100f)};
                    _imageAttributes.SetColorMatrix(ca, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                    OnItemChanged(EventArgs.Empty);
                    if (MapFrame != null)
                        MapFrame.Invalidate(MapFrame.ViewExtents);
                }
            }
        }

        private static BtExtent ToBrutileExtent(DsExtent extent)
        {
            return new BtExtent(extent.MinX, extent.MinY, extent.MaxX, extent.MaxY);
        }

        /*
        private static Extent FromBruTileExtent(BruTile.Extent extent)
        {
            return new Extent(extent.MinX, extent.MinY, extent.MaxX, extent.MaxY);
        }
         */
        //public Symbology.RasterSymbolizer RasterSymbolizer { get; set; }

        private readonly object _drawLock = new object();

        /// <summary>
        /// This draws content from the specified geographic regions onto the specified graphics
        /// object specified by MapArgs.
        /// </summary>
        public void DrawRegions(MapArgs args, List<DsExtent> regions)
        {

            // If this layer is not marked visible, exit
            if (!IsVisible) return;

            _stopwatch.Reset();

            var region = regions.FirstOrDefault() ?? args.GeographicExtents;

            if (!Monitor.TryEnter(_drawLock))
                return;
            
                LogManager.DefaultLogManager.LogMessage("MAP   : " + region, DialogResult.OK);

                // If we have a target projection, so project extent to providers extent
                var geoExtent = _targetProjection == null
                                    ? region
                                    : region.Intersection(Extent).Reproject(_targetProjection, _sourceProjection);

                LogManager.DefaultLogManager.LogMessage("SOURCE: " + geoExtent, DialogResult.OK);

                if (geoExtent.IsEmpty())
                {
                    LogManager.DefaultLogManager.LogMessage("Skipping because extent is empty!", DialogResult.OK);
                    Monitor.Exit(_drawLock);
                    return; 
                }

                BtExtent extent;
                try
                {
                     extent = ToBrutileExtent(geoExtent);
                }
                catch (Exception ex)
                {
                    LogManager.DefaultLogManager.Exception(ex);
                    Monitor.Exit(_drawLock);
                    return;
                }

                if (double.IsNaN(extent.Area))
                {
                    LogManager.DefaultLogManager.LogMessage("Skipping because extent is empty!", DialogResult.OK);
                    Monitor.Exit(_drawLock);
                    return;
                }

                var pixelSize = extent.Width/args.ImageRectangle.Width;

                var tileSource = _configuration.TileSource;
                var schema = tileSource.Schema;
                var level = _level = Utilities.GetNearestLevel(schema.Resolutions, pixelSize);
                
                _tileFetcher.Clear();
                var tiles = new List<TileInfo>(Sort(schema.GetTileInfos(extent, level), geoExtent.Center));
                var waitHandles = new List<WaitHandle>();
                var tilesNotImmediatelyDrawn = new List<TileInfo>();

                LogManager.DefaultLogManager.LogMessage(string.Format("Trying to get #{0} tiles: ", tiles.Count),
                                                        DialogResult.OK);

                // Set up Tile reprojector
                var tr = new TileReprojector(args, _sourceProjection, _targetProjection);


                var sw = new Stopwatch();
                sw.Start();

                // Store the current transformation
                var transform = args.Device.Transform;
                var resolution = schema.Resolutions[level];
                foreach (var info in tiles)
                {
                    var are = _tileFetcher.AsyncMode ? null : new AutoResetEvent(false);
                    var imageData = _tileFetcher.GetTile(info, are);
                    if (imageData != null)
                    {
                        //DrawTile
                        DrawTile(args, info, resolution, imageData, tr);
                        continue;
                    }
                    if (are == null) continue;

                    waitHandles.Add(are);
                    tilesNotImmediatelyDrawn.Add(info);
                }

                //Wait for tiles
                foreach (var handle in waitHandles)
                    handle.WaitOne();

                //Draw the tiles that were not present at the moment requested
                foreach (var tileInfo in tilesNotImmediatelyDrawn)
                    DrawTile(args, tileInfo, resolution, _tileFetcher.GetTile(tileInfo, null), tr);

                //Restore the transform
                args.Device.Transform = transform;

                sw.Stop();
                
                Debug.WriteLine("{0} ms", sw.ElapsedMilliseconds);
                Debug.Write(string.Format("Trying to render #{0} tiles: ", tiles.Count));

                LogManager.DefaultLogManager.LogMessage(string.Format("{0} ms", sw.ElapsedMilliseconds), DialogResult.OK);
                //if (InvalidRegion != null)
                //    MapFrame.Invalidate();

                //_stopwatch.Restart();
                Monitor.Exit(_drawLock);
        }

        private static IEnumerable<TileInfo> Sort(IEnumerable<TileInfo> enumerable, Coordinate coordinate)
        {
            var res = new SortedList<double, TileInfo>();
            foreach (var tileInfo in enumerable)
            {
                var dx = coordinate.X - tileInfo.Extent.CenterX;
                var dy = coordinate.Y - tileInfo.Extent.CenterY;
                var d = Math.Sqrt(dx*dx + dy*dy);
                while (res.ContainsKey(d)) d *= 1e-12;
                res.Add(d, tileInfo);
            }
            return res.Values;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void DrawTile(MapArgs args, TileInfo info, Resolution resolution, byte[] buffer, TileReprojector tr = null)
        {
            if (buffer == null || buffer.Length == 0)
                return;

            tr = tr ?? new TileReprojector(args, _sourceProjection, _targetProjection);

            using (var bitmap = (Bitmap)Image.FromStream(new MemoryStream(buffer)))
            {
                var inWorldFile = new WorldFile(resolution.UnitsPerPixel, 0, 
                                                0, -resolution.UnitsPerPixel,
                                                info.Extent.MinX, info.Extent.MaxY);

                WorldFile outWorldFile;
                Bitmap outBitmap;


                tr.Reproject(inWorldFile, bitmap, out outWorldFile, out outBitmap);
                if (outWorldFile == null) return;

                var lt = args.ProjToPixel(outWorldFile.ToGround(0, 0));
                var rb = args.ProjToPixel(outWorldFile.ToGround(outBitmap.Width, outBitmap.Height));
                var rect = new Rectangle(lt, Size.Subtract(new Size(rb), new Size(lt)));

                args.Device.DrawImage(outBitmap, rect, 0, 0, outBitmap.Width, outBitmap.Height,
                    GraphicsUnit.Pixel, _imageAttributes);

                if (outBitmap != bitmap) outBitmap.Dispose();
                if (FrameTile)
                {
                    if (FramePen != null)
                        args.Device.DrawRectangle(FramePen, rect);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating if each tile should be framed or not
        /// </summary>
        public bool FrameTile { get; set; }

        ///
        /// <summary>
        /// Gets or sets a value indicating the pen to use for the tile's frame
        /// </summary>
        public Pen FramePen { get; set; }

    }
}