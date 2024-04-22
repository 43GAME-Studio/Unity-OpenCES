using UnityEngine.Events;

namespace FSGAMEStudio.Timer.DeltaTimers
{
    public class DeltaTimer
    {
        public enum EventTypes
        {
            UnityEvent,
            UnityAction
        }
        public enum States
        {
            Running,
            Paused,
            Stopped,
            Created,
            BeginDestroy,
        }
        public class EventInformation
        {
            public bool StartOnCreate { get; protected set; }
            public bool EventOnStart { get; set; }
            public bool EventOnPause { get; set; }
            public bool EventOnStop { get; set; }
            public bool EventOnDestroy { get; set; }
            public bool DestroyOnStop { get; set; }

            public UnityAction<DeltaTimer> OnStart { get; set; }
            public UnityAction<DeltaTimer> OnPause { get; set; }
            public UnityAction<DeltaTimer> OnStop { get; set; }
            public UnityAction<DeltaTimer> OnDestroy { get; set; }
           
            /// <summary>
            /// (OnActivity 执行在 EventOnActivity 之后)
            /// </summary>
            public EventInformation(UnityAction<DeltaTimer> onStart = null, UnityAction<DeltaTimer> onPause = null, UnityAction<DeltaTimer> onStop = null, UnityAction<DeltaTimer> onDestroy = null, bool startOnCreate = false, bool eventOnStart = false, bool eventOnPause = false, bool eventOnStop = false, bool eventOnDestroy = false, bool destroyOnStop = false)
            {
                StartOnCreate = startOnCreate;
                EventOnStart = eventOnStart;
                EventOnPause = eventOnPause;
                EventOnStop = eventOnStop;
                EventOnDestroy = eventOnDestroy;
                DestroyOnStop = destroyOnStop;

                OnStart = onStart;
                OnPause = onPause;
                OnStop = onStop;
                OnDestroy = onDestroy;
            }
        }

        public int Id { get; private set; }
        public string Name { get; set; } = "Void";
        public float Time { get; set; }
        public EventTypes EventType { get; private set; }
        public object Event
        {
            get => EventType switch { EventTypes.UnityEvent => unityEvent, EventTypes.UnityAction => unityAction, _ => null };
            set { switch (EventType) { case EventTypes.UnityEvent: unityEvent = value as UnityEvent<DeltaTimer>; break; case EventTypes.UnityAction: unityAction = value as UnityAction<DeltaTimer>; break; } }
        }
        public States State { get; private set; } = States.Created;

        public bool Repeat { get; set; }
        public EventInformation eventInformation;

        private UnityEvent<DeltaTimer> unityEvent;
        private UnityAction<DeltaTimer> unityAction;

        public float CurrentTime { get; set; } = 0;

        /// <param name="time">时间单位是秒</param>
        public DeltaTimer(float time, UnityEvent<DeltaTimer> timerEvent, EventInformation eventInformation, bool repeat = false, string name = null)
        {
            unityEvent = timerEvent;
            EventType = EventTypes.UnityEvent;
            Build(time, name, repeat, eventInformation);
        }
        /// <param name="time">时间单位是秒</param>
        public DeltaTimer(float time, UnityAction<DeltaTimer> timerEvent, EventInformation eventInformation, bool repeat = false, string name = null)
        {
            unityAction = timerEvent;
            EventType = EventTypes.UnityAction;
            Build(time, name, repeat, eventInformation);
        }

        public void Update(float delta)
        {
            CurrentTime += delta;

            if (CurrentTime >= Time)
            {
                CurrentTime = 0;
                TimeOver();
            }
        }
        private void TimeOver()
        {
            Invoke();
            if (!Repeat) Stop();
        }

        public void Start()
        {
            if (State != States.Running && State != States.BeginDestroy)
            {
                DeltaTimerList.SetTimer(Id, true);
                State = States.Running;

                if (eventInformation.EventOnStart) Invoke();
                eventInformation.OnStart?.Invoke(this);
            }
        }
        public void Pause()
        {
            if (State != States.Paused && State != States.BeginDestroy)
            {
                DeltaTimerList.SetTimer(Id, false);
                State = States.Paused;

                if (eventInformation.EventOnPause) Invoke();
                eventInformation.OnPause?.Invoke(this);
            }
        }
        public void Stop()
        {
            if (State != States.Stopped && State != States.BeginDestroy)
            {
                DeltaTimerList.SetTimer(Id, false);
                CurrentTime = 0;
                State = States.Stopped;

                if (eventInformation.EventOnStop) Invoke();
                eventInformation.OnStop?.Invoke(this);
                if (eventInformation.DestroyOnStop) Destroy();
            }
        }
        private void StopWithNoEvent()
        {
            if (State != States.Stopped && State != States.BeginDestroy)
            {
                DeltaTimerList.SetTimer(Id, false);
                CurrentTime = 0;
                State = States.Stopped;
            }
        }
        public void Destroy()
        {
            if (State != States.BeginDestroy)
            {
                State = States.BeginDestroy;
                StopWithNoEvent();

                if (eventInformation.EventOnDestroy) Invoke();
                eventInformation.OnDestroy?.Invoke(this);

                DeltaTimerList.RemoveTimer(Id);
            }
        }

        private void Invoke()
        {
            switch (EventType)
            {
                case EventTypes.UnityEvent:
                    unityEvent.Invoke(this);
                    break;
                case EventTypes.UnityAction:
                    unityAction.Invoke(this);
                    break;
            }
        }

        private void Build(float time, string name, bool repeat, EventInformation eventInformation)
        {
            Id = GetHashCode();
            Time = time;
            Name = name ?? Id.ToString();

            Repeat = repeat;

            this.eventInformation = eventInformation;

            DeltaTimerList.AddTimer(this);
            if (eventInformation.StartOnCreate) Start();
        }
    }
}
