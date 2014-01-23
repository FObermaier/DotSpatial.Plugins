using System;
using System.Diagnostics;
using System.Threading;
using Amib.Threading;
using BruTile;
using BruTile.Cache;

namespace DotSpatial.Plugins.BruTileLayer
{
    public class TileFetcher : IDisposable
    {
        internal class NoopCache : ITileCache<byte[]>
        {
            public static readonly NoopCache Instance = new NoopCache();

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

        private ITileProvider _provider;
        private MemoryCache<byte[]> _volatileCache;
        private ITileCache<byte[]> _permaCache;
        private SmartThreadPool _threadPool;

        private readonly System.Collections.Concurrent.ConcurrentDictionary<TileIndex, int> _activeTileRequests =
            new System.Collections.Concurrent.ConcurrentDictionary<TileIndex, int>();
        private readonly System.Collections.Concurrent.ConcurrentDictionary<TileIndex, int> _openTileRequests =
            new System.Collections.Concurrent.ConcurrentDictionary<TileIndex, int>();

        /// <summary>
        /// Creates an instance of this class
        /// </summary>
        /// <param name="provider">The tile provider</param>
        /// <param name="minTiles">min. number of tiles in memory cache</param>
        /// <param name="maxTiles">max. number of tiles in memory cache</param>
        /// <param name="permaCache">The perma cache</param>
        internal TileFetcher(ITileProvider provider, int minTiles, int maxTiles, ITileCache<byte[]> permaCache)
            : this(provider, minTiles, maxTiles, permaCache, BruTileLayerPlugin.Settings.MaximumNumberOfThreads)
        {
        }

        /// <summary>
        /// Creates an instance of this class
        /// </summary>
        /// <param name="provider">The tile provider</param>
        /// <param name="minTiles">min. number of tiles in memory cache</param>
        /// <param name="maxTiles">max. number of tiles in memory cache</param>
        /// <param name="permaCache">The perma cache</param>
        /// <param name="maxNumberOfThreads">The maximum number of threads used to get the tiles</param>
        internal TileFetcher(ITileProvider provider, int minTiles, int maxTiles, ITileCache<byte[]> permaCache,
            int maxNumberOfThreads)
        {
            _provider = provider;
            _volatileCache = new MemoryCache<byte[]>(minTiles, maxTiles);
            _permaCache = permaCache ?? NoopCache.Instance;
            _threadPool = new SmartThreadPool(10000, maxNumberOfThreads);
            AsyncMode = BruTileLayerPlugin.Settings.UseAsyncMode;
        }

        /// <summary>
        /// Method to get the tile
        /// </summary>
        /// <param name="tileInfo">The tile info</param>
        /// <param name="are">A manual reset event object</param>
        /// <returns>An array of bytes</returns>
        internal byte[] GetTile(TileInfo tileInfo, AutoResetEvent are)
        {
            var index = tileInfo.Index;
            var res = _volatileCache.Find(index);
            if (res != null)
                return res;

            res = _permaCache.Find(index);
            if (res != null)
            {
                _volatileCache.Add(index, res);
                return res;
            }

            if (!Contains(tileInfo.Index))
            {
                Add(tileInfo.Index);
                _threadPool.QueueWorkItem(GetTileOnThread, AsyncMode
                    ? new object[] {tileInfo}
                    : new object[] {tileInfo, are ?? new AutoResetEvent(false)});
            }
            return null;
        }

        /// <summary>
        /// Method to check if a tile has already been requested
        /// </summary>
        /// <param name="tileIndex">The tile index object</param>
        /// <returns><c>true</c> if the index object is already in the queue</returns>
        private bool Contains(TileIndex tileIndex)
        {
            var res = _activeTileRequests.ContainsKey(tileIndex) || _openTileRequests.ContainsKey(tileIndex);
            return res;
        }

        /// <summary>
        /// Method to add a tile to the active tile requests queue
        /// </summary>
        /// <param name="tileIndex">The tile index object</param>
        private void Add(TileIndex tileIndex)
        {
            if (!Contains(tileIndex))
            {
                Debug.WriteLine("Add: Adding TileIndex({0}, {1}, {2}) to active requests", tileIndex.Level,
                    tileIndex.Row, tileIndex.Col);
                _activeTileRequests.TryAdd(tileIndex, 1);
            }
            else
            {
                Debug.WriteLine("Add: Ignoring TileIndex({0}, {1}, {2}) because it has already been added",
                    tileIndex.Level, tileIndex.Row, tileIndex.Col);
            }
        }

