using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace                                       uStableObject
{
    public class                                GameEventListenerString : GameEventListenerBase<string>
    {
        public GameEventString                  _event;
        public UnityEventTypes.String           _response;

        public override GameEventBase<string>   Event { get { return (this._event); } }
        public override UnityEvent<string>      Response { get { return (this._response); } }
    }
}
