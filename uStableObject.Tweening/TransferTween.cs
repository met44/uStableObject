using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using uStableObject.Data;
using uStableObject.Utilities;

namespace                                   uStableObject.Tweening
{
    public class                            TransferTween : MonoBehaviour
    {
        #region Input Data
        [SerializeField] EasingFunction.Ease _easing;
        [SerializeField] TransformVar       _tweenTargetVar;
        [SerializeField] Transform          _tweenTargetOverride;
        [Header("Tweening"), EnumMask]
        [SerializeField] Modes              _mode;
        [SerializeField] Interval           _tweenDuration;
        [Header("Rect Target")]
        [SerializeField] CameraVar          _cameraVar;
        [SerializeField] CanvasVar          _canvasVar;
        [SerializeField] float              _depth;
        [SerializeField] CoordsConversion   _coords;
        [SerializeField] RectPositions      _position;
        [Header("Events")]
        [SerializeField] UnityEvent         _onDone;
        #endregion

        #region Members
        Vector3                             _tweenTargetWorldPositionFrom;
        Vector3                             _targetPosition;
        float                               _followStartTime;
        float                               _duration;
        bool                                _toFixedPosition;
        bool                                _tweening;
        #endregion

        #region Properties
        public Transform                    TweenTarget
        {
            get
            {
                return (this._tweenTargetOverride ? this._tweenTargetOverride : this._tweenTargetVar?.Transform);
            }
        }

        public Transform                    PreferredParent
        {
            get
            {
                return (this.transform is RectTransform
                        ? this._canvasVar.Canvas.transform
                        : this._coords == CoordsConversion.CanvasToCameraLocal
                            ? this._cameraVar.Camera.transform
                            : null);
            }
        }
        #endregion

        #region Unity
        void                                Update()
        {
            if (this._toFixedPosition)
            {
                this.ApplyTween();
            }
            else if (this.TweenTarget)
            {
                if (this._mode == Modes.AutoFollow)
                {
                    this.CheckAutoFollow();
                }
                if (this._mode == Modes.LiveUpdateTargetPosition)
                {
                    this.RefreshTargetPosition();
                }
                this.ApplyTween();
            }
        }
        #endregion

        #region Triggers
        public TransferTween                SpawnClone(Vector2 screenPos, float depth)
        {
            Vector3                         worldPosition;

            if (this.transform is RectTransform)
            {
                Vector3 canvasLocalPosition = this._cameraVar.Camera.ScreenToCanvasPoint(this._canvasVar.Canvas, screenPos);
                worldPosition = this._canvasVar.Canvas.transform.TransformPoint(canvasLocalPosition);
            }
            else
            {
                worldPosition = this._cameraVar.Camera.ScreenToWorldPoint(screenPos);
            }
            var instance = GoPool.Spawn(this,
                                        worldPosition,
                                        Quaternion.identity,
                                        this.PreferredParent);
            instance.SetDepth(depth);
            return (instance);
        }

        public void                         SetTargetTransform(Transform tr)
        {
            this._tweenTargetOverride = tr;
            this._targetPosition = this.GetTargetPosition();
        }

        public void                         SetDepth(float depth)
        {
            this._depth = depth;
        }

        public void                         GotoTarget()
        {
            this._toFixedPosition = false;
            this._tweenTargetWorldPositionFrom = this.transform.position;
            this.RefreshTargetPosition();
            this._followStartTime = Time.time;
            this._duration = this._tweenDuration.Rand;
            this._tweening = true;
            //Debug.Log("Transfert GoToTarget FROM=" + this._tweenTargetWorldPositionFrom + ", to = " + this._targetPosition);
            //DebugEx.DrawArrow(this._tweenTargetWorldPositionFrom, this._targetPosition, this._cameraVar.Camera.transform.forward, Color.magenta, .1f, 5);
        }