        /// <summary>
        /// Method to actually get the tile from the <see cref="_provider"/>.
        /// </summary>
        /// <param name="parameter">The parameter, usually a <see cref="TileInfo"/> and a <see cref="AutoResetEvent"/></param>
        private void GetTileOnThread(object parameter)
        {
            var @params = (object[]) parameter;
            var tileInfo = (TileInfo) @params[0];

            byte[] result = null;

            //Try get the tile
            try
            {
                _openTileRequests.TryAdd(tileInfo.Index, 1); 
                result = _provider.GetTile(tileInfo);
            }
// ReSharper disable once EmptyGeneralCatchClause
            catch
            {
            }

            //Try at least once again
            if (result == null)
            {
                try
                {
                    result = _provider.GetTile(tileInfo);
                }
                catch
                {
                    if (!AsyncMode)
                    {
                        var are = (AutoResetEvent) @params[1];
                        are.Set();
                    }
                }
            }

            //Remove the tile info request
            int one;
            if (!_activeTileRequests.TryRemove(tileInfo.Index, out one))
            {
                //try again
                _activeTileRequests.TryRemove(tileInfo.Index, out one);
            }
            if (!_openTileRequests.TryRemove(tileInfo.Index, out one))
            {
                //try again
                _openTileRequests.TryRemove(tileInfo.Index, out one);
            }


            if (result != null)
            {
                //Add to the volatile cache
                _volatileCache.Add(tileInfo.Index, result);
                //Add to the perma cache
                _permaCache.Add(tileInfo.Index, result);

                if (AsyncMode)
                {
                    //Raise the event
                    OnTileReceived(new TileReceivedEventArgs(tileInfo, result));
                }
                else
                {
                    var are = (AutoResetEvent) @params[1];
                    are.Set();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the tile fetcher should work in async mode or not.
        /// </summary>
        public bool AsyncMode { get; set; }

        public bool Ready()
        {
            return (_activeTileRequests.Count == 0 && _openTileRequests.Count == 0);
        }

        /// <summary>
        /// Event raised when tile fetcher is in <see cref="AsyncMode"/> and a tile has been received.
        /// </summary>
        public event EventHandler<TileReceivedEventArgs> TileReceived;

        /// <summary>
        /// Event invoker for the <see cref="TileReceived"/> event
        /// </summary>
        /// <param name="tileReceivedEventArgs">The event arguments</param>
        private void OnTileReceived(TileReceivedEventArgs tileReceivedEventArgs)
        {
            // Don't raise events if we are not in async mode!
            if (!AsyncMode) return;

            if (TileReceived != null)
                TileReceived(this, tileReceivedEventArgs);

            var i = tileReceivedEventArgs.TileInfo.Index;
            System.Diagnostics.Debug.WriteLine("Tile received (Index({0}, {1}, {2})) {3} tiles loading", i.Level, i.Row, i.Col, _openTileRequests.Count);

            if (_activeTileRequests.Count == 0 && _openTileRequests.Count == 0)
                OnQueueEmpty(EventArgs.Empty);
        }

        /// <summary>
        /// Event raised when <see cref="AsyncMode"/> is <c>true</c> and the tile request queue is empty
        /// </summary>
        public event EventHandler QueueEmpty;

        /// <summary>
        /// Event invoker for the <see cref="TileReceived"/> event
        /// </summary>
        /// <param name="eventArgs">The event arguments</param>
        private void OnQueueEmpty(EventArgs eventArgs)
        {
            // Don't raise events if we are not in async mode!
            if (!AsyncMode) return;

            if (QueueEmpty != null)
                QueueEmpty(this, eventArgs);
        }

        void IDisposable.Dispose()
        {
            if (_volatileCache == null)
                return;

            _volatileCache.Clear();
            _volatileCache = null;
            _provider = null;
            _permaCache = null;

            _threadPool.Dispose();
            _threadPool = null;
        }

        /// <summary>
        /// Method to cancel the working queue, see http://dotspatial.codeplex.com/discussions/473428
        /// </summary>
        public void Clear()
        {
            _threadPool.Cancel(false);
            foreach (var request in _activeTileRequests.ToArray())
            {
                int one;
                if (!_openTileRequests.ContainsKey(request.Key)) 
                {
                    if (!_activeTileRequests.TryRemove(request.Key, out one))
                        _activeTileRequests.TryRemove(request.Key, out one);
                }
                _openTileRequests.Clear();
            }
        }
    }
}