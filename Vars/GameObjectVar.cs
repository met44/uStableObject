using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace uStableObject.Data
{
    [CreateAssetMenu(menuName = "uStableObject/Var/GameObject", order = 2)]
    public class                            GameObjectVar : GameEventData
    {
        [SerializeField] GameObject         _go;

        public GameObject                   GameObject
        {
            get
            {
                return (this._go);
            }
            set
            {
                this._go = value;
                this.Changed.Invoke();
            }
        }

        protected override void             RaiseIsNotNull()
        {
            this.ForceRaise(this._go != null);
        }

        public override void                Clear()
        {
            this.GameObject = null;
        }
    }
}
