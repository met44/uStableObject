using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Utilities;

namespace                               uStableObject
{
    [CreateAssetMenu(menuName = "uStableObject/AssetListener/Int2")]
    public class                        AssetGameEventListenerInt2 : AssetGameEventListenerBase<int, int>
    {
        public GameEventInt2            _event;
        public UnityEventTypes.Int2     _response;

        public override GameEventBase<int, int>  Event { get { return (this._event); } }
        public override UnityEvent<int, int>     Response { get { return (this._response); } }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Assets/Create/uStableObject/AssetListener/As Child - Int2")]
        public static void                      AddTypeAsChild()
        {
            ScriptableUtils.AddAsChild(typeof(AssetGameEventListenerInt2), "EventListener - ");
        }

        [UnityEditor.MenuItem("Assets/Create/uStableObject/AssetListener/As Child - Int2", true)]
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
