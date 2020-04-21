using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace                       uStableObject
{
    public abstract class       TimeProvider : ScriptableObject
    {
        public abstract uint    CurrentTime { get; }
        public abstract float   CurrentTimeF { get; }
    }
}
