using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace                           uStableObject
{
    [CreateAssetMenu(menuName = "uStableObject/GameEvent/Game Event Simple", order = 5)]
    public class                    GameEvent : ScriptableObject
    {
        #region Input Data
        [SerializeField] bool       _logListeners = false;
        #endregion

        #region Members
        List<IGameEventListener>    _listeners = new List<IGameEventListener>();
        List<IGameEventListener>    _listenersTemp = new List<IGameEventListener>();
        bool                        _running;
        bool                        _changed;
        #endregion
        
        #region Triggers
        [ContextMenu("Raise event")]
        public void                 Raise()
        {
            if (!this._running)
            {
                try
                {
                    this._running = true;
                    if (this._changed)
                    {
                        this._changed = false;
                        this._listenersTemp.Clear();
                        for (var i = 0; i < this._listeners.Count; ++i)
                        {
                            this._listenersTemp.Add(this._listeners[i]);
                        }
                    }
                    for (var i = 0; i < this._listenersTemp.Count; ++i)
                    {
                        try
                        {
                            if (this._logListeners)
                            {
                                if (this._listenersTemp[i] is Object)
                                {
                                    Debug.Log("[" + this.name + "] Event Listener: " + (this._listenersTemp[i] as UnityEngine.Object).name);
                                }
                                else
                                {
                                    Debug.Log("[" + this.name + "] Event Listener: " + this._listenersTemp[i]);
                                }
                            }
                            this._listenersTemp[i].OnEventRaised();
                        }
                        catch (System.Exception ex)
                        {
                            Debug.LogException(ex);
                        }
                    }
                }
                finally
                {
                    this._running = false;
                }
            }
            else
            {
                Debug.LogError("Event Loop: " + this.name);
            }
        }

        public virtual void         Register(IGameEventListener gameEventListener)
        {
            this._changed = true;
            this._listeners.Add(gameEventListener);
        }

        public virtual void         Unregister(IGameEventListener gameEventListener)
        {
            this._changed = true;
            this._listeners.Remove(gameEventListener);
        }
        #endregion

#if UNITY_EDITOR
        public static class         Editor
        {
            public static void      ShowInspector(GameEvent instance)
            {
                if (instance._listeners != null)
                {
                    UnityEditor.EditorGUILayout.Separator();
                    UnityEditor.EditorGUILayout.LabelField("Listeners: ");
                    for (var i = 0; i < instance._listeners.Count; ++i)
                    {
                        var listener = instance._listeners[i];
                        if (listener is Object)
                        {
                            Object obj = listener as Object;
                            UnityEditor.EditorGUILayout.ObjectField("[" + i + "] " + listener.GetType().Name, obj, obj.GetType(), false);
                        }
                        else
                        {
                            UnityEditor.EditorGUILayout.LabelField("[" + i + "] " + listener.GetType().Name);
                        }
                    }
                }
            }
        }
#endif
    }
}
