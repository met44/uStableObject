using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace                                   uStableObject.Data
{
    [CreateAssetMenu(menuName = "uStableObject/Var/Sprite", order = 2)]
    public class                            SpriteVar : GameEventData
    {
        [SerializeField] Sprite             _sprite;

        public Sprite                       Sprite
        {
            get
            {
                return (this._sprite);
            }
            set
            {
                this._sprite = value;
                this.Changed.Invoke();
            }
        }

        protected override void             RaiseIsNotNull()
        {
            this.ForceRaise(this._sprite != null);
        }

        public override void                Clear()
        {
            this.Sprite = null;
        }
    }
}
