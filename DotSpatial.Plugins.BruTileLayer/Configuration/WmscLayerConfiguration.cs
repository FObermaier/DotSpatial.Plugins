using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BruTile;
using BruTile.Cache;
using BruTile.Web;
using BruTile.Wmsc;
using DotSpatial.Serialization;

namespace DotSpatial.Plugins.BruTileLayer.Configuration
{
    public class WmscLayerConfiguration : PermaCacheConfiguration, IConfiguration
    {
        [Serialize("fileCacheRoot", ConstructorArgumentIndex = 0)]
        private readonly string _fileCacheRoot;

        [Serialize("url")]
        private string _url;
        //
        //needed for schema generation
        [Serialize("axis")]
        private YAxis _axis;
        [Serialize("minX")]
        private double _minX;
        [Serialize("minY")]
        private double _minY;
        [Serialize("maxX")]
        private double _maxX;
        [Serialize("maxY")]
        private double _maxY;
        [Serialize("format")]
        private string _format;
        [Serialize("height")]
        private int _height;
        [Serialize("schemaName")]
        private string _schemaName;
        [Serialize("originX")]
        private double _originX;
        [Serialize("originY")]
        private double _originY;
        [Serialize("resolutions")]
        private Dictionary<string, Resolution> _resolutions;
        [Serialize("srs")]
        private string _srs;
        [Serialize("width")]
        private int _width;

        [Serialize("layers")]
        private List<String> _layers;
        [Serialize("styles")]
        private List<String> _styles;
        [Serialize("customParameters")]
        private Dictionary<string, string> _customParameters;
        [Serialize("version")]
        private string _version;
        
        private bool _initialized;
        private TileFetcher _tileFetcher;


        public WmscLayerConfiguration(string fileCacheRoot)
            : base(BruTileLayerPlugin.Settings.PermaCacheType, fileCacheRoot)
        {
            _fileCacheRoot = fileCacheRoot;
            _initialized = false;
        }


        public WmscLayerConfiguration(string fileCacheRoot, string name, WmscTileSource source)
            : base(BruTileLayerPlugin.Settings.PermaCacheType, fileCacheRoot)
        {
            _fileCacheRoot = fileCacheRoot;
            LegendText = name;

            var provider = ReflectionHelper.Reflect(source) as HttpTileProvider;
            if (provider == null)
                throw new ArgumentException("Source does not have a WebTileProvider", "source");

            var request = ReflectionHelper.ReflectRequest<WmscRequest>(provider);
            if (request == null)
                throw new ArgumentException("Source does not have a WmscRequest" ,"source");

            SafeRequest(request);
            SafeSchema(source.Schema);

            TileSource = source;
            TileCache = CreateTileCache();

            _tileFetcher = new TileFetcher(provider, BruTileLayerPlugin.Settings.MemoryCacheMinimum,
                                           BruTileLayerPlugin.Settings.MemoryCacheMaximum, TileCache);
            _initialized = true;
        }

        private void SafeRequest(WmscRequest request)
        {
            _url = ReflectionHelper.ReflectBaseUri(request).ToString();
            _layers = new List<string>(ReflectionHelper.ReflectListItems(request, "_layers"));
            _styles = new List<string>(ReflectionHelper.ReflectListItems(request, "_styles"));
            _customParameters = new Dictionary<string, string>(ReflectionHelper.ReflectDictionary(request, "_customParameters"));
            _version = ReflectionHelper.Reflect<string>(request, "_version");
        }
        private void SafeSchema(ITileSchema schema)
        {
            _axis = schema.YAxis;
            _minX = schema.Extent.MinX;
            _maxX = schema.Extent.MaxX;
            _minY = schema.Extent.MinY;
            _maxY = schema.Extent.MaxY;
            _format = schema.Format;
            _height = schema.GetTileHeight(String.Empty);
            _width = schema.GetTileWidth(String.Empty);
            _schemaName = schema.Name;
            _originX = schema.GetOriginX(String.Empty);
            _originY = schema.GetOriginY(String.Empty);
            _srs = schema.Srs;
            _resolutions = new Dictionary<string, Resolution>();
            foreach (var resolution in schema.Resolutions)
                _resolutions.Add(resolution.Key, resolution.Value);
        }

        /// <summary>
        /// Gets the <see cref="IConfiguration.TileFetcher"/>
        /// </summary>
        public TileFetcher TileFetcher { get { return _tileFetcher; } }

        /// <summary>
        /// Method called prior to any tile access
        /// </summary>
        public void Initialize()
        {
            if (_initialized) return;
            _initialized = true;

            var schema = RestoreSchema();
            var request = new WmscRequest(new Uri(_url), schema, _layers, _styles, _customParameters, _version);
            
            ITileProvider provider = new HttpTileProvider(request);
            TileSource = (WmscTileSource)Activator.CreateInstance(typeof(WmscTileSource), BindingFlags.NonPublic, schema, provider);
            TileCache = CreateTileCache();
            _tileFetcher = new TileFetcher(provider,
                                           BruTileLayerPlugin.Settings.MemoryCacheMinimum,
                                           BruTileLayerPlugin.Settings.MemoryCacheMaximum,
                                           TileCache);

        }


        private ITileSchema RestoreSchema()
        {
            var schema = new TileSchema
                {
                    YAxis = _axis,
                    Extent = new Extent(_minX, _minY, _maxX, _maxY),
                    Format = _format,
                    /*
                    Height = _height,
                    Width = _width,
                     */
                    Name = _schemaName,
                    OriginX = _originX,
                    OriginY = _originY,
                    Srs = _srs
                };

            foreach (var resolution in _resolutions)
            {
                schema.Resolutions.Add(resolution);
            }
            return schema;
        }


        public ITileSource TileSource { get; private set; }
        public ITileCache<byte[]> TileCache { get; private set; }
        
        public string LegendText { get; private set; }

        public IConfiguration Clone()
        {
            return new WmscLayerConfiguration(_fileCacheRoot);
        }
    }
}