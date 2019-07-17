using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace                                   uStableObject.Data
{
    [CreateAssetMenu(menuName = "uStableObject/Var/Vector2Int", order = 2)]
    public class                            Vector2IntVar : GameEventVector2Int, ISerializationCallbackReceiver, IBaseTypeVar<Vector2Int>
    {
        [SerializeField] Vector2Int         _value;

        Vector2Int                          _runtimeValue;

        public virtual Vector2Int           Value
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

        public static implicit operator     Vector2Int(Vector2IntVar var)
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
