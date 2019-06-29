using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using uStableObject.Utilities;

namespace                                       uStableObject.Utilities.Converters
{
    public class                                IntValueStringConverter : MonoBehaviour
    {
        #region Input Data
        [SerializeField] UnityEventTypes.String _ouputValue;
        #endregion

        #region Triggers
        public void                             ConvertValue(int val)
        {
            this._ouputValue.Invoke(val.ToStringNonAlloc());
        }
        #endregion
    }
}
