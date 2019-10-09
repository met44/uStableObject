using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using uStableObject.Data;

namespace                                       uStableObject
{
    public abstract class                       SchedulerBase<T> : ScriptableObject
    {
        #region Input Data
        [SerializeField] FloatVar               _durationScaler;
        #endregion

        #region Members
        bool                                    _running;
        List<ScheduledTask>                     _scheduledTasksUnique = new List<ScheduledTask>();
        List<ScheduledTask>                     _scheduledTasks = new List<ScheduledTask>();
        #endregion

        #region Properties
        public float                            DurationScale { get { return (this._durationScaler ? this._durationScaler : 1f); } }
        #endregion

        #region Unity
        [RuntimeInitializeOnLoadMethod]
        public void                             OnEnable()
        {
            this.BeginUpdate();
        }

        public void                             OnDisable()
        {
            this._running = false;
        }
        #endregion

        #region Helpers
        async void                              BeginUpdate()
        {
            if (!this._running)
            {
                this._running = true;
                try
                {
                    await this.Update();
                }
                finally
                {
                    this._running = false;
                }
            }
        }

        async Task                              Update()
        {
            ScheduledTask                       sTask;
            TimeSpan                            delay = TimeSpan.FromSeconds(1 / 5f);

            while (this._running)
            {
                try
                {
                    while (this._scheduledTasksUnique.Count > 0
                           && this._scheduledTasksUnique[0]._endTime <= Time.realtimeSinceStartup)
                    {
                        sTask = this._scheduledTasksUnique[0];
                        this._scheduledTasksUnique.RemoveAt(0);
                        sTask._onCompleted?.Invoke(sTask._identifier);
                        sTask._onCompleted = null;
                        sTask._onCanceled = null;
                        AutoPool<ScheduledTask>.Dispose(sTask);
                    }
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
        public void                             CancelUnique(T identifier)
        {
            ScheduledTask                       sTask;

            for (var i = 0; i < this._scheduledTasksUnique.Count; ++i)
            {
                sTask = this._scheduledTasksUnique[i];
                if (sTask._identifier.Equals(identifier))
                {
                    this._scheduledTasksUnique.RemoveAt(i);
                    sTask._onCanceled?.Invoke(identifier);
                    sTask._onCompleted = null;
                    sTask._onCanceled = null;
                    AutoPool<ScheduledTask>.Dispose(sTask);
                    break;
                }
            }
        }

        public void                             CancelAll(T identifier)
        {
            ScheduledTask                       sTask;

            for (var i = this._scheduledTasks.Count; i >= 0; --i)
            {
                sTask = this._scheduledTasks[i];
                if (sTask._identifier.Equals(identifier))
                {
                    this._scheduledTasks.RemoveAt(i);
                    sTask._onCanceled?.Invoke(identifier);
                    sTask._onCompleted = null;
                    sTask._onCanceled = null;
                    AutoPool<ScheduledTask>.Dispose(sTask);
                }
            }
        }

        public void                             ScheduleUnique(float duration, 
                                                               T identifier, 
                                                               Action<T> onCompleted,
                                                               Action<T> onCanceled)
        {
            ScheduledTask                       instance = null;

            for (var i = 0; i < this._scheduledTasksUnique.Count; ++i)
            {
                if (this._scheduledTasksUnique[i]._identifier.Equals(identifier))
                {
                    instance = this._scheduledTasksUnique[i];
                    this._scheduledTasksUnique.RemoveAt(i);
                    instance._onCanceled?.Invoke(identifier);
                    break;
                }
            }
            if (instance == null)
            {
                instance = AutoPool<ScheduledTask>.Create();
                instance._identifier = identifier;
            }
            instance._endTime = this.CalcEndTime(duration);
            instance._onCompleted = onCompleted;
            instance._onCanceled = onCanceled;
            for (var i = 0; i < this._scheduledTasksUnique.Count; ++i)
            {
                if (this._scheduledTasksUnique[i]._endTime >= instance._endTime)
                {
                    this._scheduledTasksUnique.Insert(i, instance);
                    return;
                }
            }
            this._scheduledTasksUnique.Add(instance);
        }

        public void                             Schedule(float duration, 
                                                         T identifier, 
                                                         Action<T> onCompleted,
                                                         Action<T> onCanceled)
        {
            ScheduledTask                       instance = null;

            instance = AutoPool<ScheduledTask>.Create();
            instance._identifier = identifier;
            instance._endTime = this.CalcEndTime(duration);
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

        #region Helpers
        protected virtual float             CalcEndTime(float duration)
        {
            return (Time.realtimeSinceStartup + duration * this.DurationScale);
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
