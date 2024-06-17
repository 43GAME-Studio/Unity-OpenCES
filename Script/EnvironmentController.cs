using UnityEngine;

namespace FTGAMEStudio.Environment
{
    /// <summary>
    /// 环境控制器
    /// </summary>
    [AddComponentMenu("Game System/Environment/Environment Controller")]
    public class EnvironmentController : MonoBehaviour
    {
        [Header("跟随")]
        public Transform followObject;
        public Vector3 offsets;
        public float damp = 0.2f;
        private Vector3 followDamp;

        /// <summary>
        /// 天空控制器
        /// </summary>
        [Header("控制")]
        public SkyController sky;
        public WeatherController weather;

        private void Start()
        {
            sky.cloud.Update();
            sky.sun.Start();

            weather.cloud = sky.cloud;
            weather.sun = sky.sun;
            weather.UpdateWeather();
        }

        private void Update()
        {
#if UNITY_EDITOR
            weather.UpdateWeather();
#endif

            if (followObject != null)
            {
                Vector3 target = followObject.position + offsets;
                transform.position = Vector3.SmoothDamp(transform.position, target, ref followDamp, damp);
            }
        }

#if UNITY_EDITOR
        private void OnValidate() => sky.cloud.Update();
#endif
    }
}
