using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace                                       uStableObject.Utilities
{
    public class                                Ticker : MonoBehaviour
    {
        #region Input Data
        [SerializeField] UnityEventTypes.Float  _onTick;
        [SerializeField] UnityEvent             _onFinished;
        [SerializeField] float                  _duration;
        [SerializeField] float                  _interval;
        [SerializeField] bool                   _realTime;
        #endregion

        #region Triggers
        public void                             StartTicker(float duration, float interval)
        {
            this.StartCoroutine(this.TickerRoutine(duration, interval));
        }

        public void                             StartTicker()
        {
            this.StartCoroutine(this.TickerRoutine(this._duration, this._interval));
        }
        #endregion

        #region Routines
        IEnumerator                             TickerRoutine(float duration, float interval)
        {
            float startTime = this._realTime ? Time.realtimeSinceStartup : Time.time;
            WaitForSeconds delay = new WaitForSeconds(interval);
            while (startTime + duration > (this._realTime ? Time.realtimeSinceStartup : Time.time))
            {
                yield return delay;
                float progress = ((this._realTime ? Time.realtimeSinceStartup : Time.time) - startTime) / duration;
                this._onTick.Invoke(progress);
            }
            this._onFinished.Invoke();
        }
        #endregion
    }
}