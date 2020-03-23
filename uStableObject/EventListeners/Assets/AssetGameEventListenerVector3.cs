using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Utilities;

namespace                                       uStableObject
{
    [CreateAssetMenu(menuName = "uStableObject/AssetListener/Vector3")]
    public class                                AssetGameEventListenerVector3 : AssetGameEventListenerBase<Vector3>
    {
        public GameEventVector3                 _event;
        public UnityEventTypes.V3               _response;

        public override GameEventBase<Vector3>  Event { get { return (this._event); } }
        public override UnityEvent<Vector3>     Response { get { return (this._response); } }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Assets/Create/uStableObject/AssetListener/As Child - Vector3")]
        public static void                      AddTypeAsChild()
        {
            ScriptableUtils.AddAsChild<AssetGameEventListenerVector3>("EventListener - ");
        }

        [UnityEditor.MenuItem("Assets/Create/uStableObject/AssetListener/As Child - Vector3", true)]
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
