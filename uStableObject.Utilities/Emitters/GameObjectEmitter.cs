using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace                                       uStableObject.Utilities
{
    public class                                GameObjectEmitter : MonoBehaviour
    {
        #region Input Data
        [SerializeField] UnityEventTypes.GO     _onEmit;
        [SerializeField] GameObject             _memorizedVal;
        #endregion

        #region Triggers
        public void                             Emit(GameObject val)
        {
            this._onEmit.Invoke(val);
        }

        public void                             EmitMemorized()
        {
            this._onEmit.Invoke(this._memorizedVal);
        }

        public void                             Memorize(GameObject val)
        {
            this._memorizedVal = val;
        }
        #endregion
    }
}