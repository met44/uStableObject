using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace                                   uStableObject.Data
{
    [CreateAssetMenu(menuName = "uStableObject/Var/String", order = 2)]
    public class                            StringVar : GameEventString, ISerializationCallbackReceiver, IBaseTypeVar<string>
    {
        [SerializeField] protected string   _value = null;

        protected string                    _runtimeValue;

        public virtual string               Value
        {
            get
            {
                return (this._runtimeValue);
            }
            set
            {
                if (this._runtimeValue != value)
                {
                    this._runtimeValue = value;
                    this.Raise(value);
                }
            }
        }

        public bool                         HasRuntimeValue()
        {
            return (this._runtimeValue != this._value);
        }

        public static implicit operator     string(StringVar var)
        {
            return (var.Value);
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize() { }
        void ISerializationCallbackReceiver.OnAfterDeserialize() { this._runtimeValue = this._value; }

#if UNITY_EDITOR
        [ContextMenu("Raise event")]
        public void                         RaiseEvent()
        {
            this.Raise(this.Value);
        }
#endif
    }
}
