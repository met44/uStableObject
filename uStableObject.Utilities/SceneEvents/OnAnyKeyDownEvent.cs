using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace                               uStableObject.Utilities
{
    public class                        OnAnyKeyDownEvent : MonoBehaviour
    {
        #region Input Data
        [SerializeField] UnityEvent     _event;
        #endregion

        #region Unity
        void                            Update()
        {
            if (Input.anyKeyDown)
            {
                this._event.Invoke();
            }
        }
        #endregion
    }
}