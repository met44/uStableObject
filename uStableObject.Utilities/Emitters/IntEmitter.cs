using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace                                       uStableObject.Utilities
{
    public class                                IntEmitter : MonoBehaviour
    {
        #region Input Data
        [SerializeField] UnityEventTypes.Int   _onEmit;
        [SerializeField] int                    _memorizedVal;
        #endregion

        #region Triggers
        public void                             Emit(int val)
        {
            this._onEmit.Invoke(val);
        }

        public void                             EmitMemorized()
        {
            this._onEmit.Invoke(this._memorizedVal);
        }

        public void                             Memorize(int val)
        {
            this._memorizedVal = val;
        }
        #endregion
    }
}