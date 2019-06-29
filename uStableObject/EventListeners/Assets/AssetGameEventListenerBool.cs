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

        public override GameEventBase<bool>     Event { get { return (this._event); } }
        public override UnityEvent<bool>        Response { get { return (this._response); } }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Assets/Create/uStableObject/AssetListener/As Child - Bool")]
        public static void                      AddTypeAsChild()
        {
            ScriptableUtils.AddAsChild(typeof(AssetGameEventListenerBool), "EventListener - ");
        }

        [UnityEditor.MenuItem("Assets/Create/uStableObject/AssetListener/As Child - Bool", true)]
        public static bool                      AddTypeAsChildValidation()
        {
            return (AddAsChildValidation());
        }

        [ContextMenu("Match event name")]
        public override void                    MatchEventName()
        {
            base.MatchEventName();
        }
#endif
    }
}
