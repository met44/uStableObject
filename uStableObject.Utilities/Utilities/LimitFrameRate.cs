using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Data;

namespace                           uStableObject.Utilities
{
    public class                    LimitFrameRate : MonoBehaviour
    {
        #region Input Data
        [SerializeField] IntVar     _rate;
        #endregion

        #region Trigger
        public void                 ApplyFrameRateLimit()
        {
            Application.targetFrameRate = this._rate;
        }
        #endregion
    }
}
