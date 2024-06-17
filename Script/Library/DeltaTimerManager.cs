using System;
using System.Collections.Generic;

namespace FTGAMEStudio.Timer.DeltaTimers
{
    public enum TimerQueue
    {
        Waiting,
        Update
    }

    public static class DeltaTimerList
    {
        public static DeltaTimerUpdater timerUpdater;

        private static readonly Dictionary<Guid, DeltaTimer> waitingQueue = new();
        private static readonly Dictionary<Guid, DeltaTimer> updateQueue = new();

        public static void AddQueue(DeltaTimer timer, TimerQueue queue)
        {
            switch (queue)
            {
                case TimerQueue.Waiting:
                    waitingQueue.Add(timer.Id, timer);
                    break;
                case TimerQueue.Update:
                    updateQueue.Add(timer.Id, timer);
                    break;
            }

#if UNITY_EDITOR
            UpdateShowDeltaTimer();
#endif
        }

        public static void Awake(Guid id)
        {
            if (!updateQueue.ContainsKey(id) && waitingQueue.ContainsKey(id)) updateQueue.Add(waitingQueue[id].Id, waitingQueue[id]);

#if UNITY_EDITOR
            UpdateShowDeltaTimer();
#endif
        }

        public static void Sleep(Guid id)
        {
            if (updateQueue.ContainsKey(id)) updateQueue.Remove(waitingQueue[id].Id);

#if UNITY_EDITOR
            UpdateShowDeltaTimer();
#endif
        }
        public static void Remove(Guid id)
        {
            timerUpdater.enabled = false;
            if (waitingQueue.ContainsKey(id)) waitingQueue.Remove(id);
            if (updateQueue.ContainsKey(id)) updateQueue.Remove(id);
            timerUpdater.enabled = true;

#if UNITY_EDITOR
            UpdateShowDeltaTimer();
#endif
        }

        public static DeltaTimer GetTimer(string name)
        {
            foreach (var value in waitingQueue) if (value.Value.name == name) return value.Value;
            return null;
        }
        
        public static T GetTimer<T>(string name) where T : DeltaTimer
        {
            foreach (var value in waitingQueue) if (value.Value.name == name && value.Value is T) return value.Value as T;
            return null;
        }

        public static void UpdateTimer(float delta)
        {
            List<Guid> keys = new(updateQueue.Keys);

#if UNITY_EDITOR
            List<DeltaTimerUpdater.DebugDeltaTimerInformation> debugInformation = new();
#endif
            foreach (Guid value in keys)
            {
                    updateQueue[value].Update(delta);
#if UNITY_EDITOR
                if (timerUpdater.showDeltaTimer) if (updateQueue.TryGetValue(value, out DeltaTimer deltaTimer)) debugInformation.Add(new() { name = deltaTimer.name, id = deltaTimer.Id.ToString() });
#endif
            }
#if UNITY_EDITOR
            timerUpdater.updatedDeltaTimers = debugInformation;
#endif
        }

#if UNITY_EDITOR
        public static void UpdateShowDeltaTimer()
        {
            if (timerUpdater.showDeltaTimer)
            {
                timerUpdater.waitingQueue = new();
                timerUpdater.updateQueue = new();
                foreach (var value in waitingQueue) timerUpdater.waitingQueue.Add(new() { name = value.Value.name, id = value.Value.Id.ToString() });
                foreach (var value in updateQueue) timerUpdater.updateQueue.Add(new() { name = value.Value.name, id = value.Value.Id.ToString() });
            }
        }
#endif
    }
}