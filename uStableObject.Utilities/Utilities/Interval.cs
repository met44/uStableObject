using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace                       uStableObject.Utilities
{
    [System.Serializable]
    public class                Interval
    {
        #region Input Data
        [SerializeField] int    _from;
        [SerializeField] int    _to;
        #endregion

        #region Properties
        public int              From    { get { return (this._from); }  set { this._from = value; } }
        public int              To      { get { return (this._to); }    set { this._to = value; } }
        public int              Rand    { get { return (Random.Range(this._from, this._to)); } }
        #endregion

        #region Helpers
        public bool             Contains(int value)
        {
            return (value >= this.From && value <= this.To);
        }
        #endregion

        #region Constructors
        public                  Interval() { }
        public                  Interval(int from, int to) { this._from = from; this._to = to; }
        #endregion
    }
}
