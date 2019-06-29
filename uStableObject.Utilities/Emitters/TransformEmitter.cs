using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace                                       uStableObject.Utilities
{
    public class                                TransformEmitter : MonoBehaviour
    {
        #region Input Data
        [SerializeField] UnityEventTypes.TR     _onEmit;
        [SerializeField] Transform              _memorizedVal;
        #endregion

        #region Triggers
        public void                             Emit(Transform val)
        {
            this._onEmit.Invoke(val);
        }

        public void                             EmitMemorized()
        {
            this._onEmit.Invoke(this._memorizedVal);
        }

        public void                             Memorize(Transform val)
        {
            this._memorizedVal = val;
        }
        #endregion
    }
}