using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace                                   uStableObject
{
    public abstract class                   GameEventBase<T> : ScriptableObject
    {
        [SerializeField] bool               _logListeners;
        List<IGameEventListener<T>>         _listeners = new List<IGameEventListener<T>>();
        List<IGameEventListener<T>>         _listenersTemp = new List<IGameEventListener<T>>();
        bool                                _running;
        bool                                _changed;

        public int                          ListenersCount { get => _listenersTemp.Count; }

        public void                         Raise(T param)
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
                                if (this._listenersTemp[i] is UnityEngine.Object)
                                {
                                    Debug.Log("[" + this.name + "][" + param + "] Event Listener: " + (this._listenersTemp[i] as UnityEngine.Object).name, this._listeners[i] as UnityEngine.Object);
                                }
                                else
                                {
                                    Debug.Log("[" + this.name + "][" + param + "] Event Listener: " + this._listenersTemp[i]);
                                }
                            }
                            this._listenersTemp[i].OnEventRaised(param);
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

        public virtual void                 Register(IGameEventListener<T> gameEventListener)
        {
            this._changed = true;
            this._listeners.Add(gameEventListener);
        }

        public virtual void                 Unregister(IGameEventListener<T> gameEventListener)
        {
            this._changed = true;
            this._listeners.Remove(gameEventListener);
        }
    }

    public abstract class                   GameEventBase<T1, T2> : ScriptableObject
    {
        [SerializeField] bool               _logListeners;
        List<IGameEventListener<T1, T2>>    _listeners = new List<IGameEventListener<T1, T2>>();
        List<IGameEventListener<T1, T2>>    _listenersTemp = new List<IGameEventListener<T1, T2>>();
        bool                                _running;
        bool                                _changed;
        
        public int                          ListenersCount { get => _listenersTemp.Count; }

        public void                         Raise(T1 param1, T2 param2)
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
                                if (this._listenersTemp[i] is UnityEngine.Object)
                                {
                                    Debug.Log("[" + this.name + "][" + param1 + "][" + param2 + "] Event Listener: " + (this._listenersTemp[i] as UnityEngine.Object).name);
                                }
                                else
                                {
                                    Debug.Log("[" + this.name + "][" + param1 + "][" + param2 + "] Event Listener: " + this._listenersTemp[i]);
                                }
                            }
                            this._listenersTemp[i].OnEventRaised(param1, param2);
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

        public virtual void                 Register(IGameEventListener<T1, T2> gameEventListener)
        {
            this._changed = true;
            this._listeners.Add(gameEventListener);
        }

        public virtual void                 Unregister(IGameEventListener<T1, T2> gameEventListener)
        {
            this._changed = true;
            this._listeners.Remove(gameEventListener);
        }
    }
}
