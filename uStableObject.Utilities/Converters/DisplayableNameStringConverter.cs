using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using uStableObject.Data;
using uStableObject.Utilities;

namespace                                       uStableObject.Utilities.Converters
{
    public class                                DisplayableNameStringConverter : MonoBehaviour
    {
        #region Input Data
        [SerializeField] UnityEventTypes.String _ouputValue;
        #endregion

        #region Triggers
        public void                             ConvertValue(IDisplayable val)
        {
            this._ouputValue.Invoke(val.Name);
        }
        #endregion
    }
}
