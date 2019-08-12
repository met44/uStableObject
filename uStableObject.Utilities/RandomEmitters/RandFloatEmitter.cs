using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace                                       uStableObject.Utilities
{
    public class                                RandFloatEmitter : FloatEmitter
    {
        #region Input Data
        [SerializeField] float                  _randMin;
        [SerializeField] float                  _randMax;
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