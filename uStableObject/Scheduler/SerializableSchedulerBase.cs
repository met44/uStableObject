using UnityEngine;
using UnityEngine.Serialization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using uStableObject.Data;

namespace                                       uStableObject
{
    public abstract partial class               SerializableSchedulerBase<T> : ScriptableObject
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
        protected List<TaskAction>              _taskActions = new List<TaskAction>();
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
                AutoPool<ScheduledTask>.Dispose(sTask);
            }
            for (var i = 0; i < this._scheduledTasks.Count; ++i)
            {
                sTask = this._scheduledTasks[i];
                AutoPool<ScheduledTask>.Dispose(sTask);
            }
            this._scheduledTasksUnique.Clear();
            this._scheduledTasks.Clear();
            this._running = false;
        }

        public void                             RegisterTaskAction(uint actionID, string debugName, object debugContext, Action<T> onCompleted, Action<T> onCanceled)
        {
            TaskAction                          taskAction = null;

            if (this._taskActions.Count > actionID)
            {
                taskAction = this._taskActions[(int)actionID];
            }
            if (taskAction == null)
            {
                taskAction = new TaskAction() { _id = actionID, _debugName = debugName, _debugContext = debugContext };
                while (this._taskActions.Count <= actionID)
                {
                    this._taskActions.Add(null);
                }
                this._taskActions[(int)actionID] = taskAction;
            }
            taskAction._onCompleted = onCompleted;
            taskAction._onCanceled = onCanceled;
        }

        public void                             UnregisterTaskAction(uint actionID)
        {
            TaskAction                          taskAction = null;

            if (this._taskActions.Count > actionID)
            {
                taskAction = this._taskActions[(int)actionID];
            }
            if (taskAction != null)
            {
                taskAction._onCompleted = null;
                taskAction._onCanceled = null;
            }
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
                            this.GetTaskAction(sTask)._onCompleted?.Invoke(sTask._identifier);
                            AutoPool<ScheduledTask>.Dispose(sTask);
                        }
                        while (this._scheduledTasks.Count > 0
                               && this._scheduledTasks[0]._endTime <= this._timeProvider.CurrentTime)
                        {
                            sTask = this._scheduledTasks[0];
                            this._scheduledTasks.RemoveAt(0);
                            this.GetTaskAction(sTask)._onCompleted?.Invoke(sTask._identifier);
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
                    this.GetTaskAction(sTask)._onCanceled?.Invoke(identifier);
                    AutoPool<ScheduledTask>.Dispose(sTask);
                    break;
                }
            }
        }

        public void                             DiscardUnique(T identifier)
        {
            ScheduledTask                       sTask;

            for (var i = 0; i < this._scheduledTasksUnique.Count; ++i)
            {
                sTask = this._scheduledTasksUnique[i];
                if (sTask._identifier.Equals(identifier))
                {
                    this._scheduledTasksUnique.RemoveAt(i);
                    //this.GetTaskAction(sTask)._onCanceled?.Invoke(identifier);
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
                    this.GetTaskAction(sTask)._onCanceled?.Invoke(identifier);
                    AutoPool<ScheduledTask>.Dispose(sTask);
                    if (--max <= 0)
                    {
                        break;
                    }
                }
            }
        }

        public void                             DiscardAll(T identifier, int max = int.MaxValue)
        {
            ScheduledTask                       sTask;

            for (var i = this._scheduledTasks.Count - 1; i >= 0; --i)
            {
                sTask = this._scheduledTasks[i];
                if (sTask._identifier.Equals(identifier))
                {
                    this._scheduledTasks.RemoveAt(i);
                    //this.GetTaskAction(sTask)._onCanceled?.Invoke(identifier);
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
                                                               uint registeredActionId)
        {
            ScheduleUnique((uint)this.TimeProvider.CurrentTime, this.CalcEndTime(duration), identifier, registeredActionId);
        }

        public void                             ScheduleUnique(uint startTime,
                                                               uint endTime,
                                                               T identifier,
                                                               uint registeredActionId)
        {
            ScheduledTask                       sTask = null;

            for (var i = 0; i < this._scheduledTasksUnique.Count; ++i)
            {
                if (this._scheduledTasksUnique[i]._identifier.Equals(identifier))
                {
                    sTask = this._scheduledTasksUnique[i];
                    this._scheduledTasksUnique.RemoveAt(i);
                    this.GetTaskAction(sTask)._onCanceled?.Invoke(identifier);
                    break;
                }
            }
            if (sTask == null)
            {
                sTask = AutoPool<ScheduledTask>.Create();
                sTask._identifier = identifier;
            }
            sTask._startTime = startTime;
            sTask._endTime = endTime;
            sTask._actionID = registeredActionId;
            for (var i = 0; i < this._scheduledTasksUnique.Count; ++i)
            {
                if (this._scheduledTasksUnique[i]._endTime >= sTask._endTime)
                {
                    this._scheduledTasksUnique.Insert(i, sTask);
                    return;
                }
            }
            this._scheduledTasksUnique.Add(sTask);
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
            instance._endTime = (uint)this.CalcEndTime(instance._startTime, newDuration);
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
                                                         uint registeredActionId)
        {
            Schedule((uint)this.TimeProvider.CurrentTime, this.CalcEndTime(duration), identifier, registeredActionId);
        }

        public void                             Schedule(uint startTime,
                                                         uint endTime,
                                                         T identifier,
                                                         uint registeredActionId)
        {
            ScheduledTask                       instance = null;

            instance = AutoPool<ScheduledTask>.Create();
            instance._identifier = identifier;
            instance._startTime = startTime;
            instance._endTime = endTime;
            instance._actionID = registeredActionId;
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
        protected virtual uint                  CalcEndTime(float duration)
        {
            return ((uint)(this._timeProvider.CurrentTime + Mathf.CeilToInt(duration * this.DurationScale)));
        }

        protected virtual uint                  CalcEndTime(uint startTime, float duration)
        {
            return (startTime + (uint)Mathf.CeilToInt(duration * this.DurationScale));
        }

        protected TaskAction                    GetTaskAction(ScheduledTask task)
        {
            return (this._taskActions[(int)task._actionID]);
        }
        #endregion
        
        #region Data Types
        public class                            TaskAction
        {
            public uint                         _id;
            public string                       _debugName;
            public object                       _debugContext;
            public Action<T>                    _onCompleted;
            public Action<T>                    _onCanceled;
        }

        [Serializable]
        public class                            ScheduledTask
        {
            public T                            _identifier;
            public uint                         _startTime;
            public uint                         _endTime;
            public uint                         _actionID;
        }
        #endregion
    }
}
