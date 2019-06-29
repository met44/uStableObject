using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Data;
using uStableObject.Utilities;

namespace                               uStableObject
{
    [CreateAssetMenu(menuName = "uStableObject/AssetListener/Simple Event")]
    public class                        AssetGameEventListener : ScriptableObject, IGameEventListener
    {
        [SerializeField] BoolVar        _filter;

        public GameEvent                _event;
        public UnityEvent               _response;

        [System.NonSerialized] bool     _disabled;

        private void                    OnEnable()
        {
            if (this._event)
            {
                this._event.Unregister(this);
                this._event.Register(this);
            }
        }

        private void                    OnDisable()
        {
            this._event.Unregister(this);
        }

        public void                     OnEventRaised()
        {
            if (this._filter == null || this._filter.Value)
            {
                this._response.Invoke();
            }
        }

        public void                     Enable()
        {
            this.OnEnable();
        }

        public void                     Disable()
        {
            this.OnDisable();
        }

#if UNITY_EDITOR
        public static bool              AddAsChildValidation()
        {
            return (UnityEditor.Selection.activeObject is ScriptableObject);
        }

        [UnityEditor.MenuItem("Assets/Create/uStableObject/AssetListener/As Child - Simple Event")]
        public static void              AddTypeAsChild()
        {
            ScriptableUtils.AddAsChild(typeof(AssetGameEventListener), "EventListener - ");
        }

        [UnityEditor.MenuItem("Assets/Create/uStableObject/AssetListener/As Child - Simple Event", true)]
        public static bool              AddTypeAsChildValidation()
        {
            return (AddAsChildValidation());
        }

        [ContextMenu("Match event name")]
        public void                     MatchEventName()
        {
            this.name = "EventListener" + this._event.name.Substring(this._event.name.IndexOf(" - "));
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
        }

        [ContextMenu("DELETE")]
        public void                             RemoveAsset()
        {
            UnityEditor.AssetDatabase.RemoveObjectFromAsset(UnityEditor.Selection.activeObject);
            UnityEditor.EditorUtility.SetDirty(UnityEditor.Selection.activeObject);
            UnityEditor.AssetDatabase.SaveAssets();
        }

        [ContextMenu("DELETE", true)]
        public bool                             RemoveAssetValidation()
        {
            return (!UnityEditor.AssetDatabase.IsMainAsset(UnityEditor.Selection.activeObject));
        }
#endif
    }
}
