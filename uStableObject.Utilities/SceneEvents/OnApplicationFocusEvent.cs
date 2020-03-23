using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Data;

namespace                               uStableObject.Utilities
{
    public class                        OnApplicationFocusEvent : MonoBehaviour
    {
        #region Input Data
        [SerializeField] BoolVar        _filter = null;
        [SerializeField] UnityEvent     _onFocused = null;
        [SerializeField] UnityEvent     _onUnfocused = null;
        #endregion

        #region Unity
        void                            OnApplicationFocus(bool focused)
        {
            Debug.Log("OnApplicationFocus: focused=" + focused);
            if (this._filter == null || this._filter.Value)
            {
                if (focused)
                {
                    this._onFocused.Invoke();
                }
                else
                {
                    this._onUnfocused.Invoke();
                }
            }
        }
        #endregion
    }
}
