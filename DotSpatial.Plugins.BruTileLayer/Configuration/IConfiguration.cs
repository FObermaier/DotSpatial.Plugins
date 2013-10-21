using System.ComponentModel.Composition;
using BruTile;

namespace DotSpatial.Plugins.BruTileLayer.Configuration
{
    /// <summary>
    /// Interface for all classes that can configure a <see cref="BruTileLayer"/>
    /// </summary>
    [InheritedExport]
    public interface IConfiguration
    {
        /// <summary>
        /// The legend text
        /// </summary>
        string LegendText { get; }

        /// <summary>
        /// Gets the <see cref="TileFetcher"/>
        /// </summary>
        ITileSource TileSource { get; }

        /// <summary>
        /// Gets the <see cref="TileFetcher"/>
        /// </summary>
        TileFetcher TileFetcher { get; }

        /// <summary>
        /// Gets a deep copy of the configuration
        /// </summary>
        /// <returns>The cloned configuration</returns>
        IConfiguration Clone();

        /// <summary>
        /// Method called prior to any tile access
        /// </summary>
        void Initialize();
    }
}