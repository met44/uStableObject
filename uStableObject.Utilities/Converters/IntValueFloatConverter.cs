using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using uStableObject.Utilities;

namespace                                       uStableObject.Utilities.Converters
{
    public class                                IntValueFloatConverter : MonoBehaviour
    {
        #region Input Data
        [SerializeField] UnityEventTypes.Float  _ouputValue;
        #endregion

        #region Triggers
        public void                             ConvertValue(int val)
        {
            this._ouputValue.Invoke(val);
        }
        #endregion
    }
}
