using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Data;

namespace                                       uStableObject
{
    public class                                GameVarWatcherBool : GameEventListenerBase<bool>
    {
        [SerializeField] BoolVar                _var;
        [SerializeField] UnityEventTypes.Bool   _response;
        [SerializeField] BoolVar                _invertedfilter;
        [SerializeField] UnityEventTypes.Bool   _responseInverted;

        public override GameEventBase<bool>     Event { get { return (this._var); } }
        public override UnityEvent<bool>        Response { get { return (this._response); } }

        protected override void                 OnEnable()
        {
            base.OnEnable();
            this.OnEventRaised(this._var);
        }

        public override void                    OnEventRaised(bool param)
        {
            if (this._filter == null || this._filter.Value)
            {
                this._response.Invoke(param);
            }
            if (this._invertedfilter == null || this._invertedfilter.Value)
            {
                this._responseInverted.Invoke(!param);
            }
        }
    }
}
