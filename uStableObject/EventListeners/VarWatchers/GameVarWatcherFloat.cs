using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Data;

namespace                                       uStableObject
{
    public class                                GameVarWatcherFloat : GameEventListenerBase<float>
    {
        [SerializeField] FloatVar               _var;
        [SerializeField] UnityEventTypes.Float  _response;

        public override GameEventBase<float>    Event { get { return (this._var); } }
        public override UnityEvent<float>       Response { get { return (this._response); } }

        protected override void                 OnEnable()
        {
            base.OnEnable();
            this.OnEventRaised(this._var);
        }
    }
}
