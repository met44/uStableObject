using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace                                   uStableObject.Utilities
{
    public class                            PathfinderAStarBase<T> : IPathfinder<T> where T : IEquatable<T>
    {
        #region Members
        protected Dictionary<T, TileData>   _tilesData = new Dictionary<T, TileData>();
        protected List<T>                   _openedSorted = new List<T>();
        protected HashSet<T>                _opened = new HashSet<T>();
        protected HashSet<T>                _closed = new HashSet<T>();
        protected List<T>                   _path = new List<T>();
        protected List<T>                   _tileNeighbours = new List<T>();
        protected Action<T, List<T>>        _neighbours;
        protected Func<T, T, int>           _heuristic;
        protected T                         _to;
        protected int                       _ancestorsFactor;
        #endregion

        #region Properties
        public IReadOnlyDictionary<T, TileData> TilesData { get { return (this._tilesData); } }
        #endregion

        #region Triggers
        public void                         Init(Action<T, List<T>> neighbours, Func<T, T, int> heuristic, int ancestorsFactor)
        {
            this._neighbours = neighbours;
            this._heuristic = heuristic;
            this._ancestorsFactor = ancestorsFactor;
        }

        public IReadOnlyCollection<T>       GetPath(T from, T to)
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
                    UnityEngine.Profiling.Profiler.BeginSample("AStar.GetPath");
                    this.Clear();
                    this.Open(from, null);
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
                    UnityEngine.Profiling.Profiler.EndSample();
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
            return (this._path);
        }

        //Clears internal data including path, pools reuasable bits
        public void                         Clear()
        {
            UnityEngine.Profiling.Profiler.BeginSample("AStar.Clear");
            foreach (var tileData in this._tilesData.Values)
            {
                AutoPool<TileData>.Dispose(tileData);
            }
            this._tilesData.Clear();
            this._openedSorted.Clear();
            this._opened.Clear();
            this._closed.Clear();
            this._path.Clear();
            UnityEngine.Profiling.Profiler.EndSample();
        }

        //Clears everything out without pooling, letting garbage bits go to collector
        public void                         Flush()
        {
            this._tilesData.Clear();
            this._openedSorted.Clear();
            this._opened.Clear();
            this._closed.Clear();
            this._path.Clear();
        }
        #endregion

        #region Helpers
        bool                                CloseBest()
        {
            TileData                        tileData;

            UnityEngine.Profiling.Profiler.BeginSample("AStar.CloseBest");
            T tile = this._openedSorted[0];
            if (tile.Equals(this._to))
            {
                UnityEngine.Profiling.Profiler.EndSample();
                return (true);
            }
            this._tilesData.TryGetValue(tile, out tileData); //doing this here avoid the dict lookup for each neighbour
            this._openedSorted.RemoveAt(0);
            this._opened.Remove(tile);
            this._closed.Add(tile);
            this._tileNeighbours.Clear();
            this._neighbours(tile, this._tileNeighbours);
            foreach (var nTile in this._tileNeighbours)
            {
                if (!this._opened.Contains(nTile)
                    && !this._closed.Contains(nTile))
                {
                    this.Open(nTile, tileData);
                }
            }
            UnityEngine.Profiling.Profiler.EndSample();
            return (false);
        }

        void                                Open(T nTile, TileData prevTileData)
        {
            UnityEngine.Profiling.Profiler.BeginSample("AStar.Open");
            TileData data = this.InitTileData(nTile, prevTileData);

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
            UnityEngine.Profiling.Profiler.EndSample();
        }

        protected virtual TileData          InitTileData(T nTile, TileData prevTileData)
        {
            UnityEngine.Profiling.Profiler.BeginSample("AStar.InitTileData");
            TileData data = AutoPool<TileData>.Create();
            this._tilesData.Add(nTile, data);
            data._tile = nTile;
            data._prev = prevTileData != null ? prevTileData._tile : default(T); 
            data._ancestors = this.CalculateAncestors(prevTileData);

            //calling heuristic() must remain after everthing else so it is possible for it to poll for the tile ancestors in the heuristic calculation
            data._heuristic = this._heuristic(nTile, this._to) + data._ancestors;
            UnityEngine.Profiling.Profiler.EndSample();
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

        protected virtual int               CalculateAncestors(TileData prevTileData)
        {
            int                             prevTileAncestors = 0;
            
            if (prevTileData != null)
            {
                prevTileAncestors = prevTileData._ancestors + this._ancestorsFactor;
            }
            return (prevTileAncestors);
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
