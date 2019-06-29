using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace                                       uStableObject.Utilities
{
    public class                                BoolEmitter : MonoBehaviour
    {
        #region Input Data
        [SerializeField] UnityEventTypes.Bool   _onEmit;
        [SerializeField] bool                   _memorizedVal;
        #endregion

        #region Triggers
        public void                             Emit(bool val)
        {
            this._onEmit.Invoke(val);
        }

        public void                             EmitMemorized()
        {
            this._onEmit.Invoke(this._memorizedVal);
        }

        public void                             Memorize(bool val)
        {
            this._memorizedVal = val;
        }
        #endregion
    }
}