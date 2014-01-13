using System;

namespace DotSpatial.Plugins.BruTileLayer.Configuration.Forms
{
    /// <summary>
    /// Interface for all BruTile layer configuration editors
    /// </summary>
    public interface IConfigurationEditor
    {
        /// <summary>
        /// Gets the name of the BruTile provider to configure
        /// </summary>
        String BruTileName { get; }

        /// <summary>
        /// Method th create the configuration
        /// </summary>
        /// <returns>The configuration</returns>
        IConfiguration Create();

        /// <summary>
        /// Saves the settings or defined tile services
        /// </summary>
        void SaveSettings();
    }
}