using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace                                   uStableObject
{
    public class                            GameEventListenerBool : GameEventListenerBase<bool>
    {
        public GameEventBool                _event;
        public UnityEventTypes.Bool         _response;
        public UnityEventTypes.Bool         _responseInverted;

        public override GameEventBase<bool> Event { get { return (this._event); } }
        public override UnityEvent<bool>    Response { get { return (this._response); } }

        public override void                OnEventRaised(bool param)
        {
            if (this._filter == null || this._filter.Value)
            {
                this._response.Invoke(param);
                this._responseInverted.Invoke(!param);
            }
        }
    }
}
