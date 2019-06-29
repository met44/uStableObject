using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Utilities;

namespace                                       uStableObject
{
    [CreateAssetMenu(menuName = "uStableObject/GameEvent/AssetListener/Vector2")]
    public class                                AssetGameEventListenerVector2 : AssetGameEventListenerBase<Vector2>
    {
        public GameEventVector2                 _event;
        public UnityEventTypes.V2               _response;

        public override GameEventBase<Vector2>  Event { get { return (this._event); } }
        public override UnityEvent<Vector2>     Response { get { return (this._response); } }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Assets/Create/uStableObject/GameEvent/AssetListener/As Child - Vector2")]
        public static void                      AddTypeAsChild()
        {
            ScriptableUtils.AddAsChild(typeof(AssetGameEventListenerVector2), "EventListener - ");
        }

        [UnityEditor.MenuItem("Assets/Create/uStableObject/GameEvent/AssetListener/As Child - Vector2", true)]
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
