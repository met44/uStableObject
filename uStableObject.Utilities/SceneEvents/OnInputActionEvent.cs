using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Data;

namespace                               uStableObject.Utilities
{
    public class                        OnInputActionEvent : MonoBehaviour
    {
        #region Input Data
        [SerializeField] UnityEvent     _event = null;
        [SerializeField] InputAction    _inputAction = null;
        #endregion

        #region Unity
        public void                     OnEnable()
        {
            this._inputAction.Register(this.Perform);
        }

        public void                     OnDisable()
        {
            this._inputAction.Unregister(this.Perform);
        }
        #endregion

        #region Trigger
        public void                     Perform()
        {
            if (this.gameObject.activeSelf)
            {
                this._event.Invoke();
            }
        }
        #endregion
    }
}
