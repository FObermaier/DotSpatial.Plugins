using System;
using System.ComponentModel.Composition;
using System.IO;
using BruTile;
using BruTile.Cache;
using BruTile.Predefined;
using DotSpatial.Serialization;

namespace DotSpatial.Plugins.BruTileLayer.Configuration
{
    [Serializable, InheritedExport]
    public class KnownTileLayerConfiguration : PermaCacheConfiguration, IConfiguration
    {
        [Serialize("knownTileSource", ConstructorArgumentIndex = 2)] 
        private readonly KnownTileSource _knownTileSource;

        [Serialize("apiKey", ConstructorArgumentIndex = 3)]
        private readonly string _apiKey;

        [NonSerialized]
        private readonly TileFetcher _tileFetcher;

        public KnownTileLayerConfiguration(PermaCacheType permaCacheType, string fileCacheRoot,
            KnownTileSource tileSource, string apiKey)
            : base(permaCacheType, Path.Combine(BruTileLayerPlugin.Settings.PermaCacheRoot, tileSource.ToString()))
        {
            _knownTileSource = tileSource;
            _apiKey = apiKey;
            /*
            if (tileServers == KnownTileServers.Custom)
                throw new NotSupportedException();
            */

            TileSource = KnownTileSources.Create(tileSource, apiKey);
            TileCache = CreateTileCache();
            LegendText = tileSource.ToString();

            _tileFetcher = new TileFetcher(ReflectionHelper.Reflect(TileSource),
                                           BruTileLayerPlugin.Settings.MemoryCacheMinimum,
                                           BruTileLayerPlugin.Settings.MemoryCacheMaximum,
                                           TileCache);
        }

        //[Obsolete]
        public KnownTileLayerConfiguration(string fileCacheRoot, KnownTileSource tileSource, string apiKey) 
            : base(BruTileLayerPlugin.Settings.PermaCacheType, 
                   fileCacheRoot ?? Path.Combine(BruTileLayerPlugin.Settings.PermaCacheRoot , tileSource.ToString()))
        {
            _knownTileSource = tileSource;
            _apiKey = apiKey;
            /*
            if (tileServers == KnownTileServers.Custom)
                throw new NotSupportedException();
            */

            TileSource = KnownTileSources.Create(tileSource, apiKey);
            TileCache = CreateTileCache();
            LegendText = tileSource.ToString();

            _tileFetcher = new TileFetcher(ReflectionHelper.Reflect(TileSource),
                                           BruTileLayerPlugin.Settings.MemoryCacheMinimum,
                                           BruTileLayerPlugin.Settings.MemoryCacheMaximum,
                                           TileCache);
        }

        /// <summary>
        /// Gets or sets the legend text
        /// </summary>
        public string LegendText { get; set; }

        /// <summary>
        /// Gets the <see cref="IConfiguration.TileFetcher"/>
        /// </summary>
        public TileFetcher TileFetcher
        {
            get { return _tileFetcher; }
        }

        /// <summary>
        /// Gets a deep copy of the configuration
        /// </summary>
        /// <returns>The cloned configuration</returns>
        public IConfiguration Clone()
        {
            return new KnownTileLayerConfiguration(PermaCacheRoot, _knownTileSource, _apiKey);
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
        /// Method called prior to any tile access
        /// </summary>
        public void Initialize()
        {
        }

    }

    /*
    [Serializable]
    internal class CustomOsmLayerConfiguration : PermaCacheConfiguration, IConfiguration
    {
        [Serialize("url", ConstructorArgumentIndex = 2)]
        private readonly string _url;
        [Serialize("servers", ConstructorArgumentIndex = 3)]
        private readonly string[] _servers;
        [Serialize("minLevel", ConstructorArgumentIndex = 4)]
        private readonly int _minLevel;
        [Serialize("maxLevel", ConstructorArgumentIndex = 5)]
        private readonly int _maxLevel;

        [NonSerialized]
        private readonly TileFetcher _tileFetcher;

        public CustomOsmLayerConfiguration(string fileCacheRoot, string title, string url, string[] servers, int minLevel, int maxLevel)
            : base(BruTileLayerPlugin.Settings.PermaCacheType, fileCacheRoot)
        {
            LegendText = title;
            _url = url;
            _servers = servers ?? new string[0];
            _minLevel = minLevel;
            _maxLevel = maxLevel;

            TileSource
            var req = new WebRequest(KnownTileSources.Create(url, _servers.Length, _servers, minLevel, maxLevel));

            TileSource = new TileSource(req);
            TileCache = CreateTileCache();

            _tileFetcher = new TileFetcher(TileSource.Provider,
                                           BruTileLayerPlugin.Settings.MemoryCacheMinimum,
                                           BruTileLayerPlugin.Settings.MemoryCacheMaximum,
                                           TileCache) { AsyncMode = true};
        }

        /// <summary>
        /// Gets or sets the legend text
        /// </summary>
        [Serialize("Title", ConstructorArgumentIndex = 1)]
        public string LegendText { get; set; }

        /// <summary>
        /// Gets the <see cref="IConfiguration.TileFetcher"/>
        /// </summary>
        public TileFetcher TileFetcher
        {
            get { return _tileFetcher; }
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
        /// 
        /// </summary>
        /// <returns></returns>
        public IConfiguration Clone()
        {
            return new CustomOsmLayerConfiguration(PermaCacheRoot, LegendText, _url, _servers, _minLevel, _maxLevel);
        }

        /// <summary>
        /// Method called prior to any tile access
        /// </summary>
        public void Initialize()
        { }

    }
     */

}