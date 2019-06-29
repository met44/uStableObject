using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Data;

namespace                       uStableObject
{
    public class                GameEventListener : MonoBehaviour, IGameEventListener
    {
        [SerializeField] BoolVar _filter;

        public GameEvent        _event;
        public UnityEvent       _response;

        private void            OnEnable()
        {
            this._event.Register(this);
        }

        private void            OnDisable()
        {
            this._event.Unregister(this);
        }

        public void             OnEventRaised()
        {
            if (this._filter == null || this._filter.Value)
            {
                this._response.Invoke();
            }
        }
    }
}
