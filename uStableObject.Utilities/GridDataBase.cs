using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using uStableObject;

namespace                                   uStableObject.Utilities
{
    public class                            GridDataBase : ScriptableObject
    {
        #region Input Data
        [SerializeField] Vector2Int         _gridSize;
        #endregion

        #region Properties
        public int                          TilesCount { get { return (this._tiles.Count); } }
        public Vector2Int                   GridSize { get; private set; }
        #endregion

        #region Members
        Dictionary<Vector2Int, TileData>    _tiles = new Dictionary<Vector2Int, TileData>();
        #endregion

        #region Unity
        void                                OnEnable()
        {
            this.GridSize = this._gridSize;
            if (this._tiles == null)
            {
                this._tiles = new Dictionary<Vector2Int, TileData>();
            }
            this._tiles.Clear();
        }
        #endregion

        #region Triggers
        public void                         ClearAll()
        {
            foreach (var tileData in this._tiles.Values)
            {
                tileData.Clear(true);
                AutoPool<TileData>.Dispose(tileData);
            }
            this._tiles.Clear();
        }

        public void                         SetGridSize(Vector2Int gridSize)
        {
            this.GridSize = gridSize;
        }

        public bool                         Clear(Vector2Int tile, bool shallowCleanup = false)
        {
            TileData                        tileData;

            if (this._tiles.TryGetValue(tile, out tileData))
            {
                this._tiles.Remove(tile);
                tileData.Clear(shallowCleanup);
                AutoPool<TileData>.Dispose(tileData);
                return (true);
            }
            return (false);
        }
        
        void                                ClearShallow(Vector2Int tile) { this.Clear(tile, true); }
        void                                ClearNotShallow(Vector2Int tile) { this.Clear(tile, false); }
        public void                         Clear(Vector2Int tileFrom, Vector2Int tileTo, bool shallowCleanup = false)
        {
            if (shallowCleanup)
            {
                ForEach(tileFrom, tileTo, this.ClearShallow);
            }
            else
            {
                ForEach(tileFrom, tileTo, this.ClearNotShallow);
            }
        }

        public void                         SetTileData<V>(Vector2Int tile,  IDataIdentifier dataIdentifier, V data)
        {
            TileData                        tileData;

            if (!this._tiles.TryGetValue(tile, out tileData))
            {
                if (!Equals(default(V), data))
                {
                    tileData = AutoPool<TileData>.Create();
                    this._tiles.Add(tile, tileData);
                }
                else
                {
                    return;
                }
            }
            tileData.SetValue(dataIdentifier, data);
        }

        public void                         SetTileData<V>(Vector2Int tileFrom, Vector2Int tileTo,  IDataIdentifier dataIdentifier, V data)
        {
            ForEach(tileFrom, tileTo, this.SetTileData, dataIdentifier, data);
        }

        public void                         UnsetTileData<V>(Vector2Int tile,  IDataIdentifier dataIdentifier)
        {
            TileData                        tileData;

            if (this._tiles.TryGetValue(tile, out tileData))
            {
                if (tileData.UnsetValue<V>(dataIdentifier))
                {
                    if (!tileData.HasValues())
                    {
                        this._tiles.Remove(tile);
                        tileData.Clear(true);
                        AutoPool<TileData>.Dispose(tileData);
                    }
                }
            }
        }

        public void                         UnsetTileData<V>(Vector2Int tileFrom, Vector2Int tileTo, IDataIdentifier dataIdentifier)
        {
            ForEach(tileFrom, tileTo, this.UnsetTileData<V>, dataIdentifier);
        }

        public bool                         GetTileData<V>(Vector2Int tile,  IDataIdentifier dataIdentifier, out V data)
        {
            TileData                        tileData;

            if (this._tiles.TryGetValue(tile, out tileData))
            {
                if (tileData.TryGetValue(dataIdentifier, out data))
                {
                    return (data != null);
                }
            }
            else
            {
                data = default(V);
            }
            return (false);
        }

        public IEnumerable<V>               GetAllTilesData<V>(IDataIdentifier dataIdentifier)
        {
            V                               data;

            foreach (var tileData in this._tiles.Values)
            {
                if (tileData.TryGetValue(dataIdentifier, out data))
                {
                    yield return (data);
                }
            }
        }

