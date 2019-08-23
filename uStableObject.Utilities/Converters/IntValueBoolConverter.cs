using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using uStableObject.Utilities;

namespace                                       uStableObject.Utilities.Converters
{
    public class                                IntValueBoolConverter : MonoBehaviour
    {
        #region Input Data
        [SerializeField] UnityEventTypes.Bool   _ouputValue;
        #endregion

        #region Triggers
        public void                             ConvertValue(int val)
        {
            this._ouputValue.Invoke(val != 0);
        }
        #endregion
    }
}
