using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace                               uStableObject.Utilities
{
    public class                        OnDisableEvent : MonoBehaviour
    {
        #region Input Data
        [SerializeField] UnityEvent     _event;
        #endregion

        #region Unity
        void                            OnDisable()
        {
            this._event.Invoke();
        }
        #endregion
    }
}
