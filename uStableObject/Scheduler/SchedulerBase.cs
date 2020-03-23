using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using uStableObject.Data;

namespace                                       uStableObject
{
    public abstract partial class               SchedulerBase<T> : ScriptableObject
    {
        #region Input Data
        [SerializeField] FloatVar               _durationScaler;
        [SerializeField] TimeProvider           _timeProvider;
        #endregion

        #region Members
        protected bool                          _running;
        protected bool                          _paused;
        protected List<ScheduledTask>           _scheduledTasksUnique = new List<ScheduledTask>();
        protected List<ScheduledTask>           _scheduledTasks = new List<ScheduledTask>();
        #endregion

        #region Properties
        public float                            DurationScale { get { return (this._durationScaler ? this._durationScaler : 1f); } }
        public TimeProvider                     TimeProvider { get => _timeProvider; }
        #endregion

        #region Unity
        [RuntimeInitializeOnLoadMethod]
        void                                    OnEnable()
        {
            this.BeginUpdate();
        }

        void                                    OnDisable()
        {
            this._running = false;
        }
        #endregion

        #region Triggers
        /// <summary>
        /// For non asset instances, this allows to start the scheduler while ensuring it has a time provider. (Asset instances should wire the time provider in editor)
        /// </summary>
        /// <param name="timeProvider"></param>
        public void                             Init(TimeProvider timeProvider)
        {
            this._timeProvider = timeProvider;
            this.BeginUpdate();
        }

        public void                             Shutdown()
        {
            ScheduledTask                       sTask;

            for (var i = 0; i < this._scheduledTasksUnique.Count; ++i)
            {
                sTask = this._scheduledTasksUnique[i];
                sTask._onCompleted = null;
                sTask._onCanceled = null;
                AutoPool<ScheduledTask>.Dispose(sTask);
            }
            for (var i = 0; i < this._scheduledTasks.Count; ++i)
            {
                sTask = this._scheduledTasks[i];
                sTask._onCompleted = null;
                sTask._onCanceled = null;
                AutoPool<ScheduledTask>.Dispose(sTask);
            }
            this._scheduledTasksUnique.Clear();
            this._scheduledTasks.Clear();
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
                if (!this._paused)
                {
                    try
                    {
                        while (this._scheduledTasksUnique.Count > 0
                               && this._scheduledTasksUnique[0]._endTime <= this._timeProvider.CurrentTime)
                        {
                            sTask = this._scheduledTasksUnique[0];
                            this._scheduledTasksUnique.RemoveAt(0);
                            sTask._onCompleted?.Invoke(sTask._identifier);
                            sTask._onCompleted = null;
                            sTask._onCanceled = null;
                            AutoPool<ScheduledTask>.Dispose(sTask);
                        }
                        while (this._scheduledTasks.Count > 0
                               && this._scheduledTasks[0]._endTime <= this._timeProvider.CurrentTime)
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
                }
                await Task.Delay(delay);

#if UNITY_EDITOR
                if (!Application.isPlaying) break;
#endif
            }
        }
#endregion

        #region Triggers
        public void                             TogglePause(bool paused)
        {
            this._paused = paused;
        }

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

        public void                             CancelAll(T identifier, int max = int.MaxValue)
        {
            ScheduledTask                       sTask;

            for (var i = this._scheduledTasks.Count - 1; i >= 0; --i)
            {
                sTask = this._scheduledTasks[i];
                if (sTask._identifier.Equals(identifier))
                {
                    this._scheduledTasks.RemoveAt(i);
                    sTask._onCanceled?.Invoke(identifier);
                    sTask._onCompleted = null;
                    sTask._onCanceled = null;
                    AutoPool<ScheduledTask>.Dispose(sTask);
                    if (--max <= 0)
                    {
                        break;
                    }
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
            instance._startTime = this.TimeProvider.CurrentTime;
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

        public void                             DirtyUnique(float newDuration, T identifier)
        {
            ScheduledTask                       instance = null;

            for (var i = 0; i < this._scheduledTasksUnique.Count; ++i)
            {
                if (this._scheduledTasksUnique[i]._identifier.Equals(identifier))
                {
                    instance = this._scheduledTasksUnique[i];
                    this._scheduledTasksUnique.RemoveAt(i);
                    break;
                }
            }
            if (instance == null)
            {
                return;
            }
            instance._endTime = this.CalcEndTime(instance._startTime, newDuration);
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
            instance._startTime = this.TimeProvider.CurrentTime;
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
        protected virtual int                   CalcEndTime(float duration)
        {
            return (this._timeProvider.CurrentTime + Mathf.CeilToInt(duration * this.DurationScale));
        }

        protected virtual int                   CalcEndTime(int startTime, float duration)
        {
            return (startTime + Mathf.CeilToInt(duration * this.DurationScale));
        }
        #endregion
        
        #region Data Types
        [Serializable]
        public class                            ScheduledTask
        {
            public T                            _identifier;
            public int                          _startTime;
            public int                          _endTime;
            public Action<T>                    _onCompleted;
            public Action<T>                    _onCanceled;
        }
        #endregion
    }
}
