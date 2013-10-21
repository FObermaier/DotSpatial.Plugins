namespace DotSpatial.Plugins.BruTileLayer.Configuration
{
    /// <summary>
    /// Possible perma cache types
    /// </summary>
    public enum PermaCacheType
    {
        /// <summary>
        /// The default perma cache
        /// </summary>
        Default = 0,

        /// <summary>
        /// No operation cache
        /// </summary>
        Noop = 2,

        /// <summary>
        /// File cache is currently the default
        /// </summary>
        FileCache = Default,

        /// <summary>
        /// SQLite Db cache
        /// </summary>
        DbCache = 1
    }
}