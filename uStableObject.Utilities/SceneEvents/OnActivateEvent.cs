using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace                                       uStableObject.Utilities
{
    public class                                OnActivateEvent : MonoBehaviour
    {
        #region Input Data
        [SerializeField] UnityEventTypes.Bool   _event;
        [SerializeField] bool                   _invert;
        [SerializeField] Scopes                 _scope;
        #endregion

        #region Unity
        void                                    OnEnable()
        {
            this.Perform();
        }

        void                                    OnDisable()
        {
            this.Perform();
        }
        #endregion

        #region Trigger
        void                                    Perform()
        {
            if (this._scope == Scopes.Self)
            {
                this._event.Invoke(this.gameObject.activeSelf != this._invert);
            }
            else
            {
                this._event.Invoke(this.gameObject.activeInHierarchy != this._invert);
            }
        }
        #endregion

        #region Data Types
        enum                                    Scopes
        {
            Self, Hierarchy
        }
        #endregion
    }
}
