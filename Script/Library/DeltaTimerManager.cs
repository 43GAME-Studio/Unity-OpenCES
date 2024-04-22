using System.Collections.Generic;
using System.Collections;

namespace FSGAMEStudio.Timer.DeltaTimers
{
    public static class DeltaTimerList
    {
        public static DeltaTimerUpdater TimerUpdater { get; set; }

        private static readonly Dictionary<int, DeltaTimer> timerData = new();
        private static readonly Dictionary<int, bool> metaData = new();

        private static bool Has(int id)
        {
            return timerData.ContainsKey(id) && metaData.ContainsKey(id);
        }

        public static void AddTimer(DeltaTimer timerData, bool metaData = false)
        {
            if (!Has(timerData.Id))
            {
                DeltaTimerList.timerData.Add(timerData.Id, timerData);
                DeltaTimerList.metaData.Add(timerData.Id, metaData);
            }
        }
        public static void SetTimer(int id, bool metaData)
        {
            if (Has(id)) DeltaTimerList.metaData[id] = metaData;
        }
        public static void RemoveTimer(int id)
        {
            if (Has(id))
            {
                timerData.Remove(id);
                metaData.Remove(id);
            }
        }

        public static IEnumerator UpdateDeltaTimer(float delta)
        {
            foreach (var value in metaData)
            {
                if (value.Value) timerData[value.Key].Update(delta);
            }
            yield return null;
        }
    }
}