using System;
using System.IO;
using BruTile.Cache;
using DotSpatial.Controls;
using DotSpatial.Plugins.BruTileLayer.Configuration;

namespace DotSpatial.Plugins.BruTileLayer
{
    internal class BruTileLayerSettings
    {
        private int _maximumNumberOfThreads;

        internal BruTileLayerSettings()
        {
            PermaCacheRoot = Path.GetTempPath();
            PermaCacheExpireInDays = 14;
            PermaCacheFormat = "png";

            MemoryCacheMinimum = 100;
            MemoryCacheMaximum = 200;
            
            BingMapsToken = string.Empty;
            UseAsyncMode = false;

            MaximumNumberOfThreads = 4;
        }

        /// <summary>
        /// Gets the type of the perma cache
        /// </summary>
        public PermaCacheType PermaCacheType { get; set; }
        
        /// <summary>
        /// Gets the tile cache root
        /// </summary>
        public string PermaCacheRoot { get; internal set; }

        /// <summary>
        /// Gets the <see cref="MemoryCache{T}._minTiles"/> value
        /// </summary>
        public int MemoryCacheMinimum { get; internal set; }

        /// <summary>
        /// Gets the <see cref="MemoryCache{T}._maxTiles"/> value
        /// </summary>
        public int MemoryCacheMaximum { get; internal set; }

        /// <summary>
        /// Gets the Bing Maps token
        /// </summary>
        public string BingMapsToken { get; set; }

        /// <summary>
        /// Gets a value indicating if <see cref="TileFetcher"/> is to be run in async mode
        /// </summary>
        public bool UseAsyncMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating how long tiles remain valid
        /// </summary>
        public int PermaCacheExpireInDays { get; set; }

        /// <summary>
        /// Gets or sets a value indicating which format should be used to store the tiles
        /// </summary>
        public string PermaCacheFormat { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the maximum number of threads used for <see cref="TileFetcher"/>
        /// </summary>
        public int MaximumNumberOfThreads
        {
            get { return _maximumNumberOfThreads; }
            set
            {
                if (value < 1) value = 1;
                if (value > 25) value = 25;
                _maximumNumberOfThreads = value;
            }
        }

        public void Serialize(SerializationManager manager)
        {
            manager.SetCustomSetting("UseAsyncMode", UseAsyncMode);

            manager.SetCustomSetting("PermaCacheType", (int)PermaCacheType);
            manager.SetCustomSetting("PermaCacheRoot", PermaCacheRoot);
            manager.SetCustomSetting("PermaCacheExpire", PermaCacheExpireInDays);

            manager.SetCustomSetting("MemoryCacheMinimum", MemoryCacheMinimum);
            manager.SetCustomSetting("MemoryCacheMaximum", MemoryCacheMaximum);

            manager.SetCustomSetting("BingMapsToken", BingMapsToken);

            manager.SetCustomSetting("MaximumNumberOfThreads", MaximumNumberOfThreads);
        }

        public void Deserialize(SerializationManager manager)
        {
            UseAsyncMode = manager.GetCustomSetting("UseAsyncMode", false);

            PermaCacheType = (PermaCacheType)manager.GetCustomSetting("PermaCacheType", 0);
            PermaCacheRoot = manager.GetCustomSetting("PermaCacheRoot", Path.GetTempPath());
            PermaCacheExpireInDays = manager.GetCustomSetting("PermaCacheExpire", 14);

            MemoryCacheMinimum = manager.GetCustomSetting("MemoryCacheMinimum", 100);
            MemoryCacheMaximum = manager.GetCustomSetting("MemoryCacheMaximum", 200);

            BingMapsToken = manager.GetCustomSetting("BingMapsToken", string.Empty);

            MaximumNumberOfThreads = manager.GetCustomSetting("MaximumNumberOfThreads", 4);
        }
    }
}