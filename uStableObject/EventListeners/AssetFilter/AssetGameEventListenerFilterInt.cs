using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Utilities;
using uStableObject.Data;

namespace                                       uStableObject
{
    [CreateAssetMenu(menuName = "uStableObject/AssetListenerFilter/Int")]
    public class                                AssetGameEventListenerFilterInt : AssetGameEventListenerBase<int>
    {
        [SerializeField] FilterOps              _filterOp;
        [SerializeField] IntVar                 _filterValue;
        [SerializeField] GameEventInt           _event;
        [SerializeField] UnityEventTypes.Int    _response;

        public override GameEventBase<int>      Event { get { return (this._event); } }
        public override UnityEvent<int>         Response { get { return (this._response); } }

        public override void                    OnEventRaised(int param)
        {
            if (this._filter == null || this._filter.Value)
            {
                if (this.CheckValueFilter(param))
                {
                    {
                        this.Response.Invoke(param);
                    }
                }
            }
        }

        bool                                    CheckValueFilter(int val)
        {
            if (val == this._filterValue)
            {
                return (this._filterOp >= FilterOps.InferiorOrEqualTo && this._filterOp <= FilterOps.SuperiorOrEqualTo);
            }
            else if (val < this._filterValue)
            {
                return (this._filterOp <= FilterOps.InferiorOrEqualTo);
            }
            else
            {
                return (this._filterOp >= FilterOps.InferiorOrEqualTo);
            }
        }

        #region Data Types
        public enum                             FilterOps
        {
            InferiorTo, InferiorOrEqualTo, EqualTo, SuperiorOrEqualTo, SuperiorTO
        }
        #endregion

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Assets/Create/uStableObject/AssetListenerFilter/As Child - Int")]
        public static void                      AddTypeAsChild()
        {
            ScriptableUtils.AddAsChild<AssetGameEventListenerFilterInt>("EventListenerFilter - ");
        }

        [UnityEditor.MenuItem("Assets/Create/uStableObject/AssetListenerFilter/As Child - Int", true)]
        public static bool                      AddTypeAsChildValidation()
        {
            return (AddAsChildValidation());
        }

        [ContextMenu("Match event name")]
        public void                             MatchEventName()
        {
            base.MatchPrefixAndEventName("EventListenerFilter");
        }
#endif
    }
}
