using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace                                   uStableObject.Data
{
    [CreateAssetMenu(menuName = "uStableObject/Var/Bool", order = 2)]
    public class                            BoolVar : GameEventBool, ISerializationCallbackReceiver, IBaseTypeVar<bool>
    {
        [SerializeField] bool               _value;

        bool                                _runtimeValue;

        public virtual bool                 Value
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

        public static implicit operator     bool(BoolVar var)
        {
            return (var.Value);
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize() { }
        void ISerializationCallbackReceiver.OnAfterDeserialize() { this._runtimeValue = this._value; }

        [ContextMenu("Raise event")]
        public void                         RaiseCurrent()
        {
            this.Raise(this.Value);
        }

        [ContextMenu("Try Raise event")]
        public void                         TryRaiseCurrent()
        {
            this.Value = this.Value; // for dynamically generated values
        }

        public void                         ForceRaise(bool value)
        {
            this._runtimeValue = value;
            this.Raise(value);
        }

        public void                         Toggle()
        {
            this.Value = !this.Value;
        }
    }
}
