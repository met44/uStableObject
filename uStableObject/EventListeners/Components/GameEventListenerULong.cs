using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace                                       uStableObject
{
    public class                                GameEventListenerULong : GameEventListenerBase<ulong>
    {
        public GameEventULong                   _event;
        public UnityEventTypes.ULong            _response;

        public override GameEventBase<ulong>    Event { get { return (this._event); } }
        public override UnityEvent<ulong>       Response { get { return (this._response); } }
    }
}
