using System;
using BruTile;

namespace DotSpatial.Plugins.BruTileLayer
{
    /// <summary>
    /// Event arguments for the <see cref="TileFetcher.TileReceived"/> event
    /// </summary>
    public class TileReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the tile information object
        /// </summary>
        public TileInfo TileInfo { get; private set; }
        
        /// <summary>
        /// Gets the actual tile data as a byte Array
        /// </summary>
        public byte[] Tile { get; private set; }

        /// <summary>
        /// Creates an instance of this class
        /// </summary>
        /// <param name="tileInfo">The tile info object</param>
        /// <param name="tile">The tile data</param>
        internal TileReceivedEventArgs(TileInfo tileInfo, byte[] tile)
        {
            TileInfo = tileInfo;
            Tile = tile;
        }
    }
}