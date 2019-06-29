using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace                                           uStableObject
{
    public class                                    GameEventListenerVector2Int : GameEventListenerBase<Vector2Int>
    {
        public GameEventVector2Int                  _event;
        public UnityEventTypes.V2Int                _response;

        public override GameEventBase<Vector2Int>   Event { get { return (this._event); } }
        public override UnityEvent<Vector2Int>      Response { get { return (this._response); } }
    }
}
