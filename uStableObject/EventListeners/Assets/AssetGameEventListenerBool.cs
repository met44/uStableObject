using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Utilities;

namespace                                       uStableObject
{
    [CreateAssetMenu(menuName = "uStableObject/AssetListener/Bool")]
    public class                                AssetGameEventListenerBool : AssetGameEventListenerBase<bool>
    {
        public GameEventBool                    _event;
        public UnityEventTypes.Bool             _response;
        public UnityEventTypes.Bool             _responseInverted;

        public override GameEventBase<bool>     Event { get { return (this._event); } }
        public override UnityEvent<bool>        Response { get { return (this._response); } }

        public override void                OnEventRaised(bool param)
        {
            if (this._filter == null || this._filter.Value)
            {
                this._response.Invoke(param);
                this._responseInverted.Invoke(!param);
            }
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Assets/Create/uStableObject/AssetListener/As Child - Bool")]
        public static void                      AddTypeAsChild()
        {
            ScriptableUtils.AddAsChild<AssetGameEventListenerBool>("EventListener - ");
        }

        [UnityEditor.MenuItem("Assets/Create/uStableObject/AssetListener/As Child - Bool", true)]
        public static bool                      AddTypeAsChildValidation()
        {
            return (AddAsChildValidation());
        }

        [ContextMenu("Match event name")]
        public void                             MatchEventName()
        {
            base.MatchPrefixAndEventName();
        }
#endif
    }
}
