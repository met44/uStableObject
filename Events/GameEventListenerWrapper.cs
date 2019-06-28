using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace                   uStableObject.Data
{
    public class            GameEventListenerWrapper<T> : IGameEventListenerBase<T>, System.IDisposable
    {
        System.Action<T>    _callback;
        GameEventBase<T>    _evnt;

        public static GameEventListenerWrapper<T> Create(GameEventBase<T> evnt, System.Action<T> callback)
        {
            var instance = new GameEventListenerWrapper<T>(evnt, callback);
            return (instance);
        }

        private             GameEventListenerWrapper(GameEventBase<T> evnt, System.Action<T> callback)
        {
            this._callback = callback;
            this._evnt = evnt;
            evnt.Register(this);
        }

        public void         Dispose()
        {
            this._callback = null;
            this._evnt.Unregister(this);
            this._evnt = null;
        }

        public void         OnEventRaised(T param)
        {
            this._callback(param);
        }
    }

    public class            GameEventListenerWrapper : IGameEventListener, System.IDisposable
    {
        System.Action       _callback;
        GameEvent           _evnt;

        public static GameEventListenerWrapper Create(GameEvent evnt, System.Action callback)
        {
            var instance = new GameEventListenerWrapper(evnt, callback);
            return (instance);
        }

        private             GameEventListenerWrapper(GameEvent evnt, System.Action callback)
        {
            this._callback = callback;
            this._evnt = evnt;
            evnt.Register(this);
        }

        public void         Dispose()
        {
            this._callback = null;
            this._evnt.Unregister(this);
            this._evnt = null;
        }

        public void         OnEventRaised()
        {
            this._callback();
        }
    }
}
