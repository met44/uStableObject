using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace                                       uStableObject.Utilities
{
    public class                                BoolSplitEmitter : MonoBehaviour
    {
        #region Input Data
        [SerializeField] UnityEvent             _onEmitTrue;
        [SerializeField] UnityEvent             _onEmitFalse;
        [SerializeField] bool                   _memorizedVal;
        #endregion

        #region Members
        #endregion

        #region Triggers
        public void                             Emit(bool val)
        {
            (val ? this._onEmitTrue : this._onEmitFalse).Invoke();
        }

        public void                             EmitMemorized()
        {
            (this._memorizedVal ? this._onEmitTrue : this._onEmitFalse).Invoke();
        }

        public void                             Memorize(bool val)
        {
            this._memorizedVal = val;
        }
        #endregion
    }
}