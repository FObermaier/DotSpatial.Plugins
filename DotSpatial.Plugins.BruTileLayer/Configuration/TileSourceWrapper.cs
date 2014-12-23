using System.Collections.Generic;

namespace DotSpatial.Plugins.BruTileLayer.Configuration
{
    public class TileSourceWrapper : BruTile.ITileSource
    {
        private readonly BruTile.ITileSource _tileSource;
        private readonly Dictionary<string, object> _properties;

        public TileSourceWrapper(BruTile.ITileSource tileSource)
        {
            _tileSource = tileSource;
            _properties = new Dictionary<string, object>();
        }

        public BruTile.ITileSchema Schema
        {
            get { return _tileSource.Schema; }
        }

        public string Name
        {
            get { return _tileSource.Name; }
        }

        public object this[string key]
        {
            get { return _properties[key]; }
            set
            {
                if (_properties.ContainsKey(key))
                    _properties[key] = value;
                else 
                    _properties.Add(key, value);
            }
        }

        public static IEnumerable<BruTile.ITileSource> Wrap(IEnumerable<BruTile.ITileSource> parse)
        {
            foreach (var tileSource in parse)
                yield return new TileSourceWrapper(tileSource);
        }

        public byte[] GetTile(BruTile.TileInfo tileInfo)
        {
            return _tileSource.GetTile(tileInfo);
        }
    }
}