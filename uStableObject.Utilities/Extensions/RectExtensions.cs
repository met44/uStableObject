using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace                       uStableObject.Utilities
{
    public static class         RectExtensions
    {
        public static void      ForEach(this Rect r, System.Action<int, int> a)
        {
            int colMax = Mathf.CeilToInt(r.xMax);
            int rowMax = Mathf.CeilToInt(r.yMax);
            for (int iCol = (int)r.xMin; iCol <= colMax; ++iCol)
            {
                for (int iRow = (int)r.yMin; iRow <= rowMax; ++iRow)
                {
                    a(iRow, iCol);
                }
            }
        }
    }
}
