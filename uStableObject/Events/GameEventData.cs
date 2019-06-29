using uStableObject.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace                           uStableObject
{
    public abstract class           GameEventData : BoolVar
    {
        [SerializeField] UnityEvent _changedEvent;
        public UnityEvent           Changed { get { return (this._changedEvent); } }

        protected virtual void      OnEnable()
        {
            if (this._changedEvent == null)
            {
                this._changedEvent = new UnityEvent();
            }
            this._changedEvent.RemoveAllListeners();
            this._changedEvent.AddListener(this.RaiseIsNotNull);
        }

        protected abstract void     RaiseIsNotNull();

        public abstract void        Clear();
    }
}
