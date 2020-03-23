using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Data;

namespace                           uStableObject.Utilities
{
    public class                    UnloadUnusedAssets : MonoBehaviour
    {
        #region Input Data
        #endregion

        #region Trigger
        public void                 UnloadUnusedAssetsNow()
        {
            Resources.UnloadUnusedAssets();
        }
        #endregion
    }
}
