using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace                                   uStableObject
{
    public class                            GameEventListenerFloat : GameEventListenerBase<float>
    {
        public GameEventFloat               _event;
        public UnityEventTypes.Float        _response;

        public override GameEventBase<float>  Event { get { return (this._event); } }
        public override UnityEvent<float>     Response { get { return (this._response); } }
    }
}
