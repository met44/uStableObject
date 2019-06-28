using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace                               uStableObject
{
    public class                        GameEventListenerInt2 : GameEventListenerBase<int, int>
    {
        public GameEventInt2            _event;
        public UnityEventTypes.Int2     _response;

        public override GameEventBase<int, int>  Event { get { return (this._event); } }
        public override UnityEvent<int, int>     Response { get { return (this._response); } }
    }
}
