using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace                                   uStableObject.Math
{
    [CreateAssetMenu(menuName = "uStableObject/Math/Values/Linear")]
    public class                            MathFunctionLinear : MathFunctionBase
    {
        internal override float             EvalY(float x, MathFunctionShell functionInstance)
        {
            return (x * (functionInstance as LinearValueShell).FloatValue);
        }

        public override MathFunctionShell   GetNewShell()
        {
            return (new LinearValueShell(this));
        }

        public override System.Type         GetShellType()
        {
            return (typeof(LinearValueShell));
        }

        #region Data Types
        [System.Serializable]
        public class                        LinearValueShell : MathFunctionShell
        {
            public                          LinearValueShell(MathFunctionBase function) : base(function) { }
        }
        #endregion
    }
}
