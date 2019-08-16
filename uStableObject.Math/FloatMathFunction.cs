using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace                                   uStableObject.Math
{
    [System.Serializable]
    public class                            FloatMathFunction
    {
        #region Input Data
        [SerializeField] MathFunctionShell  _functionInstance;
        #endregion

        #region Triggers
        public float                        EvalY(float x)
        {
            return (this._functionInstance.EvalY(x));
        }
        #endregion
    }
}
