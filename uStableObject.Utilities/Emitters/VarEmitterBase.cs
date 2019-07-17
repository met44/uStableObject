using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Data;

namespace                                       uStableObject.Utilities
{
    public class                                VarEmitterBase<T, E, V> : MonoBehaviour 
                                                where E : UnityEvent<T> 
                                                where V : IBaseTypeVar<T>
    {
        #region Input Data
        [SerializeField] E                      _onEmit;
        [SerializeField] V                      _var;
        #endregion

        #region Members
        #endregion

        #region Triggers
        public void                             Emit(T val)
        {
            this._onEmit.Invoke(val);
        }

        public void                             EmitMemorized()
        {
            this._onEmit.Invoke(this._var.Value);
        }

        public void                             SetVar(V val)
        {
            this._var = val;
        }
        #endregion
    }
}