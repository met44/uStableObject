using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace uStableObject.Data
{
    [CreateAssetMenu(menuName = "uStableObject/Var/Integer", order = 2)]
    public class                            IntVar : GameEventInt, ISerializationCallbackReceiver, IBaseTypeVar<int>
    {
        [SerializeField] int                _value = 0;
        [SerializeField] int                _minValue = int.MinValue;
        [SerializeField] int                _maxValue = int.MaxValue;

        protected int                       _runtimeValue;

        public virtual int                  Value
        {
            get
            {
                return (this._runtimeValue);
            }
            set
            {
                if (value > this._maxValue)
                {
                    this._runtimeValue = this._maxValue;
                    this.Raise(this._maxValue);
                }
                if (value < this._minValue)
                {
                    this._runtimeValue = this._minValue;
                    this.Raise(this._minValue);
                }
                else if (this._runtimeValue != value)
                {
                    this._runtimeValue = value;
                    this.Raise(value);
                }
            }
        }

        public virtual int                  MinValue
        {
            get
            {
                return (this._minValue);
            }
        }

        public virtual int                  MaxValue
        {
            get
            {
                return (this._maxValue);
            }
        }

        public bool                         HasRuntimeValue()
        {
            return (this._runtimeValue != this._value);
        }

        public void                         Increment()
        {
            if (this._runtimeValue + 1 <= this._maxValue)
            {
                ++this.Value;
            }
        }

        public void                         Add(int val)
        {
            this.Value += val;
        }

        public void                         Substract(int val)
        {
            this.Value -= val;
        }

        public void                         Add(IntVar otherVar)
        {
            this.Value += otherVar;
        }

        public void                         Substract(IntVar otherVar)
        {
            this.Value -= otherVar;
        }

        public void                         Copy(IntVar otherVar)
        {
            this.Value = otherVar;
        }

        public static implicit operator     int(IntVar var)
        {
            return (var.Value);
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize() { }
        void ISerializationCallbackReceiver.OnAfterDeserialize() { this._runtimeValue = this._value; }

        [ContextMenu("Raise event")]
        public void                         RaiseEvent()
        {
            this.Raise(this.Value);
        }
    }
}
