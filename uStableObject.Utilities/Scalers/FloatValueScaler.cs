using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using uStableObject.Utilities;

namespace                                       uStableObject.Utilities.Scalers
{
    public class                                FloatValueScaler : MonoBehaviour
    {
        #region Input Data
        [SerializeField] UnityEventTypes.Float  _ouputValue;
        [SerializeField] float                  _scale;
        #endregion

        #region Triggers
        public void                             ScaleValue(int val)
        {
            this._ouputValue.Invoke(val * this._scale);
        }

        public void                             ScaleValue(float val)
        {
            this._ouputValue.Invoke(val * this._scale);
        }
        #endregion
    }
}
