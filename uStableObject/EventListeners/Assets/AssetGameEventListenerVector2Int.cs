using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Utilities;

namespace                                       uStableObject
{
    [CreateAssetMenu(menuName = "uStableObject/AssetListener/Vector2 Int")]
    public class                                AssetGameEventListenerVector2Int : AssetGameEventListenerBase<Vector2Int>
    {
        public GameEventVector2Int                 _event;
        public UnityEventTypes.V2Int               _response;

        public override GameEventBase<Vector2Int>  Event { get { return (this._event); } }
        public override UnityEvent<Vector2Int>     Response { get { return (this._response); } }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Assets/Create/uStableObject/AssetListener/As Child - Vector2Int")]
        public static void                      AddTypeAsChild()
        {
            ScriptableUtils.AddAsChild(typeof(AssetGameEventListenerVector2Int), "EventListener - ");
        }

        [UnityEditor.MenuItem("Assets/Create/uStableObject/AssetListener/As Child - Vector2Int", true)]
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
