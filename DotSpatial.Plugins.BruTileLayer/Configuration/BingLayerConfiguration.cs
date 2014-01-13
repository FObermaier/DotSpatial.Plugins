using System;
using BruTile;
using BruTile.Cache;
using BruTile.Web;
using DotSpatial.Serialization;

namespace DotSpatial.Plugins.BruTileLayer.Configuration
{
    [Serializable]
    public class BingLayerConfiguration : PermaCacheConfiguration, IConfiguration
    {
        [Serialize("BingMapType", ConstructorArgumentIndex = 1)]
        private readonly BingMapType _bingMapType;
        [Serialize("BingToken", ConstructorArgumentIndex = 2)]
        private readonly string _bingToken;

        [NonSerialized]
        private TileFetcher _tileFetcher;

        public BingLayerConfiguration(string fileCacheRoot, BingMapType bingMapType, string bingToken = null)
            : base(BruTileLayerPlugin.Settings.PermaCacheType, fileCacheRoot)
        {
            _bingMapType = bingMapType;
            _bingToken = bingToken;

            var url = string.IsNullOrEmpty(bingToken)
                          ? BingRequest.UrlBingStaging
                          : BingRequest.UrlBing;
            
            TileSource = new BingTileSource(url, bingToken, bingMapType);
            //TileSource.Schema.Resolutions.RemoveAt(0);
            //var s = TileSource.Schema as TileSchema;
            //s.Axis = AxisDirection.Normal;
            //s.OriginY = 0;
            //s.Extent = new Extent(s.Extent.MinX, 0, s.Extent.MaxX, s.Extent.MaxY);
            TileCache = CreateTileCache();
        }

        /// <summary>
        /// Gets the tile source
        /// </summary>
        public ITileSource TileSource { get; private set; }

        /// <summary>
        /// Gets the tile cache
        /// </summary>
        public ITileCache<byte[]> TileCache { get; private set; }

        /// <summary>
        /// Gets the <see cref="IConfiguration.TileFetcher"/>
        /// </summary>
        public TileFetcher TileFetcher
        {
            get { return _tileFetcher; }
        }

        /// <summary>
        /// The legend text
        /// </summary>
        public string LegendText { get { return _bingMapType.ToString(); } }

        /// <summary>
        /// Gets a deep copy of the configuration
        /// </summary>
        /// <returns>The cloned configuration</returns>
        public IConfiguration Clone()
        {
            return new BingLayerConfiguration(PermaCacheRoot, _bingMapType, _bingToken);
        }

        /// <summary>
        /// Method called prior to any tile access
        /// </summary>
        public void Initialize()
        {
            _tileFetcher = new TileFetcher(TileSource.Provider,
                                           BruTileLayerPlugin.Settings.MemoryCacheMinimum,
                                           BruTileLayerPlugin.Settings.MemoryCacheMaximum,
                                           TileCache);
        }
    }
}