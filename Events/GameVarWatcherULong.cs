using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Data;

namespace                                       uStableObject
{
    public class                                GameVarWatcherULong : GameEventListenerBase<ulong>
    {
        [SerializeField] ULongVar               _var;
        [SerializeField] UnityEventTypes.ULong  _response;

        public override GameEventBase<ulong>    Event { get { return (this._var); } }
        public override UnityEvent<ulong>       Response { get { return (this._response); } }

        protected override void                 OnEnable()
        {
            base.OnEnable();
            this.OnEventRaised(this._var);
        }
    }
}
