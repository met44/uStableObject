using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace                                       uStableObject.Utilities
{
    public class                                StringEmitter : MonoBehaviour
    {
        #region Input Data
        [SerializeField] UnityEventTypes.String _onEmit;
        [SerializeField] string                 _memorizedVal;
        #endregion

        #region Triggers
        public void                             Emit(string val)
        {
            this._onEmit.Invoke(val);
        }

        public void                             EmitMemorized()
        {
            this._onEmit.Invoke(this._memorizedVal);
        }

        public void                             Memorize(string val)
        {
            this._memorizedVal = val;
        }
        #endregion
    }
}