using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using uStableObject.Data;

namespace                           uStableObject
{
    public abstract class           GameEventData : BoolVar
    {
        #region Input Data
        [SerializeField] UnityEvent _changedEvent;
        #endregion

        #region Properties
        public UnityEvent           Changed { get { return (this._changedEvent); } }
        #endregion

        #region Unity
        protected virtual void      OnEnable()
        {
            if (this._changedEvent == null)
            {
                this._changedEvent = new UnityEvent();
            }
            this._changedEvent.RemoveAllListeners();
            this._changedEvent.AddListener(this.RaiseIsNotNull);
        }
        #endregion

        #region Triggers
        public abstract void        Clear();
        #endregion

        #region Helpers
        protected abstract void     RaiseIsNotNull();
        #endregion
    }
}
