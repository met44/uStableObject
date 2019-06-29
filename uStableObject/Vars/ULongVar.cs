using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace                                   uStableObject.Data
{
    [CreateAssetMenu(menuName = "uStableObject/Var/Unsigned Long", order = 2)]
    public class                            ULongVar : GameEventULong, ISerializationCallbackReceiver, IBaseTypeVar<ulong>
    {
        [SerializeField] ulong              _value = 0;

        ulong                               _runtimeValue;

        public virtual ulong                Value
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

        public static implicit operator     ulong(ULongVar var)
        {
            return (var.Value);
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize() { }
        void ISerializationCallbackReceiver.OnAfterDeserialize() { this._runtimeValue = this._value; }

#if UNITY_EDITOR
        [ContextMenu("Raise event")]
        public void                         RaiseEvent()
        {
            this.Raise(this._runtimeValue);
        }
#endif
    }
}
