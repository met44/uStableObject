using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using uStableObject.Utilities;

namespace                                       uStableObject.Utilities.Converters
{
    public class                                FloatValueVector3Converter : MonoBehaviour
    {
        #region Input Data
        [SerializeField] Vector3                _otherFieldValues = Vector3.one;
        [SerializeField] UnityEventTypes.V3     _ouputValue;
        #endregion

        #region Triggers
        public void                             ConvertValueX(float val)
        {
            this._ouputValue.Invoke(new Vector3(val, this._otherFieldValues.y, this._otherFieldValues.z));
        }

        public void                             ConvertValueY(float val)
        {
            this._ouputValue.Invoke(new Vector3(this._otherFieldValues.x, val, this._otherFieldValues.z));
        }

        public void                             ConvertValueZ(float val)
        {
            this._ouputValue.Invoke(new Vector3(this._otherFieldValues.x, this._otherFieldValues.y, val));
        }
        #endregion
    }
}
