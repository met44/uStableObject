using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace                       uStableObject.Utilities
{
    [System.Serializable]
    public class                IntervalF
    {
        #region Input Data
        [SerializeField] float  _from;
        [SerializeField] float  _to;
        #endregion

        #region Properties
        public float            From    { get { return (this._from); }  set { this._from = value; } }
        public float            To      { get { return (this._to); }    set { this._to = value; } }
        public float            Rand    { get { return (Random.Range(this._from, this._to)); } }
        #endregion

        #region Constructors
        public                  IntervalF() { }
        public                  IntervalF(int from, int to) { this._from = from; this._to = to; }
        #endregion
    }
}
