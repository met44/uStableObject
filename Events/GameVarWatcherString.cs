using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Data;

namespace                                       uStableObject
{
    public class                                GameVarWatcherString : GameEventListenerBase<string>
    {
        [SerializeField] StringVar              _var;
        [SerializeField] UnityEventTypes.String _response;

        public override GameEventBase<string>   Event { get { return (this._var); } }
        public override UnityEvent<string>      Response { get { return (this._response); } }

        protected override void                 OnEnable()
        {
            base.OnEnable();
            this.OnEventRaised(this._var);
        }
    }
}
