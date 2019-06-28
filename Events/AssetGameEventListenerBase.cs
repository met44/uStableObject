using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Data;

namespace                                   uStableObject
{
    public abstract class                   AssetGameEventListenerBase<T> : ScriptableObject, IGameEventListenerBase<T>
    {
        [SerializeField] protected BoolVar  _filter;

        public abstract GameEventBase<T>    Event { get; }
        public abstract UnityEvent<T>       Response { get; }

        protected virtual void              OnEnable()
        {
            this.Event.Unregister(this);
            this.Event.Register(this);
        }

        private void                        OnDisable()
        {
            this.Event.Unregister(this);
        }

        public virtual void                 OnEventRaised(T param)
        {
            if (this._filter == null || this._filter.Value)
            {
                this.Response.Invoke(param);
            }
        }

        public void                         Enable()
        {
            this.OnEnable();
        }

        public void                         Disable()
        {
            this.OnDisable();
        }

#if UNITY_EDITOR
        public static bool                  AddAsChildValidation()
        {
            return (UnityEditor.Selection.activeObject is ScriptableObject);
        }

        public virtual void                 MatchEventName()
        {
            this.name = "EventListener" + this.Event.name.Substring(this.Event.name.IndexOf(" - "));
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
        }

        [ContextMenu("DELETE")]
        public virtual void                 RemoveAsset()
        {
            UnityEditor.AssetDatabase.RemoveObjectFromAsset(UnityEditor.Selection.activeObject);
            UnityEditor.EditorUtility.SetDirty(UnityEditor.Selection.activeObject);
            UnityEditor.AssetDatabase.SaveAssets();
        }

        [ContextMenu("DELETE", true)]
        public virtual bool                 RemoveAssetValidation()
        {
            return (!UnityEditor.AssetDatabase.IsMainAsset(UnityEditor.Selection.activeObject));
        }
#endif
    }

    public abstract class                   AssetGameEventListenerBase<T1, T2> : ScriptableObject, IGameEventListenerBase2<T1, T2>
    {
        [SerializeField] BoolVar _filter;

        public abstract GameEventBase<T1, T2>   Event { get; }
        public abstract UnityEvent<T1, T2>      Response { get; }

        private void                        OnEnable()
        {
            this.Event.Unregister(this);
            this.Event.Register(this);
        }

        private void                        OnDisable()
        {
            this.Event.Unregister(this);
        }

        public void                         OnEventRaised(T1 param1, T2 param2)
        {
            if (this._filter == null || this._filter.Value)
            {
                this.Response.Invoke(param1, param2);
            }
        }

        public void                         Enable()
        {
            this.OnEnable();
        }

        public void                         Disable()
        {
            this.OnDisable();
        }

#if UNITY_EDITOR
        public static bool                  AddAsChildValidation()
        {
            return (UnityEditor.Selection.activeObject is ScriptableObject);
        }

        public virtual void                 MatchEventName()
        {
            this.name = "EventListener" + this.Event.name.Substring(this.Event.name.IndexOf(" - "));
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
        }

        [ContextMenu("DELETE")]
        public void                         RemoveAsset()
        {
            UnityEditor.AssetDatabase.RemoveObjectFromAsset(UnityEditor.Selection.activeObject);
            UnityEditor.EditorUtility.SetDirty(UnityEditor.Selection.activeObject);
            UnityEditor.AssetDatabase.SaveAssets();
        }

        [ContextMenu("DELETE", true)]
        public bool                         RemoveAssetValidation()
        {
            return (!UnityEditor.AssetDatabase.IsMainAsset(UnityEditor.Selection.activeObject));
        }
#endif
    }
}
