using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace                                   uStableObject.Data
{
    [CreateAssetMenu(menuName = "uStableObject/Var/Canvas", order = 2)]
    public class                            CanvasVar : GameEventData
    {
        [SerializeField] Canvas             _canvas;

        public Canvas                       Canvas
        {
            get
            {
                return (this._canvas);
            }
            set
            {
                this._canvas = value;
                this.Changed.Invoke();
            }
        }

        protected override void             RaiseIsNotNull()
        {
            this.ForceRaise(this._canvas != null);
        }

        public override void                Clear()
        {
            this.Canvas = null;
        }
    }
}
