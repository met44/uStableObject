using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace                       uStableObject
{
    [CreateAssetMenu(menuName = "uStableObject/TimeProvider/Unity Real Time")]
    public class                TimeProviderUnityRealTime : TimeProvider
    {
        public override uint     CurrentTime
        {
            get
            {
                return ((uint)Time.realtimeSinceStartup);
            }
        }

        public override float   CurrentTimeF
        {
            get
            {
                return (Time.realtimeSinceStartup - (int)Time.realtimeSinceStartup);
            }
        }
    }
}
