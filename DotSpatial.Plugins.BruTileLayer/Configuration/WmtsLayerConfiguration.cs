using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using BruTile;
using BruTile.Cache;
using BruTile.Web;
using BruTile.Wmts;
using DotSpatial.Serialization;

namespace DotSpatial.Plugins.BruTileLayer.Configuration
{
    public class WmtsLayerConfiguration : PermaCacheConfiguration, IConfiguration
    {
        private TileFetcher _tileFetcher;
        private bool _initialized;

        [Serialize("fileCacheRoot", ConstructorArgumentIndex = 0)]
        private readonly string _fileCacheRoot;

        [Serialize("capabilitiesUri")] 
        private Uri _capabilitiesUri;

        public WmtsLayerConfiguration(string fileCacheRoot)
            : base(BruTileLayerPlugin.Settings.PermaCacheType, fileCacheRoot)
        {
            _fileCacheRoot = fileCacheRoot;
            _initialized = false;
        }

        internal static IEnumerable<ITileSource> GetTileSources(Uri uri)
        {
            try
            {
                var req = WebRequest.Create(uri);
                using (var resp = req.GetResponse())
                {
                    using (var s = resp.GetResponseStream())
                    {
                        var tileSources = WmtsParser.Parse(s);
                        if (tileSources.Any())
                            return tileSources;
                        return null;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
 

        public WmtsLayerConfiguration(string fileCacheRoot, string name, ITileSource tileSource)
            :base(BruTileLayerPlugin.Settings.PermaCacheType, fileCacheRoot)
        {
            _fileCacheRoot = fileCacheRoot;

            LegendText = name;

            //var tileSources = GetTileSources(capabilitiesUri);
            //var tileSource = tileSources.FirstOrDefault(ts => ts.Title.Equals(name, StringComparison.InvariantCulture));
            if (tileSource == null)
                throw new ArgumentException("TileSource not found", "capabilitiesUri");

            TileSource = tileSource;
            TileCache = CreateTileCache();

            _tileFetcher = new TileFetcher(tileSource.Provider, BruTileLayerPlugin.Settings.MemoryCacheMinimum,
                                           BruTileLayerPlugin.Settings.MemoryCacheMaximum, TileCache);
            _initialized = true;
        }

        private static string ToString(Resolution r)
        {
            var sb = new StringBuilder("{" + r.Id);
            sb.AppendFormat(NumberFormatInfo.InvariantInfo, ";{0}", r.UnitsPerPixel);
            sb.AppendFormat(NumberFormatInfo.InvariantInfo, ";{0}", r.ScaleDenominator);
            sb.AppendFormat(NumberFormatInfo.InvariantInfo, ";{0}", r.Left);
            sb.AppendFormat(NumberFormatInfo.InvariantInfo, ";{0}", r.Top);
            sb.AppendFormat(NumberFormatInfo.InvariantInfo, ";{0}", r.MatrixWidth);
            sb.AppendFormat(NumberFormatInfo.InvariantInfo, ";{0}", r.MatrixHeight);
            sb.AppendFormat(NumberFormatInfo.InvariantInfo, ";{0}", r.TileWidth);
            sb.AppendFormat(NumberFormatInfo.InvariantInfo, ";{0}", r.TileHeight);
            sb.Append("}");
            return sb.ToString();
        }

        private static Resolution FromString(string resolutionString)
        {
            if (resolutionString.StartsWith("{") &&
                resolutionString.EndsWith("}"))
            {
                resolutionString = resolutionString.Substring(1, resolutionString.Length - 2);
                var parts = resolutionString.Split(';');
                return new Resolution
                {
                    Id = parts[0],
                    UnitsPerPixel = double.Parse(parts[1], NumberFormatInfo.InvariantInfo),
                    Left = double.Parse(parts[2], NumberFormatInfo.InvariantInfo),
                    Top = double.Parse(parts[3], NumberFormatInfo.InvariantInfo),
                    MatrixWidth = int.Parse(parts[4], NumberFormatInfo.InvariantInfo),
                    MatrixHeight = int.Parse(parts[5], NumberFormatInfo.InvariantInfo),
                    TileWidth = int.Parse(parts[6], NumberFormatInfo.InvariantInfo),
                    TileHeight = int.Parse(parts[7], NumberFormatInfo.InvariantInfo),
                };
            }
            throw new ArgumentException("Not a valid resolution string", "resolutionString");
        }

        [Serialize("title")]
        public string LegendText { get; private set; }

        public ITileSource TileSource { get; private set; }

        public ITileCache<byte[]> TileCache { get; private set; }

        public TileFetcher TileFetcher { get { return _tileFetcher; } }

        public IConfiguration Clone()
        {
            return new WmtsLayerConfiguration(_fileCacheRoot);
        }

        public void Initialize()
        {
            if (_initialized) return;
            _initialized = true;


            var tileSources = GetTileSources(_capabilitiesUri);
            var tileSource = tileSources.FirstOrDefault(ts => ts.Title.Equals(LegendText, StringComparison.InvariantCulture));
            if (tileSource == null)
                throw new ArgumentException("TileSource not found", "capabilitiesUri");

            TileSource = tileSource;
            var provider = ((WebTileProvider)tileSource.Provider);

            TileCache = CreateTileCache();

            _tileFetcher = new TileFetcher(TileSource.Provider,
                                           BruTileLayerPlugin.Settings.MemoryCacheMinimum,
                                           BruTileLayerPlugin.Settings.MemoryCacheMaximum,
                                           TileCache);
        }
    }
}