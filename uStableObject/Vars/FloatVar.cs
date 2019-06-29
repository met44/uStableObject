using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace uStableObject.Data
{
    [CreateAssetMenu(menuName = "uStableObject/Var/Float", order = 2)]
    public class                            FloatVar : GameEventFloat, ISerializationCallbackReceiver, IBaseTypeVar<float>
    {
        [SerializeField] float              _value = 0;

        float                               _runtimeValue;

        public virtual float                Value
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

        public static implicit operator     float(FloatVar var)
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
