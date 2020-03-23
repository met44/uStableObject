using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace                                       uStableObject
{
    public class                                GameEventListenerVector3 : GameEventListenerBase<Vector3>
    {
        public GameEventVector3                 _event;
        public UnityEventTypes.V3               _response;

        public override GameEventBase<Vector3>  Event { get { return (this._event); } }
        public override UnityEvent<Vector3>     Response { get { return (this._response); } }
    }
}
