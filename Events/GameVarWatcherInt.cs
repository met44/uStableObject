using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Data;

namespace                                       uStableObject
{
    public class                                GameVarWatcherInt : GameEventListenerBase<int>
    {
        [SerializeField] IntVar                 _var;
        [SerializeField] UnityEventTypes.Int    _response;

        public override GameEventBase<int>      Event { get { return (this._var); } }
        public override UnityEvent<int>         Response { get { return (this._response); } }

        protected override void                 OnEnable()
        {
            base.OnEnable();
            this.OnEventRaised(this._var);
        }
    }
}
