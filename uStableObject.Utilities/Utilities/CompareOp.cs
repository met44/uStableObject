using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Utilities;
using uStableObject.Data;

namespace                                       uStableObject
{
    [System.Serializable]
    public class                                CompareOp
    {
        #region Input Data
        [SerializeField] FilterOps              _filterOp;
        #endregion

        #region Properties
        public FilterOps                        FilterOp { get { return (this._filterOp); } }
        #endregion

        #region Trigger
        public bool                             Matches<T>(T val1, T val2) where T : System.IComparable
        {
            int comp = val1.CompareTo(val2);
            if (comp == 0)
            {
                return (this._filterOp >= FilterOps.InferiorOrEqualTo && this._filterOp <= FilterOps.SuperiorOrEqualTo);
            }
            else if (comp < 0)
            {
                return (this._filterOp <= FilterOps.InferiorOrEqualTo);
            }
            else
            {
                return (this._filterOp >= FilterOps.SuperiorOrEqualTo);
            }
        }
        #endregion

        #region Data Types
        public enum                             FilterOps
        {
            InferiorTo, InferiorOrEqualTo, EqualTo, SuperiorOrEqualTo, SuperiorTO
        }
        #endregion
    }
}
