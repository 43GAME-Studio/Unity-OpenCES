using UnityEngine;
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
#endif

namespace FTGAMEStudio.Timer.DeltaTimers
{
    [AddComponentMenu("Game System/Timer/Delta Timer/Delta Tiemr Updater")]
    public class DeltaTimerUpdater : MonoBehaviour
    {
#if UNITY_EDITOR
        [Serializable]
        public struct DebugDeltaTimerInformation
        {
            public string name;
            public string id;
        }

        [Header("Debug (以下字段仅在编辑器工作)")]
        public bool showDeltaTimer = false;
        public List<DebugDeltaTimerInformation> waitingQueue;
        public List<DebugDeltaTimerInformation> updateQueue;
        public List<DebugDeltaTimerInformation> updatedDeltaTimers;

        private void Awake()
        {
            if (DeltaTimerList.timerUpdater == null)
            {
                DeltaTimerList.timerUpdater = this;
            }
            else
            {
                Debug.LogWarning("There is more than one delta timer updater in the scene.", gameObject);
                DestroyImmediate(this);
            }
        }
#else
        private void Awake() => DeltaTimerList.timerUpdater = this;
#endif

        private void Update() => DeltaTimerList.UpdateTimer(Time.deltaTime);

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                waitingQueue = new();
                updateQueue = new();
                updatedDeltaTimers = new();
            }
        }
#endif
    }
}
