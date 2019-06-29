using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace                       uStableObject.Utilities
{
    public class                AlignWithAxis : MonoBehaviour
    {
        #region Input Data
        [SerializeField] float  _targetAngle;
        [SerializeField] float  _smoothTime;
        #endregion

        #region Members
        float                   _speed;
        Vector3                 _prevPosition;
        bool                    _paused;
        #endregion

        #region Unity
        void                    LateUpdate()
        {
            if (Vector3.Distance(this._prevPosition, this.transform.position) > 0.01f)
            {
                this._prevPosition = this.transform.position;
                this.transform.localRotation = Quaternion.identity;
                this._paused = false;
            }
            else if (!this._paused)
            {
                Vector3 currentAngles = this.transform.eulerAngles;
                if (!Mathf.Approximately(currentAngles.y, this._targetAngle))
                {
                    currentAngles.y = Mathf.SmoothDampAngle(currentAngles.y, this._targetAngle, ref this._speed, this._smoothTime);
                    this.transform.eulerAngles = currentAngles;
                }
                else
                {
                    this._paused = true;
                }
            }

        }
        #endregion
    }
}
