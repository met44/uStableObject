using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Data;

namespace                       uStableObject
{
    public abstract class       GameEventListenerBase<T> : MonoBehaviour, IGameEventListener<T>
    {
        [SerializeField] protected BoolVar  _filter;

        public abstract GameEventBase<T>    Event { get; }
        public abstract UnityEvent<T>       Response { get; }

        protected virtual void  OnEnable()
        {
            this.Event.Register(this);
        }

        private void            OnDisable()
        {
            this.Event.Unregister(this);
        }

        public virtual void     OnEventRaised(T param)
        {
            if (this._filter == null || this._filter.Value)
            {
                this.Response.Invoke(param);
            }
        }
    }

    public abstract class       GameEventListenerBase<T1, T2> : MonoBehaviour, IGameEventListener<T1, T2>
    {
        [SerializeField] BoolVar _filter;

        public abstract GameEventBase<T1, T2>    Event { get; }
        public abstract UnityEvent<T1, T2>       Response { get; }

        private void            OnEnable()
        {
            this.Event.Register(this);
        }

        private void            OnDisable()
        {
            this.Event.Unregister(this);
        }

        public void             OnEventRaised(T1 param1, T2 param2)
        {
            if (this._filter == null || this._filter.Value)
            {
                this.Response.Invoke(param1, param2);
            }
        }
    }
}
