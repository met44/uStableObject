using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace                                   uStableObject.Math
{
    [CreateAssetMenu(menuName = "uStableObject/Math/Values/Constant")]
    public class                            MathFunctionConstant : MathFunctionBase
    {
        internal override float             EvalY(float x, MathFunctionShell functionInstance)
        {
            return ((functionInstance as ConstantValueShell).FloatValue);
        }

        public override MathFunctionShell   GetNewShell()
        {
            return (new ConstantValueShell(this));
        }

        public override System.Type         GetShellType()
        {
            return (typeof(ConstantValueShell));
        }

        #region Data Types
        [System.Serializable]
        public class                        ConstantValueShell : MathFunctionShell
        {
            public                          ConstantValueShell(MathFunctionBase function) : base(function) { }
        }
        #endregion
    }
}
