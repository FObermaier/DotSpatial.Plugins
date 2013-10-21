// ********************************************************************************************************
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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using BruTile;
using BruTile.Web;
using DotSpatial.Controls;
using DotSpatial.Data;
using DotSpatial.Data.Forms;
using DotSpatial.Plugins.BruTileLayer.Configuration;
using DotSpatial.Plugins.BruTileLayer.Configuration.Forms;
using DotSpatial.Plugins.BruTileLayer.Properties;
using DotSpatial.Projections;
using DotSpatial.Projections.AuthorityCodes;
using DotSpatial.Serialization;
using DotSpatial.Symbology;
using DotSpatial.Topology;
using Extent = DotSpatial.Data.Extent;

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
            var config = new BingLayerConfiguration(null, BingMapType.Roads, token);
            return new BruTileLayer(config);
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
            var config = new BingLayerConfiguration(null, BingMapType.Hybrid, token);
            return new BruTileLayer(config);
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
            var config = new BingLayerConfiguration(null, BingMapType.Aerial, token);
            return new BruTileLayer(config);
        }

        /// <summary>
        /// Creates an OpenStreetMap layer
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public static BruTileLayer CreateOsmMapnicLayer()
        {
            var config = new OsmLayerConfiguration(null, KnownOsmTileServers.Mapnik, string.Empty);
            return new BruTileLayer(config);
        }

        #region Fields

        //[NonSerialized]
        //private readonly List<SymbologyMenuItem> _contextMenuItems = 
        //    new List<SymbologyMenuItem>();
        private ProjectionInfo _projectionInfo;
        [Serialize("targetProjection")]
        private ProjectionInfo _targetProjection;
        [Serialize("config", ConstructorArgumentIndex = 0)]
        private readonly IConfiguration _configuration;

        [NonSerialized]
        private Image _legendImage;
        private int _transparency;
        
        [NonSerialized]
        private readonly TileFetcher _tileFetcher;

        [Serialize("projection")]
// ReSharper disable InconsistentNaming
        private ProjectionInfo _projection
