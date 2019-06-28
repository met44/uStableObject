using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace                                       uStableObject
{
    public class                                GameEventListenerVector2 : GameEventListenerBase<Vector2>
    {
        public GameEventVector2                 _event;
        public UnityEventTypes.V2          _response;

        public override GameEventBase<Vector2>  Event { get { return (this._event); } }
        public override UnityEvent<Vector2>     Response { get { return (this._response); } }
    }
}
