using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace                               uStableObject.Tweening
{
    public class                        FollowTransformTween : MonoBehaviour
    {
        #region Input Data
        [SerializeField] EasingFunction.Ease _easing;
        [SerializeField] Modes          _mode;
        [SerializeField] Vector3        _startOffset;
        [SerializeField] Vector3        _wobbleOffset;
        [SerializeField] Transform      _tweenTarget;
        [SerializeField] float          _tweenDurationMin;
        [SerializeField] float          _tweenDurationMAx;
        [SerializeField] float          _maxDistance;
        [SerializeField] bool           _rotation;
        [SerializeField] UnityEvent     _onDone;
        #endregion

        #region Members
        Vector3                         _tweenTargetWorldPositionFrom;
        Vector3                         _tweenTargetWorldPositionTo;
        Vector3                         _lastWorldPosition;
        float                           _followStartTime;
        float                           _duration;
        bool                            _tweening;
        #endregion

        #region Unity
        void                            Start()
        {
            if (this._tweenTarget)
            {
                this._lastWorldPosition = this._tweenTarget.position + this._startOffset;
            }
        }

        void                            Update()
        {
            if (this._tweenTarget)
            {
                if (this._mode == Modes.Auto)
                {
                    this.UpdateTargetPosition();
                }
                this.ApplyTween();
            }
        }
        #endregion

        #region Triggers
        public void                     SetTargetTransform(Transform tr)
        {
            this._tweenTarget = tr;
            this._lastWorldPosition = this.transform.position + this._startOffset;
        }

        public void                     GotoTarget()
        {
            this._tweenTargetWorldPositionFrom = this._lastWorldPosition;
            this._tweenTargetWorldPositionTo = this._tweenTarget.position;
            if (this._maxDistance > 0)
            {
                float distance = Vector3.Distance(this._tweenTargetWorldPositionFrom, this._tweenTargetWorldPositionTo);
                if (distance > this._maxDistance)
                {
                    this._tweenTargetWorldPositionFrom = Vector3.Lerp(this._tweenTargetWorldPositionTo, this._tweenTargetWorldPositionFrom, this._maxDistance / distance);
                }
            }
            this._lastWorldPosition = this._tweenTarget.position;
            this._followStartTime = Time.time;
            this._duration = Mathf.Lerp(this._tweenDurationMin, this._tweenDurationMAx, Random.value);
            this._tweening = true;
        }
        #endregion

        #region Helpers
        void                            UpdateTargetPosition()
        {
            if (Vector3.Distance(this._lastWorldPosition, this._tweenTarget.position) > 0.01f)
            {
                this.GotoTarget();
            }
        }

        void                            ApplyTween()
        {
            if (this._tweening)
            {
                float timeProgress = (Time.time - this._followStartTime) / this._duration;
                if (timeProgress >= 1)
                {
                    this._tweening = false;
                    this.transform.position = this._tweenTargetWorldPositionTo;
                    if (this._rotation)
                    {
                        this.transform.eulerAngles = this._tweenTarget.eulerAngles;
                    }
                    this._onDone.Invoke();
                }
                else
                {
                    float alpha = EasingFunction.GetEasingFunction(this._easing)(timeProgress);
                    this.transform.position = Vector3.LerpUnclamped(this._tweenTargetWorldPositionFrom, this._tweenTargetWorldPositionTo, alpha);
                    if (this._rotation)
                    {
                        this.transform.eulerAngles = Vector3.LerpUnclamped(this._wobbleOffset, this._tweenTarget.eulerAngles, alpha);
                    }
                }
            }
        }
        #endregion

        #region
        public enum                     Modes
        {
            Auto, Manual
        }
        #endregion
    }
}
