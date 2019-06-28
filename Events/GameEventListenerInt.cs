using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace                           uStableObject
{
    public class                    GameEventListenerInt : GameEventListenerBase<int>
    {
        public GameEventInt         _event;
        public UnityEventTypes.Int  _response;

        public override GameEventBase<int>  Event { get { return (this._event); } }
        public override UnityEvent<int>     Response { get { return (this._response); } }
    }
}
