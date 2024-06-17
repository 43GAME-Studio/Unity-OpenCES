using FTGAMEStudio.Date;
using FTGAMEStudio.Timer.DeltaTimers;
using UnityEngine;
using UnityEngine.UI;

namespace FTGAMEStudio.Environment
{
    [AddComponentMenu("Game System/Environment/Date Time Setter")]
    public class DateTimeSetter : MonoBehaviour
    {
        public Text target;
        public bool date = true;

        [Space]
        [Tooltip("日期时间更新器的名称")]
        public string updaterName = "DateTimeUpdater";
        private DateTimer updater;

        private void Start()
        {
            if (target != null)
            {
                updater = DeltaTimerList.GetTimer<DateTimer>(updaterName);
                if (updater != null) updater.action += UpdateDateTime;
            }
        }

        private void UpdateDateTime(DateTime dateTime) => target.text = (date ? dateTime.GetDate() + "\n": "") + dateTime.GetTime();

        private void OnDestroy()
        {
            if (updater != null) updater.action -= UpdateDateTime;
        }
    }
}
