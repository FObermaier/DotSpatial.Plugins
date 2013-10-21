using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
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

        //private readonly ReaderWriterLockSlim _readerWriterLockSlim = 
        //    new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        //private readonly HashSet<TileInfo> _activeTileRequests = new HashSet<TileInfo>();

        private readonly System.Collections.Concurrent.ConcurrentDictionary<TileInfo, int> _activeTileRequests =
            new System.Collections.Concurrent.ConcurrentDictionary<TileInfo, int>();

        internal TileFetcher(ITileProvider provider, int minTiles, int maxTiles,  ITileCache<byte[]> permaCache)
        {
            _provider = provider;
            _volatileCache = new MemoryCache<byte[]>(minTiles, maxTiles);
            _permaCache = permaCache ?? NoopCache.Instance;
            AsyncMode = BruTileLayerPlugin.Settings.UseAsyncMode;
        }

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

            if (!Contains(tileInfo))
            {
                Add(tileInfo);
                ThreadPool.QueueUserWorkItem(GetTileOnThread, AsyncMode 
                                                ? new object[] {tileInfo} 
                                                : new object[] {tileInfo, are ?? new AutoResetEvent(false)});
            }
            return null;
        }

        private bool Contains(TileInfo tileInfo)
        {
            //_readerWriterLockSlim.EnterReadLock();
            var res = _activeTileRequests.ContainsKey(tileInfo);
            //_readerWriterLockSlim.ExitReadLock();
            return res;
        }

        private void Add(TileInfo tileInfo)
        {
            if (!_activeTileRequests.ContainsKey(tileInfo))
            {
                //_readerWriterLockSlim.EnterWriteLock();
                Debug.WriteLine("Add: Adding TileInfo(Index({0}, {1}, {2})) to active requests", tileInfo.Index.Level, tileInfo.Index.Row, tileInfo.Index.Col);
                _activeTileRequests.TryAdd(tileInfo, 1);
                //_readerWriterLockSlim.ExitWriteLock();
            }
            else
                Debug.WriteLine("Add: Ignoring TileInfo(Index({0}, {1}, {2})) because it has already been added", tileInfo.Index.Level, tileInfo.Index.Row, tileInfo.Index.Col);
        }

        private static void Remove(ReaderWriterLockSlim rwlock, HashSet<TileInfo> tileInfos, TileInfo info)
        {
            rwlock.EnterWriteLock();
            tileInfos.Remove(info);
            rwlock.ExitWriteLock();
        }

        private void GetTileOnThread(object parameter)
        {
            var @params = (object[]) parameter;
            var tileInfo = (TileInfo) @params[0];

            byte[] result = null;

            //Try get the tile
            try
            {
                result = _provider.GetTile(tileInfo);
            }
            catch {}
            
            //Try at least once again
            if (result == null)
            {
                try
                {
                    result = _provider.GetTile(tileInfo);
                }
                catch
                {
                    var are = (AutoResetEvent)@params[1];
                    are.Set();
                }
            }

            //Remove the tile info request
            int one;
            if (!_activeTileRequests.TryRemove(tileInfo, out one))
            {
                //try again
                _activeTileRequests.TryRemove(tileInfo, out one);
            }

            //Remove(_readerWriterLockSlim, _activeTileRequests, tileInfo);
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
            // Don't raise events if we are in async mode!
            if (!AsyncMode) return;

            if (TileReceived != null)
                TileReceived(this, tileReceivedEventArgs);

            //_readerWriterLockSlim.EnterReadLock();
            if (_activeTileRequests.Count == 0)
                OnQueueEmpty(EventArgs.Empty);
            //_readerWriterLockSlim.ExitReadLock();
        }

        public event EventHandler QueueEmpty;
        
        /// <summary>
        /// Event invoker for the <see cref="TileReceived"/> event
        /// </summary>
        /// <param name="eventArgs">The event arguments</param>
        private void OnQueueEmpty(EventArgs eventArgs)
        {
            // Don't raise events if we are in async mode!
            if (!AsyncMode) return;

            if (QueueEmpty != null)
                QueueEmpty(this, eventArgs);
        }

        public void Dispose()
        {
            if (_volatileCache == null)
                return;
            _volatileCache.Clear();
            _volatileCache = null;
            _provider = null;
            _permaCache = null;
        }
    }
}