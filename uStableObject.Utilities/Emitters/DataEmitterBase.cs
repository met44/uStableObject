using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace                                       uStableObject.Utilities
{
    public class                                DataEmitterBase<T, E> : MonoBehaviour where E : UnityEvent<T>
    {
        #region Input Data
        [SerializeField] E                      _onEmit;
        [SerializeField] T                      _memorizedVal;
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
            this._onEmit.Invoke(this._memorizedVal);
        }

        public void                             Memorize(T val)
        {
            this._memorizedVal = val;
        }
        #endregion
    }
}