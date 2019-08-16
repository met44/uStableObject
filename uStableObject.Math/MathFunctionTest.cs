using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace                                       uStableObject.Math
{
    [CreateAssetMenu(menuName = "uStableObject/Math/Test")]
    public class                                MathFunctionTest : ScriptableObject
    {
        #region Input Data
        [SerializeField] FloatMathFunction      _floatMathFunction;
        [SerializeField] float                  _xTestValue;
        #endregion

        #region Triggers
        [ContextMenu("Perform Float Test")]
        public void                             PerformFloatTest()
        {
            float output = this._floatMathFunction.EvalY(this._xTestValue);
            Debug.Log("FloatMathFunction.EvalY(x=" + this._xTestValue + ") = " + output);
        }
        #endregion
    }
}
