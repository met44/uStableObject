﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using uStableObject.Utilities;

namespace                                       uStableObject.Utilities.Scalers
{
    public class                                IntValueScaler : MonoBehaviour
    {
        #region Input Data
        [SerializeField] UnityEventTypes.Int    _ouputValue;
        [SerializeField] float                  _scale;
        #endregion

        #region Triggers
        public void                             ScaleValue(int val)
        {
            this._ouputValue.Invoke(Mathf.RoundToInt(val * this._scale));
        }

        public void                             ScaleValue(float val)
        {
            this._ouputValue.Invoke(Mathf.RoundToInt(val * this._scale));
        }
        #endregion
    }
}
