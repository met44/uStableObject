using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace                               uStableObject.Tweening
{
    public class                        FollowTransformTween : MonoBehaviour
    {
        #region Input Data
        [SerializeField] Vector3        _startOffset;
        [SerializeField] Vector3        _wobbleOffset;
        [SerializeField] Transform      _tweenTarget;
        [SerializeField] float          _tweenDuration;
        [SerializeField] float          _maxDistance;
        #endregion

        #region Members
        Vector3                         _tweenTargetLocalPosition;
        Vector3                         _tweenTargetLocalRotation;
        Vector3                         _tweenTargetWorldPositionFrom;
        Vector3                         _tweenTargetWorldPositionTo;
        Vector3                         _lastTargetWorldPosition;
        float                           _followStartTime;
        bool                            _tweening;
        #endregion

        #region Unity
        void                            Start()
        {
            this._tweenTargetLocalPosition = this.transform.position - this._tweenTarget.position;
            this._tweenTargetLocalRotation = this.transform.eulerAngles - this._tweenTarget.eulerAngles;
            this._lastTargetWorldPosition = this._tweenTarget.position + this._startOffset;
        }

        void                            Update()
        {
            this.UpdateLastWorldPosition();
            this.ApplyTween();
        }
        #endregion

        #region Helpers
        void                            UpdateLastWorldPosition()
        {
            if (Vector3.Distance(this._lastTargetWorldPosition, this._tweenTarget.position) > 0.01f)
            {
                this._tweenTargetWorldPositionFrom = this._lastTargetWorldPosition + this._tweenTargetLocalPosition;
                this._tweenTargetWorldPositionTo = this._tweenTarget.position + this._tweenTargetLocalPosition;
                if (this._maxDistance > 0)
                {
                    float distance = Vector3.Distance(this._tweenTargetWorldPositionFrom, this._tweenTargetWorldPositionTo);
                    if (distance > this._maxDistance)
                    {
                        this._tweenTargetWorldPositionFrom = Vector3.Lerp(this._tweenTargetWorldPositionTo, this._tweenTargetWorldPositionFrom, this._maxDistance / distance);
                    }
                }
                this._lastTargetWorldPosition = this._tweenTarget.position;
                this._followStartTime = Time.time;
                this._tweening = true;
            }
        }

        void                            ApplyTween()
        {
            if (this._tweening)
            {
                float timeProgress = (Time.time - this._followStartTime) / this._tweenDuration;
                if (timeProgress >= 1)
                {
                    this._tweening = false;
                    this.transform.localPosition = this._tweenTargetLocalPosition;
                    this.transform.localEulerAngles = this._tweenTargetLocalRotation;
                }
                else
                {
                    float alpha = EasingFunction.EaseOutElastic(timeProgress);
                    this.transform.position = Vector3.LerpUnclamped(this._tweenTargetWorldPositionFrom, this._tweenTargetWorldPositionTo, alpha);
                    this.transform.localEulerAngles = Vector3.LerpUnclamped(this._wobbleOffset, this._tweenTargetLocalRotation, alpha);
                }
            }
        }
        #endregion
    }
}
