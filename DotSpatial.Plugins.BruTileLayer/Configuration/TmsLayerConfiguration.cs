using System;
using System.Net;
using BruTile;
using BruTile.Cache;
using BruTile.Tms;
using DotSpatial.Serialization;

namespace DotSpatial.Plugins.BruTileLayer.Configuration
{
    [Serializable]
    public class TmsLayerConfiguration : PermaCacheConfiguration, IConfiguration
    {
        [Serialize("url", ConstructorArgumentIndex = 2)]
        private readonly string _url;

        [Serialize("overwriteUrls", ConstructorArgumentIndex = 4)]
        private readonly bool _overwriteUrls;
        [Serialize("inverted", ConstructorArgumentIndex = 3)]
        private readonly bool _inverted;

        [NonSerialized]
        private TileFetcher _tileFetcher;

       


        public TmsLayerConfiguration(string fileCacheRoot, string name, string url, bool inverted, bool overwriteUrls)
            : base(BruTileLayerPlugin.Settings.PermaCacheType, fileCacheRoot)
        {
            LegendText = name ?? "TmsLayer - " + new Uri(url).Host;
            _url = url;
            _inverted = inverted;
            _overwriteUrls = overwriteUrls;

            TileSource = CreateTileSource(_url, _overwriteUrls);
            if (inverted)
            {
                var schema = (TileSchema) TileSource.Schema;
                schema.YAxis = YAxis.TMS;
                schema.OriginY = Math.Abs(schema.OriginY);
            }
            TileCache = CreateTileCache();
        }

        private static ITileSource CreateTileSource(string url, bool overwriteUrls)
        {
            var request = (HttpWebRequest) WebRequest.Create(url);
            request.UserAgent =
                "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.1.14) Gecko/20080404 Firefox/2.0.0.14";
            var response = (HttpWebResponse) request.GetResponse();
            using (var stream = response.GetResponseStream())
            {
                return overwriteUrls ? 
                    TileMapParser.CreateTileSource(stream, url) : 
                    TileMapParser.CreateTileSource(stream);
            }
        }

        /// <summary>
        /// Gets the <see cref="IConfiguration.TileFetcher"/>
        /// </summary>
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
        [Serialize("name", ConstructorArgumentIndex = 1)]
        public string LegendText { get; private set; }

        /// <summary>
        /// Gets a deep copy of the configuration
        /// </summary>
        /// <returns>The cloned configuration</returns>
        public IConfiguration Clone()
        {
            return new TmsLayerConfiguration(PermaCacheRoot, LegendText, _url, _inverted, _overwriteUrls);
        }

        /// <summary>
        /// Method called prior to any tile access
        /// </summary>
        public void Initialize()
        {
            _tileFetcher = new TileFetcher(ReflectionHelper.Reflect(TileSource),
                                           BruTileLayerPlugin.Settings.MemoryCacheMinimum,
                                           BruTileLayerPlugin.Settings.MemoryCacheMaximum,
                                           TileCache);
        }

    }
}