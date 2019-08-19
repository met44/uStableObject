using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace                               uStableObject.Utilities
{
    public class                        OnAwakeEvent : MonoBehaviour
    {
        #region Input Data
        [SerializeField] UnityEvent     _event;
        [SerializeField] float          _delay = 0;
        #endregion

        #region Unity
        void                            Awake()
        {
            if (this._delay == 0)
            {
                this.Perform();
            }
            else
            {
                Invoke("Perform", this._delay);
            }
        }
        #endregion

        #region Trigger
        void                            Perform()
        {
            if (this.gameObject.activeSelf)
            {
                this._event.Invoke();
            }
        }
        #endregion
    }
}
