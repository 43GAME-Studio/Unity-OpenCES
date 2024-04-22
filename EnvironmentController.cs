using UnityEngine;
using System;

namespace FSGAMEStudio.Environment
{
    /// <summary>
    /// ����������
    /// </summary>
    [AddComponentMenu("FSGAMEStudio/Environment/Environment Controller"), ExecuteInEditMode]
    public class EnvironmentController : MonoBehaviour
    {
        [Header("����")]
        public Transform followObject;
        public Vector3 offsets;
        public float damp = 0.5f;
        [NonSerialized] public Vector3 followDamp;

        /// <summary>
        /// ��տ�����
        /// </summary>
        [Header("����")]
        public SkyController sky;
        public WeatherController weather;

#if UNITY_EDITOR
        [Header("�༭�� (�����ֶν��ڱ༭������)")]
        public bool updateCloudInEditor = false;
#endif

        private void Start()
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
            {
#endif
                sky.cloud.Update();
                sky.sun.Start();

                weather.cloud = sky.cloud;
                weather.sun = sky.sun;
                weather.UpdateWeather();
#if UNITY_EDITOR
            }
#endif
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                weather.UpdateWeather();
            }
            else if (updateCloudInEditor)
            {
                sky.cloud.Update();
            }
#endif
            if (followObject != null)
            {
                Vector3 target = followObject.position + offsets;
                transform.position = Vector3.SmoothDamp(transform.position, target, ref followDamp, damp);
            }
        }
    }
}