        public void                         GotoPosition(Vector3 position)
        {
            this._toFixedPosition = true;
            this._tweenTargetWorldPositionFrom = this.transform.position;
            this._targetPosition = position;
            this._followStartTime = Time.time;
            this._duration = this._tweenDuration.Rand;
            this._tweening = true;
            //Debug.Log("Transfert GotoPosition FROM=" + this._tweenTargetWorldPositionFrom + ", to = " + this._targetPosition);
            //DebugEx.DrawArrow(this._tweenTargetWorldPositionFrom, this._targetPosition, this._cameraVar.Camera.transform.forward, Color.magenta, .1f, 5);
        }

        public void                         FinalizeToTargetPosition()
        {
            this.transform.position = this._targetPosition;
        }
        #endregion

        #region Helpers
        void                                RefreshTargetPosition()
        {
            Vector3                         tweenTargetPosition;

            tweenTargetPosition = this.GetTargetPosition();
            this._targetPosition = tweenTargetPosition;
        }

        Vector3                             GetTargetPosition()
        {
            var target = this.TweenTarget;
            if (target is RectTransform)
            {
                if (this._coords == CoordsConversion.CanvasToCameraLocal)
                {
                    return (this._cameraVar.Camera.CanvasToCameraLocalPoint(this._canvasVar.Canvas, target as RectTransform, this._position, this._depth));
                }
                else if (this._coords == CoordsConversion.CanvasToCameraWorld)
                {
                    return (this._cameraVar.Camera.CanvasToCameraWorldPoint(this._canvasVar.Canvas, target as RectTransform, this._position, this._depth));
                }
                else
                {
                    return ((target as RectTransform).GetRectPointWorld(this._position));
                }
            }
            else if (target)
            {
                return (target.position);
            }
            else
            {
                return (this._targetPosition);
            }
        }

        Vector3                             GetFixedTargetPosition()
        {
            if (this.transform is RectTransform)
            {
                if (this._coords == CoordsConversion.CanvasToCameraLocal) //!!! untested but this likely does not work at all
                {
                    return (this._cameraVar.Camera.CanvasToCameraLocalPoint(this._canvasVar.Canvas, this._targetPosition, this._depth));
                }
                else if (this._coords == CoordsConversion.CanvasToCameraWorld)
                {
                    Vector3 localCanvasPos = this._cameraVar.Camera.WorldToCanvasPoint(this._canvasVar.Canvas, this._targetPosition);
                    return (this._canvasVar.Canvas.transform.TransformPoint(localCanvasPos));
                }
            }
            return (this._targetPosition);
        }

        void                                CheckAutoFollow()
        {
            if (Vector3.Distance(this._targetPosition, this.TweenTarget.position) > 0.01f)
            {
                this.GotoTarget();
            }
        }

        void                                ApplyTween()
        {
            if (this._tweening)
            {
                float timeProgress = (Time.time - this._followStartTime) / this._duration;
                if (timeProgress >= 1)
                {
                    //Debug.Log("Transfert Finished lastPos=" + this._targetPosition);
                    this._tweening = false;
                    if (this._toFixedPosition)
                    {
                        this.transform.position = this.GetFixedTargetPosition();
                    }
                    else
                    {
                        this.transform.position = this._targetPosition;
                    }
                    this._onDone.Invoke();
                }
                else
                {
                    float alpha = EasingFunction.GetEasingFunction(this._easing)(timeProgress);
                    if (this._toFixedPosition)
                    {
                        this.transform.position = Vector3.LerpUnclamped(this._tweenTargetWorldPositionFrom, this.GetFixedTargetPosition(), alpha);
                    }
                    else
                    {
                        this.transform.position = Vector3.LerpUnclamped(this._tweenTargetWorldPositionFrom, this._targetPosition, alpha);
                    }
                }
            }
        }
        #endregion

        #region Data Types
        [System.Flags]
        public enum                         Modes
        {
            AutoFollow = 1,
            LiveUpdateTargetPosition = 2
        }
        #endregion
    }
}
