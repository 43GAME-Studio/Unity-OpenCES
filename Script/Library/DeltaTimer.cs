using System;
using UnityEngine.Events;

namespace FTGAMEStudio.Timer.DeltaTimers
{
    public enum EventTypes
    {
        UnityEvent,
        UnityAction
    }
    public enum TimerStates
    {
        Running,
        Paused,
        Stopped,
        Created,
        BeginDestroy,
    }

    public class EventInformation
    {
        public bool startOnCreate;
        public bool eventOnStart;
        public bool eventOnPause;
        public bool eventOnStop;
        public bool eventOnDestroy;
        public bool destroyOnStop;

        public UnityAction<DeltaTimer> onStart;
        public UnityAction<DeltaTimer> onPause;
        public UnityAction<DeltaTimer> onStop;
        public UnityAction<DeltaTimer> onDestroy;

        /// <summary>
        /// (OnActivity 执行在 EventOnActivity 之后)
        /// </summary>
        public EventInformation(UnityAction<DeltaTimer> onStart = null, UnityAction<DeltaTimer> onPause = null, UnityAction<DeltaTimer> onStop = null, UnityAction<DeltaTimer> onDestroy = null, bool startOnCreate = false, bool eventOnStart = false, bool eventOnPause = false, bool eventOnStop = false, bool eventOnDestroy = false, bool destroyOnStop = false)
        {
            this.startOnCreate = startOnCreate;
            this.eventOnStart = eventOnStart;
            this.eventOnPause = eventOnPause;
            this.eventOnStop = eventOnStop;
            this.eventOnDestroy = eventOnDestroy;
            this.destroyOnStop = destroyOnStop;

            this.onStart = onStart;
            this.onPause = onPause;
            this.onStop = onStop;
            this.onDestroy = onDestroy;
        }
    }

    public abstract class DeltaTimer
    {
        public Guid Id { get; protected set; }
        [NonSerialized] public string name;
        [NonSerialized] public float time;
        public TimerStates State { get; protected set; } = TimerStates.Created;

        [NonSerialized] public bool repeat;
        public EventInformation eventInformation;

        [NonSerialized] public float currentTime = 0;

        /// <param name="time">时间单位是秒</param>
        public DeltaTimer(float time, EventInformation eventInformation, bool repeat = false, string name = null)
        {
            Id = Guid.NewGuid();
            this.time = time;
            this.name = name ?? Id.ToString();

            this.repeat = repeat;

            this.eventInformation = eventInformation;

            DeltaTimerList.AddQueue(this, TimerQueue.Waiting);
            if (eventInformation.startOnCreate) Start();
        }

        public virtual void Update(float delta)
        {
            currentTime += delta;

            if (currentTime >= time)
            {
                currentTime = 0;
                TimeOver();
            }
        }
        protected virtual void TimeOver()
        {
            Invoke();
            if (!repeat) Stop();
        }

        public virtual void Start()
        {
            if (State != TimerStates.Running && State != TimerStates.BeginDestroy)
            {
                DeltaTimerList.Awake(Id);
                State = TimerStates.Running;

                if (eventInformation.eventOnStart) Invoke();
                eventInformation.onStart?.Invoke(this);
            }
        }
        public virtual void Pause()
        {
            if (State != TimerStates.Paused && State != TimerStates.BeginDestroy)
            {
                DeltaTimerList.Sleep(Id);
                State = TimerStates.Paused;

                if (eventInformation.eventOnPause) Invoke();
                eventInformation.onPause?.Invoke(this);
            }
        }
        public virtual void Stop()
        {
            if (State != TimerStates.Stopped && State != TimerStates.BeginDestroy)
            {
                DeltaTimerList.Sleep(Id);
                currentTime = 0;
                State = TimerStates.Stopped;

                if (eventInformation.eventOnStop) Invoke();
                eventInformation.onStop?.Invoke(this);
                if (eventInformation.destroyOnStop) Destroy();
            }
        }
        protected virtual void StopWithNoEvent()
        {
            if (State != TimerStates.Stopped && State != TimerStates.BeginDestroy)
            {
                DeltaTimerList.Sleep(Id);
                currentTime = 0;
                State = TimerStates.Stopped;
            }
        }
        public virtual void Destroy()
        {
            if (State != TimerStates.BeginDestroy)
            {
                State = TimerStates.BeginDestroy;
                StopWithNoEvent();

                if (eventInformation.eventOnDestroy) Invoke();
                eventInformation.onDestroy?.Invoke(this);

                DeltaTimerList.Remove(Id);
            }
        }

        protected abstract void Invoke();
    }

    public class ActionTimer : DeltaTimer
    {
        public UnityAction<DeltaTimer> unityAction;

        public ActionTimer(UnityAction<DeltaTimer> unityAction, float time, EventInformation eventInformation, bool repeat = false, string name = null) : base(time, eventInformation, repeat, name)
        {
            this.unityAction = unityAction;
        }

        protected override void Invoke() => unityAction?.Invoke(this);
    }
    public class EventTimer : DeltaTimer
    {
        public UnityEvent<DeltaTimer> unityEvent;

        public EventTimer(UnityEvent<DeltaTimer> unityEvent, float time, EventInformation eventInformation, bool repeat = false, string name = null) : base(time, eventInformation, repeat, name)
        {
            this.unityEvent = unityEvent;
        }

        protected override void Invoke() => unityEvent?.Invoke(this);
    }
}
