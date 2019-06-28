using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace                                       uStableObject
{
    public abstract class                       SchedulerBase<T> : ScriptableObject
    {
        #region Input Data
        #endregion

        #region Members
        bool                                    _running;
        List<ScheduledTask>                     _scheduledTasks = new List<ScheduledTask>();
        #endregion

        #region Unity
        [RuntimeInitializeOnLoadMethod]
        public void                             OnEnable()
        {
            this._running = false;
            this.BeginUpdate();
        }
        #endregion

        #region Helpers
        async void                              BeginUpdate()
        {
            if (!this._running)
            {
                this._running = true;
                await this.Update();
                this._running = false;
            }
        }

        async Task                              Update()
        {
            ScheduledTask                       sTask;
            TimeSpan                            delay = TimeSpan.FromSeconds(1 / 5f);

            while (Application.isPlaying)
            {
                try
                {
                    while (this._scheduledTasks.Count > 0
                           && this._scheduledTasks[0]._endTime <= Time.realtimeSinceStartup)
                    {
                        sTask = this._scheduledTasks[0];
                        this._scheduledTasks.RemoveAt(0);
                        sTask._onCompleted?.Invoke(sTask._identifier);
                        sTask._onCompleted = null;
                        sTask._onCanceled = null;
                        AutoPool<ScheduledTask>.Dispose(sTask);
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
                await Task.Delay(delay);
            }
        }
        #endregion

        #region Triggers
        public void                             Cancel(T identifier)
        {
            ScheduledTask                       sTask;

            for (var i = 0; i < this._scheduledTasks.Count; ++i)
            {
                sTask = this._scheduledTasks[i];
                if (sTask._identifier.Equals(identifier))
                {
                    this._scheduledTasks.RemoveAt(i);
                    sTask._onCanceled?.Invoke(identifier);
                    sTask._onCompleted = null;
                    sTask._onCanceled = null;
                    AutoPool<ScheduledTask>.Dispose(sTask);
                    break;
                }
            }
        }

        public void                             Schedule(float duration, 
                                                         T identifier, 
                                                         Action<T> onCompleted,
                                                         Action<T> onCanceled)
        {
            ScheduledTask                       instance = null;

            for (var i = 0; i < this._scheduledTasks.Count; ++i)
            {
                if (this._scheduledTasks[i]._identifier.Equals(identifier))
                {
                    instance = this._scheduledTasks[i];
                    this._scheduledTasks.RemoveAt(i);
                    instance._onCanceled?.Invoke(identifier);
                    break;
                }
            }
            if (instance == null)
            {
                instance = AutoPool<ScheduledTask>.Create();
                instance._identifier = identifier;
            }
            instance._endTime = Time.realtimeSinceStartup + duration;
            instance._onCompleted = onCompleted;
            instance._onCanceled = onCanceled;
            for (var i = 0; i < this._scheduledTasks.Count; ++i)
            {
                if (this._scheduledTasks[i]._endTime >= instance._endTime)
                {
                    this._scheduledTasks.Insert(i, instance);
                    return;
                }
            }
            this._scheduledTasks.Add(instance);
        }
        #endregion

        #region Data Types
        [Serializable]
        public class                            ScheduledTask
        {
            public T                            _identifier;
            public float                        _endTime;
            public Action<T>                    _onCompleted;
            public Action<T>                    _onCanceled;
        }
        #endregion
    }
}
