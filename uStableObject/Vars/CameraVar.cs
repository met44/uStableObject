using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace                                   uStableObject.Data
{
    [CreateAssetMenu(menuName = "uStableObject/Var/Camera", order = 2)]
    public class                            CameraVar : GameEventData
    {
        [SerializeField] Camera             _camera;

        public Camera                       Camera
        {
            get
            {
                return (this._camera);
            }
            set
            {
                this._camera = value;
                this.Changed.Invoke();
            }
        }

        protected override void             RaiseIsNotNull()
        {
            this.ForceRaise(this._camera != null);
        }

        public override void                Clear()
        {
            this.Camera = null;
        }
    }
}
