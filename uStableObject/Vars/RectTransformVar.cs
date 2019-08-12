using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace                                   uStableObject.Data
{
    [CreateAssetMenu(menuName = "uStableObject/Var/Rect Transform", order = 2)]
    public class                            RectTransformVar : GameEventData
    {
        [SerializeField] RectTransform      _tr;

        public RectTransform                Transform
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
