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
            this._event.Invoke(this.gameObject.activeSelf != this._invert);
        }
        #endregion
    }
}
