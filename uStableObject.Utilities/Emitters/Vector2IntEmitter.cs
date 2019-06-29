using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace                                       uStableObject.Utilities.Emitter
{
    public class                                Vector2IntEmitter : MonoBehaviour
    {
        #region Input Data
        [SerializeField] UnityEventTypes.V2Int  _onEmit;
        #endregion

        #region Members
        Vector2Int                              _memorizedVal;
        #endregion

        #region Triggers
        public void                             Emit(Vector2Int val)
        {
            this._onEmit.Invoke(val);
        }

        public void                             EmitMemorized()
        {
            this._onEmit.Invoke(this._memorizedVal);
        }

        public void                             Memorize(Vector2Int val)
        {
            this._memorizedVal = val;
        }
        #endregion
    }
}