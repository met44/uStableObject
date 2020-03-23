using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Data;

namespace                               uStableObject.Utilities
{
    public class                        OnApplicationPauseEvent : MonoBehaviour
    {
        #region Input Data
        [SerializeField] BoolVar        _filter = null;
        [SerializeField] UnityEvent     _onPaused = null;
        [SerializeField] UnityEvent     _onUnpaused = null;
        #endregion

        #region Unity
        void                            OnApplicationPause(bool paused)
        {
            Debug.Log("OnApplicationPause: paused=" + paused);
            if (this._filter == null || this._filter.Value)
            {
                if (paused)
                {
                    this._onPaused.Invoke();
                }
                else
                {
                    this._onUnpaused.Invoke();
                }
            }
        }
        #endregion

        #region Triggers
        [ContextMenu("Fake Pause")]
        public void                     FakePause()
        {
            this.OnApplicationPause(true);
        }

        [ContextMenu("Fake Unpause")]
        public void                     FakeUnpause()
        {
            this.OnApplicationPause(false);
        }
        #endregion
    }
}
