using UnityEngine;

namespace FSGAMEStudio.Timer.DeltaTimers
{
    [AddComponentMenu("FSGAMEStudio/Timer/Delta Timer/Delta Tiemr Updater")]
    public class DeltaTimerUpdater : MonoBehaviour
    {
        private void Start()
        {
            if (DeltaTimerList.TimerUpdater == null)
            {
                DeltaTimerList.TimerUpdater = this;
            }
            else
            {
                Debug.LogWarning("There is more than one delta timer updater in the scene.", gameObject);
                gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            StartCoroutine(DeltaTimerList.UpdateDeltaTimer(Time.deltaTime));
        }
    }
}
