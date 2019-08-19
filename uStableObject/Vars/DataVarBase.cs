using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;

namespace                                   uStableObject.Data
{
    public class                            DataVarBase<T, E> : GameEventBase<T>, ISerializationCallbackReceiver, IBaseTypeVar<T>
                                            where T : IEquatable<T>
                                            where E : GameEventBase<T>
    {
        [SerializeField] T                  _value;

        T                                   _runtimeValue;

        public virtual T                    Value
        {
            get
            {
                return (this._runtimeValue);
            }
            set
            {
                if (!this._runtimeValue.Equals(value))
                {
                    this._runtimeValue = value;
                    this.Raise(value);
                }
            }
        }

        public bool                         HasRuntimeValue()
        {
            return (!this._runtimeValue.Equals(this._value));
        }
        
        void ISerializationCallbackReceiver.OnBeforeSerialize() { }
        void ISerializationCallbackReceiver.OnAfterDeserialize() { this._runtimeValue = this._value; }

        public static implicit operator     E(DataVarBase<T, E> var)
        {
            return (var as E);
        }

#if UNITY_EDITOR
        [ContextMenu("Raise event")]
        public void                         RaiseEvent()
        {
            this.Raise(this._runtimeValue);
        }
#endif
    }
}
