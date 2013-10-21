using System;
using BruTile;
using BruTile.Cache;
using BruTile.FileSystem;
using DotSpatial.Serialization;

namespace DotSpatial.Plugins.BruTileLayer.Configuration
{
    [Serializable]
    public class FileTileLayerConfiguration : IConfiguration
    {
        [Serialize("minTiles", ConstructorArgumentIndex = 3)] 
        private readonly int _minTiles;
        [Serialize("maxTiles", ConstructorArgumentIndex = 4)]
        private readonly int _maxTiles;

        [Serialize("format", ConstructorArgumentIndex = 2)] 
        private readonly string _path;
        [Serialize("path", ConstructorArgumentIndex = 1)]
        private readonly string _format;

        [NonSerialized]
        private bool _initialized;

        [NonSerialized]
        private TileFetcher _tileFetcher;

        public FileTileLayerConfiguration(string name, string path, string format, int min, int max)
        {
            LegendText = name;

            _path = path;
            _format = format;
            _minTiles = min;
            _maxTiles = max;

            TileSource = new TileSource(
                new FileTileProvider(path, format, new TimeSpan(0)),
                new TileSchema());

            TileCache = TileFetcher.NoopCache.Instance;
        }

        public TileFetcher TileFetcher { get { return _tileFetcher; } }

        /// <summary>
        /// The tile source
        /// </summary>
        public ITileSource TileSource { get; private set; }

        /// <summary>
        /// The tile cache
        /// </summary>
        public ITileCache<byte[]> TileCache { get; private set; }

        /// <summary>
        /// The legend text
        /// </summary>
        [Serialize("name", ConstructorArgumentIndex = 0)]
        public string LegendText { get; private set; }

        /// <summary>
        /// Gets a deep copy of the configuration
        /// </summary>
        /// <returns>The cloned configuration</returns>
        public IConfiguration Clone()
        {
            return new FileTileLayerConfiguration(LegendText, _path, _format, _minTiles, _maxTiles);
        }

        /// <summary>
        /// Method called prior to any tile access
        /// </summary>
        public void Initialize()
        {
            if (_initialized) return;
            _initialized = true;

            _tileFetcher = new TileFetcher(TileSource.Provider,
                                           BruTileLayerPlugin.Settings.MemoryCacheMinimum,
                                           BruTileLayerPlugin.Settings.MemoryCacheMaximum,
                                           TileCache);
        }

    }
}