        public IEnumerable<V>               GetMatchingTilesData<V>(Vector2Int tileFrom, Vector2Int tileTo, IDataIdentifier dataIdentifier)
        {
            V                               data;
            Vector2Int                      gridPos = new Vector2Int();
            int                             xMin = Mathf.Min(tileFrom.x, tileTo.x);
            int                             xMax = Mathf.Max(tileFrom.x, tileTo.x);
            int                             yMin = Mathf.Min(tileFrom.y, tileTo.y);
            int                             yMax = Mathf.Max(tileFrom.y, tileTo.y);

            for (int x = xMin; x <= xMax; ++x)
            {
                for (int y = yMin; y <= yMax; ++y)
                {
                    gridPos.Set(x, y);
                    if (GetTileData(gridPos, dataIdentifier, out data))
                    {
                        yield return (data);
                    }
                }
            }
        }

        public bool                         HasTileData(Vector2Int tile)
        {
            return (this._tiles.ContainsKey(tile));
        }

        public bool                         HasTileData(Vector2Int tile, IDataIdentifier dataIdentifier)
        {
            TileData                        tileData;

            if (this._tiles.TryGetValue(tile, out tileData))
            {
                return (tileData.HasValue(dataIdentifier));
            }
            return (false);
        }

        public bool                         HasTileData<V>(Vector2Int tile, IDataIdentifier dataIdentifier, V search)
        {
            V                               data;
            TileData                        tileData;

            if (this._tiles.TryGetValue(tile, out tileData))
            {
                if (tileData.TryGetValue(dataIdentifier, out data))
                {
                    return (Object.Equals(data, search));
                }
            }
            else
            {
                data = default(V);
            }
            return (false);
        }

        public bool                         HasTileData(Vector2Int tileFrom, Vector2Int tileTo, Match match = Match.Any)
        {
            if (match == Match.All)
            {
                return (All(tileFrom, tileTo, this.HasTileData));
            }
            else
            {
                return (Any(tileFrom, tileTo, this.HasTileData));
            }
        }

        public bool                         HasTileData<V>(Vector2Int tileFrom, Vector2Int tileTo, IDataIdentifier dataIdentifier, Match match = Match.Any)
        {
            if (match == Match.All)
            {
                return (All(tileFrom, tileTo, this.HasTileData, dataIdentifier));
            }
            else
            {
                return (Any(tileFrom, tileTo, this.HasTileData, dataIdentifier));
            }
        }

        public bool                         HasTileData<V>(Vector2Int tileFrom, Vector2Int tileTo, IDataIdentifier dataIdentifier, V searchType, Match match = Match.Any)
        {
            if (match == Match.All)
            {
                return (All(tileFrom, tileTo, this.HasTileData, dataIdentifier, searchType));
            }
            else
            {
                return (Any(tileFrom, tileTo, this.HasTileData, dataIdentifier, searchType));
            }
        }

        public bool                         Contains(Vector2Int tile)
        {
            return (tile.x > -this.GridSize.x / 2 && tile.x < this.GridSize.x / 2
                    && tile.y > -this.GridSize.y / 2 && tile.y < this.GridSize.y / 2);
        }

        public bool                         Contains(Vector2Int tileFrom, Vector2Int tileTo)
        {
            return (this.Contains(tileFrom) && this.Contains(tileTo));
        }

        public void                         Clear()
        {
            this._tiles.Clear();
        }
        #endregion

        #region Statics
        public static void                  ForEach(Vector2Int tileFrom, Vector2Int tileTo, System.Action<Vector2Int> action)
        {
            Vector2Int                      gridPos = new Vector2Int();
            int                             xMin = Mathf.Min(tileFrom.x, tileTo.x);
            int                             xMax = Mathf.Max(tileFrom.x, tileTo.x);
            int                             yMin = Mathf.Min(tileFrom.y, tileTo.y);
            int                             yMax = Mathf.Max(tileFrom.y, tileTo.y);

            for (int x = xMin; x <= xMax; ++x)
            {
                for (int y = yMin; y <= yMax; ++y)
                {
                    gridPos.Set(x, y);
                    action(gridPos);
                }
            }
        }
        
        public static void                  ForEach<P>(Vector2Int tileFrom, Vector2Int tileTo, System.Action<Vector2Int, P> action, P param)
        {
            Vector2Int                      gridPos = new Vector2Int();
            int                             xMin = Mathf.Min(tileFrom.x, tileTo.x);
            int                             xMax = Mathf.Max(tileFrom.x, tileTo.x);
            int                             yMin = Mathf.Min(tileFrom.y, tileTo.y);
            int                             yMax = Mathf.Max(tileFrom.y, tileTo.y);

            for (int x = xMin; x <= xMax; ++x)
            {
                for (int y = yMin; y <= yMax; ++y)
                {
                    gridPos.Set(x, y);
                    action(gridPos, param);
                }
            }
        }
        
