using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace                                   uStableObject.Data
{
    [CreateAssetMenu(menuName = "uStableObject/Var/Transform", order = 2)]
    public class                            TransformVar : GameEventData
    {
        [SerializeField] Transform          _tr;

        public Transform                    Transform
        {
            get
            {
                return (this._tr);
            }
            set
            {
                this._tr = value;
                this.Changed.Invoke();
            }
        }

        protected override void             RaiseIsNotNull()
        {
            this.ForceRaise(this._tr != null);
        }

        public override void                Clear()
        {
            this.Transform = null;
        }
    }
}
