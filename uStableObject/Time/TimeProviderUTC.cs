using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace                       uStableObject
{
    [CreateAssetMenu(menuName = "uStableObject/TimeProvider/UTC")]
    public class                TimeProviderUTC : TimeProvider
    {
        DateTime                _epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        int                     _lastUpdateFrame;
        float                   _lastUpdateValuef;
        int                     _lastUpdateValue;

        public override int     CurrentTime
        {
            get
            {
                this.UpdateTime();
                return (this._lastUpdateValue);
            }
        }

        public override float   CurrentTimeF
        {
            get
            {
                this.UpdateTime();
                return (this._lastUpdateValuef);
            }
        }

        void                    UpdateTime()
        {
            if (this._lastUpdateFrame != Time.frameCount)
            {
                this._lastUpdateFrame = Time.frameCount;
                double totalSec = (DateTime.UtcNow - this._epochStart).TotalSeconds;
                this._lastUpdateValue = (int)totalSec;
                this._lastUpdateValuef = (float)(totalSec - this._lastUpdateValue);
            }
        }
    }
}
