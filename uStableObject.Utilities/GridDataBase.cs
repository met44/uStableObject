using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using uStableObject;

namespace                                   uStableObject.Utilities
{
    public abstract class                   GridDataBase<T> : ScriptableObject
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
        public bool                         Clear(Vector2Int tile, bool ignoreInstance = false)
        {
            TileData                        tileData;

            if (this._tiles.TryGetValue(tile, out tileData))
            {
                this._tiles.Remove(tile);
                tileData._element = default(T);
                if (tileData._instance && !ignoreInstance)
                {
                    Debug.LogError("Clearing out TileData with instance set: " + tileData._instance);
                }
                tileData._instance = null;
                tileData._level = 0;
                AutoPool<TileData>.Dispose(tileData);
                return (true);
            }
            return (false);
        }

        public void                         SetTileElement(Vector2Int tile, T elem)
        {
            TileData                        tileData;

            if (!this._tiles.TryGetValue(tile, out tileData))
            {
                tileData = AutoPool<TileData>.Create();
                this._tiles.Add(tile, tileData);
            }
            tileData._element = elem;
        }

        public void                         SetTileElement(Vector2Int tileFrom, Vector2Int tileTo, T elem)
        {
            this.ForEach(tileFrom, tileTo, this.SetTileElement, elem);
        }

        public bool                         GetTileElement(Vector2Int tile, out T elem)
        {
            TileData                        tileData;

            if (this._tiles.TryGetValue(tile, out tileData))
            {
                elem = tileData._element;
            }
            else
            {
                elem = default(T);
            }
            return (elem != null);
        }

        public bool                         HasTileElement(Vector2Int tile)
        {
            T                    elem;
            TileData                        tileData;

            if (this._tiles.TryGetValue(tile, out tileData))
            {
                elem = tileData._element;
            }
            else
            {
                elem = default(T);
            }
            return (elem != null);
        }

        public bool                         HasTileElement(Vector2Int tileFrom, Vector2Int tileTo, bool matchAll = false)
        {
            if (matchAll)
            {
                return (this.All(tileFrom, tileTo, this.HasTileElement));
            }
            else
            {
                return (this.Any(tileFrom, tileTo, this.HasTileElement));
            }
        }

        public bool                         HasTileElement(Vector2Int tile, T searchType)
        {
            T                               elem;
            TileData                        tileData;

            if (this._tiles.TryGetValue(tile, out tileData))
            {
                elem = tileData._element;
            }
            else
            {
                elem = default(T);
            }
            return (Object.Equals(elem, searchType));
        }

        public bool                         HasTileElement(Vector2Int tileFrom, Vector2Int tileTo, T searchType, bool matchAll = false)
        {
            if (matchAll)
            {
                return (this.All(tileFrom, tileTo, this.HasTileElement, searchType));
            }
            else
            {
                return (this.Any(tileFrom, tileTo, this.HasTileElement, searchType));
            }
        }

        public void                         SetTileTransform(Vector2Int tile, Transform tr)
        {
            TileData                        tileData;

            if (!this._tiles.TryGetValue(tile, out tileData))
            {
                if (tr)
                {
                    tileData = AutoPool<TileData>.Create();
                    this._tiles.Add(tile, tileData);
                }
                else
                {
                    return;
                }
            }
            tileData._instance = tr;
        }

        public void                         SetTileTransform(Vector2Int tileFrom, Vector2Int tileTo, Transform tr)
        {
            this.ForEach(tileFrom, tileTo, this.SetTileTransform, tr);
        }

        public bool                         GetTileTransform(Vector2Int tile, out Transform instance)
        {
            TileData                        tileData;

            if (this._tiles.TryGetValue(tile, out tileData))
            {
                instance = tileData._instance;
            }
            else
            {
                instance = null;
            }
            return (instance);
        }

        public void                         SetTileLevel(Vector2Int tile, int level)
        {
            TileData                        tileData;

            if (!this._tiles.TryGetValue(tile, out tileData))
            {
                tileData = AutoPool<TileData>.Create();
                this._tiles.Add(tile, tileData);
            }
            tileData._level = level;
        }

        public void                         SetTileLevel(Vector2Int tileFrom, Vector2Int tileTo, int level)
        {
            this.ForEach(tileFrom, tileTo, this.SetTileLevel, level);
        }

        public bool                         GetTileLevel(Vector2Int tile, out int level)
        {
            TileData                        tileData;

            if (this._tiles.TryGetValue(tile, out tileData))
            {
                level = tileData._level;
                return (true);
            }
            level = -1;
            return (false);
        }

        public void                         ForEach(Vector2Int tileFrom, Vector2Int tileTo, System.Action<Vector2Int> action)
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

        public void                         ForEach<P>(Vector2Int tileFrom, Vector2Int tileTo, System.Action<Vector2Int, P> action, P param)
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

        public bool                         Any<P>(Vector2Int tileFrom, Vector2Int tileTo, System.Func<Vector2Int, P, bool> action, P param)
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

        public bool                         All<P>(Vector2Int tileFrom, Vector2Int tileTo, System.Func<Vector2Int, P, bool> action, P param)
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
            public T                        _element;
            public Transform                _instance;
            public int                      _level;
        }
        #endregion
    }
}
