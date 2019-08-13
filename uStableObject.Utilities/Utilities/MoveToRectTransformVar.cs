using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uStableObject.Data;

namespace                                   uStableObject.Utilities
{
    public class                            MoveToRectTransformVar : MonoBehaviour
    {
        #region Input Data
        [SerializeField] CameraVar          _camera;
        [SerializeField] CanvasVar          _canvas;
        [SerializeField] RectTransformVar   _target;
        [SerializeField] Modes              _mode;
        [SerializeField] Positions          _position;
        #endregion

        #region Trigger
        public void                         MoveNow()
        {
            if (this._mode == Modes.CanvasToCameraLocal)
            {
                Vector3                     localPos = Vector3.zero;

                switch (this._position)
                {
                    case Positions.TopLeft:         localPos = new Vector3(this._target.Transform.rect.xMin,    this._target.Transform.rect.yMax,       this._target.Transform.localPosition.z); break;
                    case Positions.TopCenter:       localPos = new Vector3(this._target.Transform.rect.center.x,this._target.Transform.rect.yMax,       this._target.Transform.localPosition.z); break;
                    case Positions.TopRight:        localPos = new Vector3(this._target.Transform.rect.xMax,    this._target.Transform.rect.yMax,       this._target.Transform.localPosition.z); break;
                    case Positions.MiddleLeft:      localPos = new Vector3(this._target.Transform.rect.xMin,    this._target.Transform.rect.center.y,   this._target.Transform.localPosition.z); break;
                    case Positions.MiddleCenter:    localPos = new Vector3(this._target.Transform.rect.center.x,this._target.Transform.rect.center.y,   this._target.Transform.localPosition.z); break;
                    case Positions.MiddleRight:     localPos = new Vector3(this._target.Transform.rect.xMax,    this._target.Transform.rect.center.y,   this._target.Transform.localPosition.z); break;
                    case Positions.BottomLeft:      localPos = new Vector3(this._target.Transform.rect.xMin,    this._target.Transform.rect.yMin,       this._target.Transform.localPosition.z); break;
                    case Positions.BottomCenter:    localPos = new Vector3(this._target.Transform.rect.center.x,this._target.Transform.rect.yMin,       this._target.Transform.localPosition.z); break;
                    case Positions.BottomRight:     localPos = new Vector3(this._target.Transform.rect.xMax,    this._target.Transform.rect.yMin,       this._target.Transform.localPosition.z); break;
                }
                Vector3 worldPos = this._target.Transform.TransformPoint(localPos);
                Vector3 position = this._camera.Camera.CanvasToCameraPoint(this._canvas.Canvas, worldPos, this.transform.localPosition.z);
                this.transform.localPosition = position;
            }
        }
        #endregion

        #region Data Types
        public enum                         Modes
        {
            CanvasToCameraLocal
        }

        public enum                         Positions
        {
            TopLeft, TopCenter, TopRight, MiddleLeft, MiddleCenter, MiddleRight, BottomLeft, BottomCenter, BottomRight
        }
        #endregion
    }
}
