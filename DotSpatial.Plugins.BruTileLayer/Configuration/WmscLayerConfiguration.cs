using System;
using System.Collections.Generic;
using System.Reflection;
using BruTile;
using BruTile.Cache;
using BruTile.Web;
using DotSpatial.Serialization;

namespace DotSpatial.Plugins.BruTileLayer.Configuration
{
    public class WmscLayerConfiguration : PermaCacheConfiguration, IConfiguration
    {
        [Serialize("fileCacheRoot", ConstructorArgumentIndex = 0)]
        private string _fileCacheRoot;

        [Serialize("url")]
        private string _url;
        //
        //needed for schema generation
        [Serialize("axis")]
        private AxisDirection _axis;
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
        private Dictionary<string, double> _resolutions;
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
            LegendText = name;

            var provider = source.Provider as WebTileProvider;
            if (provider == null)
                throw new ArgumentException("Source does not have a WebTileProvider", "source");

            var request = ReflectRequest(provider);
            if (request == null)
                throw new ArgumentException("Source does not have a WmscRequest" ,"source");

            SafeRequest(request);
            SafeSchema(source.Schema);

            TileSource = source;
            _tileFetcher = new TileFetcher(provider, BruTileLayerPlugin.Settings.MemoryCacheMinimum,
                                           BruTileLayerPlugin.Settings.MemoryCacheMaximum, TileCache);
            _initialized = true;
        }

        internal static WmscRequest ReflectRequest(WebTileProvider provider)
        {
            var fi = typeof(WebTileProvider).GetField("_request", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fi == null)
                throw new ArgumentException("Provider does not have a private field '_request'", "provider");

            return (WmscRequest)fi.GetValue(provider);
        }

        private void SafeRequest(WmscRequest request)
        {
            _url = ReflectBaseUri(request).ToString();
            _layers = new List<string>(ReflectListItems(request, "_layers"));
            _styles = new List<string>(ReflectListItems(request, "_styles"));
            _customParameters = new Dictionary<string, string>(ReflectDictionary(request, "_customParameters"));
            _version = request.Version;
        }

        /// <summary>
        /// Gets the <see cref="IConfiguration.TileFetcher"/>
        /// </summary>
        public TileFetcher TileFetcher { get { return _tileFetcher; } }

        public void Initialize()
        {
            if (_initialized) return;
            _initialized = true;

            var schema = RestoreSchema();
            var request = new WmscRequest(new Uri(_url), schema, _layers, _styles, _customParameters, _version);
            
            ITileProvider provider = new WebTileProvider(request);
            TileSource = (WmscTileSource)Activator.CreateInstance(typeof(WmscTileSource), BindingFlags.NonPublic, schema, provider);

            _tileFetcher = new TileFetcher(TileSource.Provider,
                                           BruTileLayerPlugin.Settings.MemoryCacheMinimum,
                                           BruTileLayerPlugin.Settings.MemoryCacheMaximum,
                                           TileCache);

        }

        private void SafeSchema(ITileSchema schema)
        {
            _axis = schema.Axis;
            _minX = schema.Extent.MinX;
            _maxX = schema.Extent.MaxX;
            _minY = schema.Extent.MinY;
            _maxY = schema.Extent.MaxY;
            _format = schema.Format;
            _height = schema.Height;
            _width = schema.Width;
            _schemaName = schema.Name;
            _originX = schema.OriginX;
            _originY = schema.OriginY;
            _srs = schema.Srs;
            _resolutions = new Dictionary<string, double>();
            foreach (var resolution in schema.Resolutions)
                _resolutions.Add(resolution.Id, resolution.UnitsPerPixel);
        }

        private ITileSchema RestoreSchema()
        {
            var schema = new TileSchema
                {
                    Axis = _axis,
                    Extent = new Extent(_minX, _minY, _maxX, _maxY),
                    Format = _format,
                    Height = _height,
                    Width = _width,
                    Name = _schemaName,
                    OriginX = _originX,
                    OriginY = _originY,
                    Srs = _srs
                };
            foreach (var resolution in _resolutions)
            {
                var res = new Resolution {Id = resolution.Key, UnitsPerPixel = resolution.Value};
                schema.Resolutions.Add(res);
            }
            return schema;
        }

        internal static Uri ReflectBaseUri(WmscRequest request)
        {
            var fi = typeof (WmscRequest).GetField("_baseUrl", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fi == null) 
                throw new ArgumentException("Request does not have a private field '_baseUri'", "request");

            return (Uri) fi.GetValue(request);
        }

        private static IEnumerable<string> ReflectListItems(WmscRequest request, string field)
        {
            var fi = typeof(WmscRequest).GetField(field, BindingFlags.Instance | BindingFlags.NonPublic);
            if (fi == null)
                throw new ArgumentException("Request does not have a private field '" + field + "'", "request");

            return (IEnumerable<string>) fi.GetValue(request);
        }

        private static IDictionary<string, string> ReflectDictionary(WmscRequest request, string field)
        {
            var fi = typeof(WmscRequest).GetField(field, BindingFlags.Instance | BindingFlags.NonPublic);
            if (fi == null)
                throw new ArgumentException("Request does not have a private field '" + field + "'", "request");

            return (IDictionary<string, string>)fi.GetValue(request);
        }

        private static T Reflect<T>(WmscRequest request, string field)
        {
            var fi = typeof(WmscRequest).GetField(field, BindingFlags.Instance | BindingFlags.NonPublic);
            if (fi == null)
                throw new ArgumentException("Request does not have a private field '" + field + "'", "request");

            return (T)fi.GetValue(request);
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