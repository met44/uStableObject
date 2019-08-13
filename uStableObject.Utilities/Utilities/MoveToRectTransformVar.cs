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
        #endregion

        #region Trigger
        public void                         MoveNow()
        {
            if (this._mode == Modes.CanvasToCameraLocal)
            {
                Vector3 position = this._camera.Camera.CanvasToCameraPoint(this._canvas.Canvas, this._target.Transform.position, this.transform.localPosition.z);
                this.transform.localPosition = position;
            }
        }
        #endregion

        #region Data Types
        public enum                         Modes
        {
            CanvasToCameraLocal
        }
        #endregion
    }
}
