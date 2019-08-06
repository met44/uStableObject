using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace                                   uStableObject.Utilities
{
    public class                            PathfinderAStarBase<T> : IPathfinder<T>
    {
        #region Members
        protected Dictionary<T, TileData>   _tilesData = new Dictionary<T, TileData>();
        protected List<T>                   _openedSorted = new List<T>();
        protected HashSet<T>                _opened = new HashSet<T>();
        protected HashSet<T>                _closed = new HashSet<T>();
        protected List<T>                   _path = new List<T>();
        protected Func<T, IEnumerable<T>>   _neighbours;
        protected Func<T, T, int>           _heuristic;
        protected T                         _to;
        #endregion

        #region Triggers
        public void                         Init(Func<T, IEnumerable<T>> neighbours, Func<T, T, int> heuristic)
        {
            this._neighbours = neighbours;
            this._heuristic = heuristic;
        }

        public IEnumerable<T>               GetPath(T from, T to)
        {
            this._to = to;
            if (this._neighbours == null || this._heuristic == null)
            {
                Debug.LogError("Missing heuristic or neighbours function for AStar pathfinder, must initialize first.");
            }
            else
            {
                try
                {
                    this._path.Clear();
                    this.Open(from, default(T));
                    if (!this.CloseBest())
                    {
                        while (this._openedSorted.Count > 0)
                        {
                            T bestTile = this._openedSorted[0];
                            if (this.CloseBest())
                            {
                                this.ProcessPath(from, bestTile);
                                break;
                            }
                        }
                    }
                    else
                    {
                        this._path.Insert(0, from);
                    }
                    foreach (var tileData in this._tilesData.Values)
                    {
                        AutoPool<TileData>.Dispose(tileData);
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
                this._tilesData.Clear();
                this._openedSorted.Clear();
                this._opened.Clear();
                this._closed.Clear();
            }
            return (this._path);
        }
        #endregion

        #region Helpers
        bool                                CloseBest()
        {
            T tile = this._openedSorted[0];
            if (object.Equals(tile, this._to))
            {
                return (true);
            }
            this._openedSorted.RemoveAt(0);
            this._opened.Remove(tile);
            this._closed.Add(tile);
            foreach (var nTile in this._neighbours(tile))
            {
                if (!this._opened.Contains(nTile)
                    && !this._closed.Contains(nTile))
                {
                    this.Open(nTile, tile);
                }
            }
            return (false);
        }

        void                                Open(T nTile, T prevTile)
        {
            TileData data = this.InitTileData(nTile, prevTile);

            //doing the bookkeeping of opened tiles sorted by heuristic will be faster
            int openedFrom = 0;
            int openedTo = this._opened.Count;
            while (openedTo - openedFrom > 0) //optimization
            {
                int iCenterTile = (openedFrom + openedTo) / 2;
                T centerTile = this._openedSorted[iCenterTile];
                int centerTileHeuristic = this._tilesData[centerTile]._heuristic;
                if (centerTileHeuristic == data._heuristic)
                {
                    openedFrom = iCenterTile;
                    break;
                }
                if (centerTileHeuristic < data._heuristic)
                {
                    openedFrom = iCenterTile + 1;
                }
                else
                {
                    openedTo = iCenterTile;
                }
            }
            int iTile = openedFrom;
            /*
            for (; iTile < openedTo; ++iTile)
            {
                T testTile = this._openedSorted[iTile];
                if (data._heuristic < this._tilesData[testTile]._heuristic)
                {
                    break;
                }
            }
            */
            this._openedSorted.Insert(iTile, nTile);
            this._opened.Add(nTile);
        }

        protected virtual TileData          InitTileData(T nTile, T prevTile)
        {
            TileData data = AutoPool<TileData>.Create();
            data._tile = nTile;
            data._prev = prevTile;
            data._ancestors = this.CalculateAncestors(prevTile);
            data._heuristic = this._heuristic(nTile, this._to) + data._ancestors;
            this._tilesData.Add(nTile, data);
            return (data);
        }

        protected virtual void              ProcessPath(T from, T bestTile)
        {
            while (!object.Equals(bestTile, from))
            {
                this._path.Insert(0, bestTile);
                bestTile = this._tilesData[bestTile]._prev;
            }
            this._path.Insert(0, from);
        }
        #endregion
        
        #region Helpers
        protected virtual int               CalculateAncestors(T prevTile)
        {
            TileData                        tileData;
            int                             prevTileAncestors = 0;
            
            if (this._tilesData.TryGetValue(prevTile, out tileData))
            {
                prevTileAncestors = tileData._ancestors;
            }
            return (prevTileAncestors + 1);
        }
        #endregion

        #region Data Types
        public class                        TileData
        {
            public T                        _tile;
            public T                        _prev;
            public int                      _heuristic;
            public int                      _ancestors;
        }
        #endregion
    }
}
