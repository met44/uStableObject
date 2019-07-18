using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using uStableObject;

namespace                                   uStableObject.Utilities
{
    public abstract class                   GridDataBase : ScriptableObject
    {
        #region Input Data
        [SerializeField] Vector2            _mapSize;
        #endregion

        #region Properties
        public Vector2                      MapSize { get { return (_mapSize); }}
        #endregion

        #region Members
        Dictionary<Vector2Int, TileData>    _tiles = new Dictionary<Vector2Int, TileData>();
        #endregion

        #region Unity
        void                                OnEnable()
        {
            if (this._tiles == null)
            {
                this._tiles = new Dictionary<Vector2Int, TileData>();
            }
            this._tiles.Clear();
        }
        #endregion

        #region Triggers
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
            this.ForEach(tileFrom, tileTo, this.SetTileData, dataIdentifier, data);
        }

        public bool                         GetTileData<V>(Vector2Int tile,  IDataIdentifier dataIdentifier, out V data)
        {
            TileData                        tileData;

            if (this._tiles.TryGetValue(tile, out tileData))
            {
                if (!tileData.TryGetValue(dataIdentifier, out data))
                {
                    return (false);
                }
            }
            else
            {
                data = default(V);
            }
            return (data != null);
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
                if (!tileData.TryGetValue(dataIdentifier, out data))
                {
                    return (false);
                }
            }
            else
            {
                data = default(V);
            }
            return (Object.Equals(data, search));
        }

        public bool                         HasTileData(Vector2Int tileFrom, Vector2Int tileTo, Match match = Match.Any)
        {
            if (match == Match.All)
            {
                return (this.All(tileFrom, tileTo, this.HasTileData));
            }
            else
            {
                return (this.Any(tileFrom, tileTo, this.HasTileData));
            }
        }

        public bool                         HasTileData<V>(Vector2Int tileFrom, Vector2Int tileTo, IDataIdentifier dataIdentifier, Match match = Match.Any)
        {
            if (match == Match.All)
            {
                return (this.All(tileFrom, tileTo, this.HasTileData, dataIdentifier));
            }
            else
            {
                return (this.Any(tileFrom, tileTo, this.HasTileData, dataIdentifier));
            }
        }

        public bool                         HasTileData<V>(Vector2Int tileFrom, Vector2Int tileTo, IDataIdentifier dataIdentifier, V searchType, Match match = Match.Any)
        {
            if (match == Match.All)
            {
                return (this.All(tileFrom, tileTo, this.HasTileData, dataIdentifier, searchType));
            }
            else
            {
                return (this.Any(tileFrom, tileTo, this.HasTileData, dataIdentifier, searchType));
            }
        }
        
        public void                         ForEach<P>(Vector2Int tileFrom, Vector2Int tileTo, System.Action<Vector2Int, IDataIdentifier, P> action,  IDataIdentifier dataIdentifier, P param)
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

        public bool                         Any(Vector2Int tileFrom, Vector2Int tileTo, System.Func<Vector2Int, bool> action)
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

        public bool                         Any(Vector2Int tileFrom, Vector2Int tileTo, System.Func<Vector2Int, IDataIdentifier, bool> action,  IDataIdentifier dataIdentifier)
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

        public bool                         Any<P>(Vector2Int tileFrom, Vector2Int tileTo, System.Func<Vector2Int, IDataIdentifier, P, bool> action,  IDataIdentifier dataIdentifier, P param)
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

        public bool                         All(Vector2Int tileFrom, Vector2Int tileTo, System.Func<Vector2Int, bool> action)
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

        public bool                         All(Vector2Int tileFrom, Vector2Int tileTo, System.Func<Vector2Int, IDataIdentifier, bool> action,  IDataIdentifier dataIdentifier)
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

        public bool                         All<P>(Vector2Int tileFrom, Vector2Int tileTo, System.Func<Vector2Int, IDataIdentifier, P, bool> action,  IDataIdentifier dataIdentifier, P param)
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

        public bool                         Contains(Vector2Int tile)
        {
            return (tile.x > -this.MapSize.x / 2 && tile.x < this.MapSize.x / 2
                    && tile.y > -this.MapSize.y / 2 && tile.y < this.MapSize.y / 2);
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
                        this._data[dataIdentifier] = val;
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

            internal bool                   HasValue(IDataIdentifier dataIdentifier)
            {
                return (this._data.ContainsKey(dataIdentifier));
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

        public interface                IDataIdentifier { }
        #endregion
    }

    public enum                         Match
    {
        All,
        Any
    }
}