// ReSharper restore InconsistentNaming
        {
            get { return _projectionInfo; }
            set
            {
                if (value == null)
                    return;
                if (value.Transform == null)
                    return;
                _projectionInfo = value;
            }
        }

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
            : this(new OsmLayerConfiguration(null, KnownOsmTileServers.Mapnik, string.Empty))
        { }

        /// <summary>
        /// Creates an instanc of this class using the given <paramref name="configuration"/>
        /// </summary>
        /// <param name="configuration">The configuration</param>
        public BruTileLayer(IConfiguration configuration)
        {
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
            _projectionInfo = AuthorityCodeHandler.Instance[tileSource.Schema.Srs];
            if (_projectionInfo == null)
                _projectionInfo = AuthorityCodeHandler.Instance["EPSG:3857"];

            Projection = _projection;
            var extent = tileSource.Schema.Extent;
            MyExtent = new Extent(extent.MinX, extent.MinY, extent.MaxX, extent.MaxY);

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

        private void HandleTileReceived(object sender, TileReceivedEventArgs e)
        {
            var i = e.TileInfo.Index;
            System.Diagnostics.Debug.WriteLine("Tile received (Index({0}, {1}, {2}))", i.Level, i.Row, i.Col);

            var ext = ToBrutileExtent(MapFrame.ViewExtents);
            if (ext.Intersects(e.TileInfo.Extent))
            {
                Invalidate(FromBruTileExtent(e.TileInfo.Extent));
                //Invalidate();
            }
        }

        private void HandleQueueEmpty(object sender, EventArgs empty)
        {
            Invalidate();
        }

        protected override void OnShowProperties(HandledEventArgs e)
        {
            using (var frm = new BruTileLayerDisplayProperties())
            {
                frm.BruTileLayer = this;
                if (frm.ShowDialog() == DialogResult.OK)
                    frm.BruTileLayer.Invalidate();
            }
        }

        private static Extent FromBruTileExtent(BruTile.Extent extent)
        {
            return new Extent(extent.MinX, extent.MinY, extent.MaxX, extent.MaxY);
        }
        ///// <summary>
        ///// Tests this object against the comparison object.  If any of the 
        ///// value type members are different, or if any of the properties
        ///// are IMatchable and do not match, then this returns false.
        ///// </summary>
        ///// <param name="other">The other IMatcheable object of the same type</param>
        ///// <param name="mismatchedProperties">The list of property names that do not match</param>
        ///// <returns>Boolean, true if the properties are comparably equal.</returns>
        //public bool Matches(IMatchable other, out List<string> mismatchedProperties)
        //{
        //    var matches = true;

        //    mismatchedProperties = new List<string>();
        //    var otherBruTileLayer = other as BruTileLayer;
        //    if (otherBruTileLayer == null)
        //    {
        //        AddAllProperties(mismatchedProperties);
        //        return false;
        //    }

        //    if (!ReferenceEquals(_configuration.TileSource, otherBruTileLayer._configuration.TileSource))
        //    {
        //        mismatchedProperties.Add("TileSource");
        //        matches = false;
        //    }

        //    if (!ReferenceEquals(_configuration.TileCache, otherBruTileLayer._configuration.TileCache))
        //    {
        //        mismatchedProperties.Add("TileCache");
        //        matches = false;
        //    }

        //    if (ShowErrorInTile != otherBruTileLayer.ShowErrorInTile)
        //    {
        //        mismatchedProperties.Add("ShowErrorInTile");
        //        matches = false;
        //    }

        //    return matches;
        //}

        //private static void AddAllProperties(List<string> properties)
        //{
        //    properties.AddRange(new[] { "TileSource", "TileCache", "ShowErrorsInTile" });
        //}

        ///// <summary>
        ///// This method will set the values for this class with random values that are
        ///// within acceptable parameters for this class.
        ///// </summary>
        ///// <param name="generator">An existing random number generator so that the random seed can be controlled</param>
        //public void Randomize(Random generator)
        //{
        //}

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

        ///// <summary>
        ///// This copies the public descriptor properties from the specified object to
        ///// this object.
        ///// </summary>
        ///// <param name="other">An object that has properties that match the public properties on this object.</param>
        //public override void CopyProperties(object other)
        //{
        //    var otherBrutileLayer = other as BruTileLayer;
        //    if (otherBrutileLayer == null)
        //        throw new ArgumentException();

        //    otherBrutileLayer.ShowErrorInTile = ShowErrorInTile;
        //    otherBrutileLayer.Transparency = Transparency;
        //    otherBrutileLayer.UseDynamicVisibility = otherBrutileLayer.UseDynamicVisibility;
        //}

        ///// <inheritdoc/>
        //public event EventHandler ItemChanged;

        //private void OnItemChanged(EventArgs e)
        //{
        //    if (ItemChanged != null)
        //        ItemChanged(this, e);
        //}

        ///// <inheritdoc/>
        //public event EventHandler RemoveItem;
        
        //private void OnRemoveItem(EventArgs e)
        //{
        //    if (RemoveItem != null)
        //        RemoveItem(this, e);
        //}

        //private ILegendItem _parentItem;
        ///// <summary>
        ///// Gets the parent item relative to this item.
        ///// </summary>
        //public ILegendItem GetParentItem()
        //{
        //    return _parentItem;
        //}

        ///// <summary>
        ///// Sets teh parent legend item for this item
        ///// </summary>
        ///// <param name="value"></param>
        //public void SetParentItem(ILegendItem value)
        //{
        //    _parentItem = value;
        //}

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
            var tiles = new List<TileInfo>(schema.GetTilesInView(schema.Extent, 0));
            if (tiles.Count <= 4)
            {
                //Ratio of width to height for each tile 
                double ratio = (double) schema.Width/schema.Height;
                int width = (int) Math.Round(height*ratio, MidpointRounding.ToEven);
                var result = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                var tileHeight = tiles.Count == 4 ? height/2 : height;
                var tileWidth = tiles.Count == 4 ? width/2 : width;
                using (var g = Graphics.FromImage(result))
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    
                    var ia = new ImageAttributes();
                    ia.SetWrapMode(WrapMode.TileFlipXY);

                    foreach (var ti in tiles)
                    {
                        var image = GetTileImage(ti);
                        switch (schema.Axis)
                        {
                            case AxisDirection.InvertedY:
                                g.DrawImage(image, ti.Index.Col*tileWidth, ti.Index.Row*tileHeight, tileWidth,
                                            tileHeight);
                                break;
                            case AxisDirection.Normal:
                                g.DrawImage(image, ti.Index.Col*tileWidth, height - (ti.Index.Row+1)*tileHeight, tileWidth,
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
            var buffer = _configuration.TileSource.Provider.GetTile(tileInfo);
            return Image.FromStream(new MemoryStream(buffer));
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

        ///// <summary>
        ///// Prints the formal legend content without any resize boxes or other notations.
        ///// </summary>
        ///// <param name="g">The graphics object to print to</param>
        ///// <param name="font">The system.Drawing.Font to use for the lettering</param>
        ///// <param name="fontColor">The color of the font</param>
        ///// <param name="maxExtent">Assuming 0, 0 is the top left, this is the maximum extent</param>
        //public void PrintLegendItem(Graphics g, Font font, Color fontColor, SizeF maxExtent)
        //{
        //    g.DrawString(_configuration.TileSource.Schema.Name, font, new SolidBrush(fontColor), new RectangleF(new PointF(0, 0), maxExtent));
        //}

        /// <summary>
        /// Gets the size of the symbol to be drawn to the legend
        /// </summary>
        public override Size GetLegendSymbolSize()
        {
            return new Size(25, 25);
        }

        ///// <summary>
        ///// This is a list of menu items that should appear for this layer.
        ///// These are in addition to any that are supported by default.
        ///// Handlers should be added to this list before it is retrieved.
        ///// </summary>
        //public List<SymbologyMenuItem> ContextMenuItems
        //{
        //    get { return _contextMenuItems; }
        //    set { }
        //}

        ///// <summary>
        ///// Gets or sets whether or not this legend item should be visible.
        ///// This will not be altered unless the LegendSymbolMode is set
        ///// to CheckBox.
        ///// </summary>
        //public bool Checked
        //{
        //    get { return IsVisible; }
        //    set { IsVisible = value; }
        //}

        ///// <summary>
        ///// Gets or sets whether this legend item is expanded.
        ///// </summary>
        //public bool IsExpanded { get; set; }

        ///// <summary>
        ///// Gets or sets whether this legend item is currently selected (and therefore drawn differently)
        ///// </summary>
        //public bool IsSelected { get; set; }

        /// <summary>
        /// Gets whatever the child collection is and returns it as an IEnumerable set of legend items
        /// in order to make it easier to cycle through those values.
        /// </summary>
        public override IEnumerable<ILegendItem> LegendItems
        {
            get { yield break; }
        }

        ///// <summary>
        ///// Gets or sets a boolean, that if false will prevent this item, or any of its child items
        ///// from appearing in the legend when the legend is drawn.
        ///// </summary>
        //public bool LegendItemVisible { get; set; }

        ///// <summary>
        ///// Gets the symbol mode for this legend item.
        ///// </summary>
        //public SymbolMode LegendSymbolMode
        //{
        //    get { return SymbolMode.Symbol; }
        //}

        ///// <summary>
        ///// The text that will appear in the legend 
        ///// </summary>
        //public string LegendText { get; set; }

        ///// <summary>
        ///// Gets or sets a pre-defined behavior in the legend when referring to drag and drop functionality.
        ///// </summary>
        //public LegendType LegendType
        //{
        //    get { return LegendType.Custom; }
        //}

        ///// <inheritdoc/>
        //public event EventHandler<EnvelopeArgs> EnvelopeChanged;
        
        ///// <inheritdoc/>
        //public event EventHandler Invalidated;
        
        ///// <inheritdoc/>
        //public event EventHandler VisibleChanged;

        ///// <summary>
        ///// Invalidates the drawing methods
        ///// </summary>
        //public void Invalidate()
        //{
        //    if (Invalidated != null)
        //        Invalidated(this, EventArgs.Empty);
        //}

        /// <summary>
        /// Obtains an <see cref="Extent"/> in world coordinates that contains this object
        /// </summary>
        /// <returns></returns>
        public override Extent Extent
        {
            get
            {
                return _targetProjection != null 
                    ? ReprojectExtent(MyExtent, Projection, _targetProjection) 
                    : MyExtent;
            }
        }

        ///// <summary>
        ///// Gets whether or not the unmanaged drawing structures have been created for this item
        ///// </summary>
        //public bool IsInitialized
        //{
        //    get { return true; }
        //}

        ///// <summary>
        ///// If this is false, then the drawing function will not render anything.
        ///// Warning!  This will also prevent any execution of calculations that take place
        ///// as part of the drawing methods and will also abort the drawing methods of any
        ///// sub-members to this IRenderable.
        ///// </summary>
        //public bool IsVisible
        //{
        //    get { return _isVisible; }
        //    set
        //    {
        //        _isVisible = value;
        //        OnVisibleChanged();
        //    }
        //}

        //private void OnVisibleChanged()
        //{
        //    var e = VisibleChanged;
        //    if (e != null) e(this, EventArgs.Empty);
        //}

        ///// <inheritdoc/>
        //public event EventHandler SelectionChanged;

        ///// <summary>
        ///// Removes any members from existing in the selected state
        ///// </summary>
        //public bool ClearSelection(out IEnvelope affectedArea)
        //{
        //    affectedArea = null;
        //    return false;
        //}

        ///// <summary>
        ///// Inverts the selected state of any members in the specified region.
        ///// </summary>
        ///// <param name="tolerant">The geographic region to invert the selected state of members</param>
        ///// <param name="strict">The geographic region when working with absolutes, without a tolerance</param>
        ///// <param name="mode">The selection mode determining how to test for intersection</param>
        ///// <param name="affectedArea">The geographic region encapsulating the changed members</param>
        ///// <returns>Boolean, true if members were changed by the selection process.</returns>
        //public bool InvertSelection(IEnvelope tolerant, IEnvelope strict, SelectionMode mode, out IEnvelope affectedArea)
        //{
        //    affectedArea = null;
        //    return false;
        //}

        ///// <summary>
        ///// Adds any members found in the specified region to the selected state as long as
        ///// SelectionEnabled is set to true.
        ///// </summary>
        ///// <param name="tolerant">The geographic region where selection occurs</param>
        ///// <param name="strict">The geographic region when working with absolutes, without a tolerance</param>
        ///// <param name="mode">The selection mode</param>
        ///// <param name="affectedArea">The envelope affected area</param>
        ///// <returns>Boolean, true if any members were added to the selection</returns>
        //public bool Select(IEnvelope tolerant, IEnvelope strict, SelectionMode mode, out IEnvelope affectedArea)
        //{
        //    affectedArea = null;
        //    return false;
        //}

        ///// <summary>
        ///// Removes any members found in the specified region from the selection
        ///// </summary>
        ///// <param name="tolerant">The geographic region to investigate</param>
        ///// <param name="strict">The geographic region when working with absolutes, without a tolerance</param>
        ///// <param name="mode">The selection mode to use for selecting items</param>
        ///// <param name="affectedArea">The geographic region containing all the shapes that were altered</param>
        ///// <returns>Boolean, true if any members were removed from the selection</returns>
        //public bool UnSelect(IEnvelope tolerant, IEnvelope strict, SelectionMode mode, out IEnvelope affectedArea)
        //{
        //    affectedArea = null;
        //    return false;
        //}

        ///// <summary>
        ///// Gets or sets the Boolean indicating whether this item is actively supporting selection
        ///// </summary>
        //public bool SelectionEnabled
        //{
        //    get { return false; }
        //    set { }
        //}

        ///// <summary>
        ///// Dynamic visibility represents layers that only appear when you zoom in close enough.
        ///// This value represents the geographic width where that happens.
        ///// </summary>
        //public double DynamicVisibilityWidth { get; set; }

        ///// <summary>
        ///// This controls whether the layer is visible when zoomed in closer than the dynamic 
        ///// visiblity width or only when further away from the dynamic visibility width
        ///// </summary>
        //public DynamicVisibilityMode DynamicVisibilityMode { get; set; }

        ///// <summary>
        ///// Gets or sets a boolean indicating whether dynamic visibility should be enabled.
        ///// </summary>
        //public bool UseDynamicVisibility
        //{
        //    get { return false; }
        //    set { }
        //}

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
                _configuration.TileFetcher.Dispose();
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
            if (targetProjection == null)
                return;
            
            //Set the target projection if necessary
            _targetProjection = targetProjection.Matches(Projection) 
                ? null : 
                _targetProjection;
            
            //Is this necessary?
            Invalidate(Extent);
        }

        ///// <inheritdoc/>
        //public event HandledEventHandler ShowProperties;
        ///// <inheritdoc/>
        //public event EventHandler<EnvelopeArgs> ZoomToLayer;
        ///// <inheritdoc/>
        //public event EventHandler<LayerSelectedEventArgs> LayerSelected;
        ///// <inheritdoc/>
        //public event EventHandler FinishedLoading;

        ///// <summary>
        ///// Given a geographic extent, this tests the "IsVisible", "UseDynamicVisibility", 
        ///// "DynamicVisibilityMode" and "DynamicVisibilityWidth"
        ///// In order to determine if this layer is visible. 
        ///// </summary>
        ///// <param name="geographicExtent">The geographic extent, where the width will be tested.</param>
        ///// <returns>Boolean, true if this layer should be visible for this extent.</returns>
        //public bool VisibleAtExtent(Extent geographicExtent)
        //{
        //    return Extent.Intersects(geographicExtent);
        //}

        ///// <summary>
        ///// Notifies the layer that the next time an area that intersects with this region
        ///// is specified, it must first re-draw content to the image buffer.
        ///// </summary>
        ///// <param name="region">The envelope where content has become invalidated.</param>
        //public void Invalidate(Extent region)
        //{

        //}

        ///// <summary>
        ///// Queries this layer and the entire parental tree up to the map frame to determine if
        ///// this layer is within the selected layers.
        ///// </summary>
        //public bool IsWithinLegendSelection()
        //{
        //    return true;
        //}

        ///// <summary>
        ///// Gets or sets the core dataset for this layer.
        ///// </summary>
        //public IDataSet DataSet
        //{
        //    get { return null; }
        //    set { }
        //}

        ///// <summary>
        ///// Gets the currently invalidated region.
        ///// </summary>
        //public Extent InvalidRegion
        //{
        //    get
        //    {
        //        if (MapFrame != null && MapFrame.ViewExtents != null)
        //            return MapFrame.ViewExtents;
        //        return null;
        //    }
        //}

        ///// <summary>
        ///// Gets the MapFrame that contains this layer.
        ///// </summary>
        //public IFrame MapFrame { get; set; }

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
                        Invalidate(MapFrame.ViewExtents);
                }
            }
        }

        private static BruTile.Extent ToBrutileExtent(Extent extent)
        {
            return new BruTile.Extent(extent.MinX, extent.MinY, extent.MaxX, extent.MaxY);
        }

        /*
        private static Extent FromBruTileExtent(BruTile.Extent extent)
        {
            return new Extent(extent.MinX, extent.MinY, extent.MaxX, extent.MaxY);
        }
         */
        //public Symbology.RasterSymbolizer RasterSymbolizer { get; set; }

        private static Extent ReprojectExtent(Extent extent, ProjectionInfo source, ProjectionInfo target)
        {
            var xy = new[]
                {
                    extent.MinX, extent.MinY, extent.MaxX, extent.MinY,
                    extent.MaxX, extent.MaxY, extent.MinX, extent.MaxY
                };
            Projections.Reproject.ReprojectPoints(xy, null, source, target, 0, 4);

            var s = new ShapeRange(FeatureType.MultiPoint);
            s.SetVertices(xy);
            return s.CalculateExtents();
        }

        private readonly object _drawLock = new object();

        /// <summary>
        /// This draws content from the specified geographic regions onto the specified graphics
        /// object specified by MapArgs.
        /// </summary>
        public void DrawRegions(MapArgs args, List<Extent> regions)
        {

            // If this layer is not marked visible, exit
            if (!IsVisible) return;


            lock (_drawLock)
            {
                //We have a target projection, so project extent to providers extent
                var geoExtent = _targetProjection == null
                                    ? args.GeographicExtents
                                    : ReprojectExtent(args.GeographicExtents, _targetProjection, _projectionInfo);

                var extent = ToBrutileExtent(geoExtent);

                var pixelSize = extent.Width/args.ImageRectangle.Width;

                var tileSource = _configuration.TileSource;
                var schema = tileSource.Schema;
                var level = Utilities.GetNearestLevel(schema.Resolutions, pixelSize);
                var tiles = new List<TileInfo>(Sort(schema.GetTilesInView(extent, level), geoExtent.Center));
                var waitHandles = new List<WaitHandle>();
                var tilesNotImmediatelyDrawn = new List<TileInfo>();

                LogManager.DefaultLogManager.LogMessage(string.Format("Trying to get #{0} tiles: ", tiles.Count),
                                                        DialogResult.OK);

                var sw = new System.Diagnostics.Stopwatch();
                sw.Start();

                // Store the current transformation
                var transform = args.Device.Transform;

                foreach (var info in tiles)
                {
                    var are = _tileFetcher.AsyncMode ? null : new AutoResetEvent(false);
                    var imageData = _tileFetcher.GetTile(info, are);
                    if (imageData != null)
                    {
                        //DrawTile
                        DrawTile(args, schema, info, imageData);
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
                    DrawTile(args, schema, tileInfo, _tileFetcher.GetTile(tileInfo, null));

                //Restore the transform
                args.Device.Transform = transform;

                sw.Stop();
                System.Diagnostics.Debug.WriteLine(string.Format("{0} ms", sw.ElapsedMilliseconds));
                System.Diagnostics.Debug.Write(string.Format("Trying to render #{0} tiles: ", tiles.Count));

                LogManager.DefaultLogManager.LogMessage(string.Format("{0} ms", sw.ElapsedMilliseconds), DialogResult.OK);
                //if (InvalidRegion != null)
                //    Invalidate();
            }
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

        private void DrawTile(MapArgs args, ITileSchema schema, TileInfo info, byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0)
                return;

            /*
            // Points of the tile
            // Left, Top
            var s0 = new PointF(0, 0);
            // Right, Top
            var s1 = new PointF(schema.Width, 0);
            //Right, Bottom
            var s2 = new PointF(schema.Width, schema.Height);
            */

                using (var bitmap = Image.FromStream(new MemoryStream(buffer)))
                {
                    var btExtent = info.Extent;
                    var tileExtent = new Extent(btExtent.MinX, btExtent.MinY, btExtent.MaxX, btExtent.MaxY);

                    // Do we need to reproject the tiles?
                    if (_targetProjection != null)
                    {
                        // Reproject LeftTop, RightTop, RightBottom
                        var xy = new[]
                            {
                                tileExtent.MinX, tileExtent.MaxY,
                                tileExtent.MaxX, tileExtent.MaxY,
                                tileExtent.MaxX, tileExtent.MinY
                            };
                        Projections.Reproject.ReprojectPoints(xy, null, _projectionInfo, _targetProjection, 0, 4);

                        // Project them to the device
                        var t0 = args.ProjToPixel(new Coordinate(xy[0], xy[1]));
                        var t1 = args.ProjToPixel(new Coordinate(xy[2], xy[3]));
                        var t2 = args.ProjToPixel(new Coordinate(xy[4], xy[5]));

                        /*
                        // Build the affine transformation
                        var atb = new AffineTransformationBuilder(s0, s1, s2, t0, t1, t2);

                        var tmpTransform = transform.Clone();
                        tmpTransform.Multiply(atb.GetTransformation(), MatrixOrder.Append);
                        */

                        // Draw the tile
                        args.Device.DrawImage(bitmap, new[] { t0, t1, t2 },
                                    new Rectangle(0, 0, schema.Width, schema.Height),
                                    GraphicsUnit.Pixel, _imageAttributes);
                    }
                    else
                    {
                        var minC = new Coordinate(info.Extent.MinX, info.Extent.MinY);
                        var maxC = new Coordinate(info.Extent.MaxX, info.Extent.MaxY);

                        PointF min = args.ProjToPixel(minC);
                        PointF max = args.ProjToPixel(maxC);

                        min = new PointF((float)Math.Round(min.X), (float)Math.Round(min.Y));
                        max = new PointF((float)Math.Round(max.X), (float)Math.Round(max.Y));

                        args.Device.DrawImage(bitmap,
                                    new Rectangle((int)min.X, (int)max.Y, (int)(max.X - min.X), (int)(min.Y - max.Y)),
                                    0, 0, schema.Width, schema.Height,
                                GraphicsUnit.Pixel, _imageAttributes);
                    }
                }
        }

        ///// <summary>
        ///// Worker method
        ///// </summary>
        ///// <param name="parameter">The parameters</param>
        //private static void GetTileOnThread(object parameter)
        //{
        //    var parameters = (object[])parameter;
        //    if (parameters.Length != 6) throw new ArgumentException("Six parameters expected");
        //    var schema = (ITileSchema)parameters[0];
        //    var tileProvider = (ITileProvider)parameters[1];
        //    var tileInfo = (TileInfo)parameters[2];
        //    var bitmaps = (ITileCache<byte[]>)parameters[3];
        //    var autoResetEvent = (AutoResetEvent)parameters[4];
        //    var showErrorInTile = (bool) parameters[5];

        //    try
        //    {
        //        byte[] bytes = tileProvider.GetTile(tileInfo);
        //        //Bitmap bitmap = new Bitmap(new MemoryStream(bytes));
        //        //bitmaps.Add(tileInfo.Index, bitmap);
        //        bitmaps.Add(tileInfo.Index, bytes);
        //    }
        //    catch (WebException ex)
        //    {
        //        if (showErrorInTile)
        //        {
        //            //an issue with this method is that one an error tile is in the memory cache it will stay even
        //            //if the error is resolved. PDD.
        //            var bitmap = new Bitmap(schema.Width, schema.Height);
        //            using (Graphics graphics = Graphics.FromImage(bitmap))
        //            {
        //                graphics.DrawString(ex.Message, new Font(FontFamily.GenericSansSerif, 12),
        //                                    new SolidBrush(Color.Black),
        //                                    new RectangleF(0, 0, schema.Width, schema.Height));
        //            }
        //            //bitmaps.Add(tileInfo.Index, bitmap);
        //            using (var m = new MemoryStream())
        //            {
        //                bitmap.Save(m, ImageFormat.Png);
        //                bitmaps.Add(tileInfo.Index, m.ToArray());
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManager.DefaultLogManager.Exception(ex);
        //    }
        //    finally
        //    {
        //        autoResetEvent.Set();
        //    }
        //}


    }
}