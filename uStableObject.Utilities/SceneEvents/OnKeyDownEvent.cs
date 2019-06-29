using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace                               uStableObject.Utilities
{
    public class                        OnKeyDownEvent : MonoBehaviour
    {
        #region Input Data
        [SerializeField] KeyCode        _key;
        [SerializeField] UnityEvent     _event;
        #endregion

        #region Unity
        void                            Update()
        {
            if (Input.GetKeyDown(this._key))
            {
                this.Submit();
            }
        }
        #endregion

        #region Triggers
        public void                     Submit()
        {
            this._event.Invoke();
        }
        #endregion
    }
}