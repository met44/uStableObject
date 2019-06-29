using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace                                       uStableObject.Utilities
{
    public class                                FloatEmitter : MonoBehaviour
    {
        #region Input Data
        [SerializeField] UnityEventTypes.Float  _onEmit;
        [SerializeField] float                  _memorizedVal;
        #endregion

        #region Triggers
        public void                             Emit(float val)
        {
            this._onEmit.Invoke(val);
        }

        public void                             EmitMemorized()
        {
            this._onEmit.Invoke(this._memorizedVal);
        }

        public void                             Memorize(float val)
        {
            this._memorizedVal = val;
        }
        #endregion
    }
}