using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Data;

namespace                               uStableObject.Utilities
{
    public class                        OnApplicationQuitEvent : MonoBehaviour
    {
        #region Input Data
        [SerializeField] BoolVar        _filter = null;
        [SerializeField] UnityEvent     _event = null;
        #endregion

        #region Unity
        void                            OnApplicationQuit()
        {
            if (this._filter == null || this._filter.Value)
            {
                this._event.Invoke();
            }
        }
        #endregion
    }
}
