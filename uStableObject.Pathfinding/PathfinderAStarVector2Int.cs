using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sayve;

namespace               uStableObject.Utilities
{
    public class        PathfinderAStarVector2Int : PathfinderAStarBase<Vector2Int>
    {
#if UNITY_EDITOR
        protected override void ProcessPath(Vector2Int from, Vector2Int bestTile)
        {
            while (!object.Equals(bestTile, from))
            {
                Vector2Int prev = bestTile;
                this._path.Insert(0, bestTile);
                bestTile = this._tilesData[bestTile]._prev;
                DebugEx.DrawArrow(bestTile.ToWorld(), prev.ToWorld(), Vector3.up, Color.green, 0.15f, 1);
            }
            this._path.Insert(0, from);
        }

        protected override TileData InitTileData(Vector2Int nTile, Vector2Int prevTile)
        {
            TileData data = base.InitTileData(nTile, prevTile);
            if (this._openedSorted.Count > 0)
            {
                DebugEx.DrawArrow(prevTile.ToWorld(), nTile.ToWorld(), Vector3.up, Color.red, 0.15f, 1);
            }
            return (data);
        }
#endif
    }
}