        public static void                  ForEach<P>(Vector2Int tileFrom, Vector2Int tileTo, System.Action<Vector2Int, IDataIdentifier, P> action, IDataIdentifier dataIdentifier, P param)
        {
            Vector2Int                      gridPos = new Vector2Int();
            int                             xMin = Mathf.Min(tileFrom.x, tileTo.x);
            int                             xMax = Mathf.Max(tileFrom.x, tileTo.x);
            int                             yMin = Mathf.Min(tileFrom.y, tileTo.y);
            int                             yMax = Mathf.Max(tileFrom.y, tileTo.y);

            for (int x = xMin; x <= xMax; ++x)
            {
                for (int y = yMin; y <= yMax; ++y)
                {
                    gridPos.Set(x, y);
                    action(gridPos, dataIdentifier, param);
                }
            }
        }

        public static bool                  Any(Vector2Int tileFrom, Vector2Int tileTo, System.Func<Vector2Int, bool> action)
        {
            Vector2Int                      gridPos = new Vector2Int();
            int                             xMin = Mathf.Min(tileFrom.x, tileTo.x);
            int                             xMax = Mathf.Max(tileFrom.x, tileTo.x);
            int                             yMin = Mathf.Min(tileFrom.y, tileTo.y);
            int                             yMax = Mathf.Max(tileFrom.y, tileTo.y);

            for (int x = xMin; x <= xMax; ++x)
            {
                for (int y = yMin; y <= yMax; ++y)
                {
                    gridPos.Set(x, y);
                    if (action(gridPos))
                    {
                        return (true);
                    }
                }
            }
            return (false);
        }

        public static bool                  Any<P>(Vector2Int tileFrom, Vector2Int tileTo, System.Func<Vector2Int, P, bool> action, P param)
        {
            Vector2Int                      gridPos = new Vector2Int();
            int                             xMin = Mathf.Min(tileFrom.x, tileTo.x);
            int                             xMax = Mathf.Max(tileFrom.x, tileTo.x);
            int                             yMin = Mathf.Min(tileFrom.y, tileTo.y);
            int                             yMax = Mathf.Max(tileFrom.y, tileTo.y);

            for (int x = xMin; x <= xMax; ++x)
            {
                for (int y = yMin; y <= yMax; ++y)
                {
                    gridPos.Set(x, y);
                    if (action(gridPos, param))
                    {
                        return (true);
                    }
                }
            }
            return (false);
        }

        public static bool                  Any(Vector2Int tileFrom, Vector2Int tileTo, System.Func<Vector2Int, IDataIdentifier, bool> action,  IDataIdentifier dataIdentifier)
        {
            Vector2Int                      gridPos = new Vector2Int();
            int                             xMin = Mathf.Min(tileFrom.x, tileTo.x);
            int                             xMax = Mathf.Max(tileFrom.x, tileTo.x);
            int                             yMin = Mathf.Min(tileFrom.y, tileTo.y);
            int                             yMax = Mathf.Max(tileFrom.y, tileTo.y);

            for (int x = xMin; x <= xMax; ++x)
            {
                for (int y = yMin; y <= yMax; ++y)
                {
                    gridPos.Set(x, y);
                    if (action(gridPos, dataIdentifier))
                    {
                        return (true);
                    }
                }
            }
            return (false);
        }

        public static bool                  Any<P>(Vector2Int tileFrom, Vector2Int tileTo, System.Func<Vector2Int, IDataIdentifier, P, bool> action,  IDataIdentifier dataIdentifier, P param)
        {
            Vector2Int                      gridPos = new Vector2Int();
            int                             xMin = Mathf.Min(tileFrom.x, tileTo.x);
            int                             xMax = Mathf.Max(tileFrom.x, tileTo.x);
            int                             yMin = Mathf.Min(tileFrom.y, tileTo.y);
            int                             yMax = Mathf.Max(tileFrom.y, tileTo.y);

            for (int x = xMin; x <= xMax; ++x)
            {
                for (int y = yMin; y <= yMax; ++y)
                {
                    gridPos.Set(x, y);
                    if (action(gridPos, dataIdentifier, param))
                    {
                        return (true);
                    }
                }
            }
            return (false);
        }

        public static bool                  All(Vector2Int tileFrom, Vector2Int tileTo, System.Func<Vector2Int, bool> action)
        {
            Vector2Int                      gridPos = new Vector2Int();
            int                             xMin = Mathf.Min(tileFrom.x, tileTo.x);
            int                             xMax = Mathf.Max(tileFrom.x, tileTo.x);
            int                             yMin = Mathf.Min(tileFrom.y, tileTo.y);
            int                             yMax = Mathf.Max(tileFrom.y, tileTo.y);

            for (int x = xMin; x <= xMax; ++x)
            {
                for (int y = yMin; y <= yMax; ++y)
                {
                    gridPos.Set(x, y);
                    if (!action(gridPos))
                    {
                        return (false);
                    }
                }
            }
            return (true);
        }

