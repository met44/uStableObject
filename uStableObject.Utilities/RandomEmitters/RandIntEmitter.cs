using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace                                       uStableObject.Utilities
{
    public class                                RandIntEmitter : IntEmitter
    {
        #region Input Data
        [SerializeField] int                    _randMin;
        [SerializeField] int                    _randMax;
        #endregion

        #region Members
        #endregion

        #region Triggers
        public void                             EmitRand()
        {
            this.Emit(Random.Range(this._randMin, this._randMax));
        }
        #endregion
    }
}