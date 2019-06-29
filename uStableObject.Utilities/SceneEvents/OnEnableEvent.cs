using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace                               uStableObject.Utilities
{
    public class                        OnEnableEvent : MonoBehaviour
    {
        #region Input Data
        [SerializeField] UnityEvent     _event;
        [SerializeField] float          _delay = 0;
        [SerializeField] bool           _ignoreTimeScale = false;
        #endregion

        #region Members
        CancellationTokenSource         _cancelationSource;
        #endregion

        #region Unity
        public void                     OnEnable()
        {
            if (this._delay == 0)
            {
                this.Perform();
            }
            else if (this._ignoreTimeScale)
            {
                this.InitiateDelayedPerform();
            }
            else
            {
                Invoke("Perform", this._delay);
            }
        }

        void                            OnDisable()
        {
            if (this._delay != 0)
            {
                if (this._ignoreTimeScale)
                {
                    if (this._cancelationSource != null)
                    {
                        this._cancelationSource.Cancel();
                        this._cancelationSource = null;
                    }
                }
                else
                {
                    this.CancelInvoke("Perform");
                }
            }
        }
        #endregion

        #region Callback
        async void                      InitiateDelayedPerform()
        {
            await this.DelayedPerform();
        }

        async Task                      DelayedPerform()
        {
            var cancelSource = AutoPool<CancellationTokenSource>.Create();
            this._cancelationSource = cancelSource;
            var token = cancelSource.Token;
            try
            {
                await Task.Delay((int)(this._delay * 1000), token);
                if (!token.IsCancellationRequested)
                {
                    this.Perform();
                    AutoPool<CancellationTokenSource>.Dispose(cancelSource);
                }
                else
                {
                    cancelSource.Dispose();
                }
            }
            catch (TaskCanceledException)
            {

            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
            }
            if (this._cancelationSource == cancelSource)
            {
                this._cancelationSource = null;
            }
        }

        void                            Perform()
        {
            if (this.gameObject.activeSelf && this.enabled)
            {
                this._event.Invoke();
            }
        }
        #endregion
    }
}
