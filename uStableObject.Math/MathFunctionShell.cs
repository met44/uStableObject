using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace                                           uStableObject.Math
{
    /// <summary>
    /// This can be extended to store instance specific data rather than just the function tree
    /// </summary>
    [Serializable]
    public class                                    MathFunctionShell
    {
        #region Input Data
        [SerializeField] MathFunctionBase           _function;
        [SerializeField] float                      _floatValue;
        [SerializeField] List<MathFunctionShell>    _childValues;

        #endregion

        #region Properties
        public float                                FloatValue { get { return (this._floatValue); } }
        public List<MathFunctionShell>              ChildValues { get { return (this._childValues); } }
        #endregion

        #region Constructor
        internal                                    MathFunctionShell(MathFunctionBase function)
        {
            this._function = function;
        }
        #endregion

        #region Triggers
        internal float                              EvalY(float x)
        {
            return (this._function.EvalY(x, this));
        }
        #endregion
    }
}
