using System;
using System.Configuration;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using BruTile;
using BruTile.Cache;
using DotSpatial.Serialization;

namespace DotSpatial.Plugins.BruTileLayer.Configuration
{
    [Serializable]
    public abstract class PermaCacheConfiguration
    {
        static PermaCacheConfiguration()
        {
            try
            {
                var asr = new AppSettingsReader();

                Format = (string)asr.GetValue("BruTileFileCacheFormat", typeof(string));
                if (string.IsNullOrEmpty(Format)) Format = "png";
                
                var expire = (string) asr.GetValue("BruTileFileCacheExpire", typeof (string));
                if (string.IsNullOrEmpty(expire))
                    Expire = TimeSpan.FromDays(14);
                else
                    Expire = TimeSpan.Parse(expire);

                var pct = (string)asr.GetValue("BruTilePermaCacheType", typeof (string));
                if (string.IsNullOrEmpty(pct))
                    pct = PermaCacheType.FileCache.ToString();
                PermaCacheType = (PermaCacheType) Enum.Parse(typeof(PermaCacheType), pct);
            }

            catch (Exception)
            {
                Format = "png";
                Expire = TimeSpan.FromDays(14);
                PermaCacheType = PermaCacheType.FileCache;
            }

        }

        [Serialize("permaCacheType", ConstructorArgumentIndex = 0)]
        private readonly PermaCacheType _permaCacheType;

        [Serialize("permaCacheRoot", ConstructorArgumentIndex = 1)]
        private readonly string _permaCacheRoot;

        protected PermaCacheConfiguration(PermaCacheType permaCacheType, string permaCacheRoot)
        {
            _permaCacheType = permaCacheType;
            _permaCacheRoot = permaCacheRoot;
        }

        public string PermaCacheRoot
        {
            get { return _permaCacheRoot; }
        }

        /// <summary>
        /// Gets the image format the tiles are cached in
        /// </summary>
        public static string Format { get; private set; }

        /// <summary>
        /// Gets the timespan the tiles in the file cache are valid
        /// </summary>
        public static TimeSpan Expire { get; protected set; }

        /// <summary>
        /// Method to create the tile cache
        /// </summary>
        /// <returns></returns>
        public IPersistentCache<byte[]> CreateTileCache()
        {
            var settings = BruTileLayerPlugin.Settings;
            switch (PermaCacheType)
            {
                case PermaCacheType.Noop:
                    return new NoopCache();
                case PermaCacheType.FileCache:
                    return new FileCache(PermaCacheRoot, settings.PermaCacheFormat ?? "png", 
                        TimeSpan.FromDays(settings.PermaCacheExpireInDays));
                case PermaCacheType.DbCache:
                    return CreateDbCache(PermaCacheRoot);
            }
            return new FileCache(PermaCacheRoot, Format, Expire);
        }

        private static DbCache<SQLiteConnection> CreateDbCache(string path)
        {
            var conn = new SQLiteConnection(string.Format("Data Source={0};", path));
            return new DbCache<SQLiteConnection>(conn);
        }

        public static PermaCacheType PermaCacheType { get; set; }


        protected static string GetTempPath(string suffix)
        {
            return Path.Combine(Path.GetTempPath(), suffix);
        }

        private class NoopCache : IPersistentCache<byte[]>
        {
            public void Add(TileIndex index, byte[] image)
            {
            }

            public void Remove(TileIndex index)
            {
            }

            public byte[] Find(TileIndex index)
            {
                return null;
            }
        }
    }

}