using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using uStableObject.Data;

namespace                                   uStableObject.Utilities
{
    public class                            MoveToRectTransformVar : MonoBehaviour
    {
        #region Input Data
        [FormerlySerializedAs("_camera")]
        [SerializeField] CameraVar          _cameraVar;
        [FormerlySerializedAs("_canvas")]
        [SerializeField] CanvasVar          _canvasVar;
        [FormerlySerializedAs("_target")]
        [SerializeField] RectTransformVar   _targetVar;
        [SerializeField] RectTransform      _targetOverride;
        [SerializeField] CoordsConversion   _mode;
        [SerializeField] RectPositions      _position;
        #endregion

        #region Members
        float                               _depth;
        #endregion

        #region Unity
        void                                Awake()
        {
            this._depth = this.transform.localPosition.z;
        }
        #endregion

        #region Trigger
        public void                         SetDepth(float depth)
        {
            this._depth = depth;
        }
        
        public void                         SetOverrideTarget(RectTransform overrideTarget)
        {
            this._targetOverride = overrideTarget;
        }

        public void                         MoveNow()
        {
            RectTransform targetRT = this._targetOverride ? this._targetOverride : this._targetVar.Transform;
            if (this._mode == CoordsConversion.CanvasToCameraLocal)
            {
                Vector3 position = this._cameraVar.Camera.CanvasToCameraLocalPoint(this._canvasVar.Canvas, targetRT, (RectPositions)this._position, this.transform.localPosition.z);
                this.transform.localPosition = position;
            }
            else if (this._mode == CoordsConversion.CanvasToCameraWorld)
            {
                Vector3 position = this._cameraVar.Camera.CanvasToCameraWorldPoint(this._canvasVar.Canvas, targetRT, (RectPositions)this._position, this.transform.localPosition.z);
                this.transform.position = position;
            }
            else
            {
                Vector3 position = targetRT.GetRectPointWorld(this._position);
                this.transform.position = position;
            }
        }
        #endregion
    }
}
