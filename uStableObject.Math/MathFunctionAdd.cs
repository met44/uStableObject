using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace                                   uStableObject.Math
{
    [CreateAssetMenu(menuName = "uStableObject/Math/Operators/Add")]
    public class                            MathFunctionAdd : MathFunctionBase
    {
        internal override float             EvalY(float x, MathFunctionShell functionInstance)
        {
            float total = x;
            var shell = functionInstance as AddValuesShell;
            for (var i = 0; i < shell.ChildValues.Count; ++i)
            {
                total += shell.ChildValues[i].EvalY(x);
            }
            return (total);
        }

        public override MathFunctionShell   GetNewShell()
        {
            return (new AddValuesShell(this));
        }

        public override System.Type         GetShellType()
        {
            return (typeof(AddValuesShell));
        }

        #region Data Types
        [System.Serializable]
        public class                        AddValuesShell : MathFunctionShell
        {
            public                          AddValuesShell(MathFunctionBase function) : base(function) { }
        }
        #endregion
    }
}