        public static bool                  All<P>(Vector2Int tileFrom, Vector2Int tileTo, System.Func<Vector2Int, P, bool> action, P param)
        {
            Vector2Int                      gridPos = new Vector2Int();
            int                             xMin = Mathf.Min(tileFrom.x, tileTo.x);
            int                             xMax = Mathf.Max(tileFrom.x, tileTo.x);
            int                             yMin = Mathf.Min(tileFrom.y, tileTo.y);
            int                             yMax = Mathf.Max(tileFrom.y, tileTo.y);

            for (int x = xMin; x <= xMax; ++x)
            {
                for (int y = yMin; y <= yMax; ++y)
                {
                    gridPos.Set(x, y);
                    if (!action(gridPos, param))
                    {
                        return (false);
                    }
                }
            }
            return (true);
        }

        public static bool                  All(Vector2Int tileFrom, Vector2Int tileTo, System.Func<Vector2Int, IDataIdentifier, bool> action,  IDataIdentifier dataIdentifier)
        {
            Vector2Int                      gridPos = new Vector2Int();
            int                             xMin = Mathf.Min(tileFrom.x, tileTo.x);
            int                             xMax = Mathf.Max(tileFrom.x, tileTo.x);
            int                             yMin = Mathf.Min(tileFrom.y, tileTo.y);
            int                             yMax = Mathf.Max(tileFrom.y, tileTo.y);

            for (int x = xMin; x <= xMax; ++x)
            {
                for (int y = yMin; y <= yMax; ++y)
                {
                    gridPos.Set(x, y);
                    if (!action(gridPos, dataIdentifier))
                    {
                        return (false);
                    }
                }
            }
            return (true);
        }

        public static bool                  All<P>(Vector2Int tileFrom, Vector2Int tileTo, System.Func<Vector2Int, IDataIdentifier, P, bool> action,  IDataIdentifier dataIdentifier, P param)
        {
            Vector2Int                      gridPos = new Vector2Int();
            int                             xMin = Mathf.Min(tileFrom.x, tileTo.x);
            int                             xMax = Mathf.Max(tileFrom.x, tileTo.x);
            int                             yMin = Mathf.Min(tileFrom.y, tileTo.y);
            int                             yMax = Mathf.Max(tileFrom.y, tileTo.y);

            for (int x = xMin; x <= xMax; ++x)
            {
                for (int y = yMin; y <= yMax; ++y)
                {
                    gridPos.Set(x, y);
                    if (!action(gridPos, dataIdentifier, param))
                    {
                        return (false);
                    }
                }
            }
            return (true);
        }
        #endregion

        #region Data Types
        public class                        TileData
        {
            Dictionary<object, object>      _data = new Dictionary<object, object>();

            internal void                   SetValue<V>(IDataIdentifier dataIdentifier, V val)
            {
                object                      oVal;

                if (this._data.TryGetValue(dataIdentifier, out oVal))
                {
                    if (oVal is V)
                    {
                        if (Equals(val, default(V)))
                        {
                            this._data.Remove(dataIdentifier);
                        }
                        else
                        {
                            this._data[dataIdentifier] = val;
                        }
                    }
                    else
                    {
                        Debug.LogError("Setting different value type for identifier " + dataIdentifier);
                    }     
                }
                else
                {
                    this._data.Add(dataIdentifier, val);
                }
            }

            internal bool                   UnsetValue<V>(IDataIdentifier dataIdentifier)
            {
                return (this._data.Remove(dataIdentifier));
            }

            internal bool                   HasValue(IDataIdentifier dataIdentifier)
            {
                return (this._data.ContainsKey(dataIdentifier));
            }

            internal bool                   HasValues()
            {
                return (this._data.Count > 0);
            }

            internal bool                   TryGetValue<V>(IDataIdentifier dataIdentifier, out V val)
            {
                object                      oVal;

                if (this._data.TryGetValue(dataIdentifier, out oVal)
                    && oVal is V)
                {
                    val = (V)oVal;
                    return (true);
                }
                val = default(V);
                return (false);
            }

            internal void                   Clear(bool shallowCleanup)
            {
                if (!shallowCleanup)
                {
                    foreach (var val in this._data.Values)
                    {
                        if (val is Component || val is GameObject)
                        {
                            Debug.LogError("Clearing out TileData with instance set: " + val, val as Object);
                        }
                    }
                }
                this._data.Clear();
            }
        }
        #endregion
    }

    public enum                         Match
    {
        All,
        Any
    }
}
