using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace                                   uStableObject.Math
{
    //[CreateAssetMenu("uStableObject/Math/")]
    public abstract class                   MathFunctionBase : ScriptableObject
    {
        internal abstract float             EvalY(float x, MathFunctionShell functionInstance);

        public abstract MathFunctionShell   GetNewShell();
        public abstract System.Type         GetShellType();
    }
}
