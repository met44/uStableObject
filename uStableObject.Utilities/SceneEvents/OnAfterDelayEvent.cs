using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace                               uStableObject.Utilities
{
    public class                        OnAfterDelayEvent : MonoBehaviour
    {
        #region Input Data
        [SerializeField] UnityEvent     _event;
        [SerializeField] float          _delay = 0;
        #endregion

        #region Unity
        public void                     InitiateDelay()
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

        public void                     InitiateDelay(float delay)
        {
            if (delay == 0)
            {
                this.Perform();
            }
            else
            {
                Invoke("Perform", delay);
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
