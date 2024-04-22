using UnityEngine;
using System;
using FSGAMEStudio.Timer.DeltaTimers;
using DG.Tweening;
using System.Collections.Generic;

namespace FSGAMEStudio.Environment
{
    /// <summary>
    /// ��տ�����
    /// </summary>
    [Serializable]
    public class SkyController
    {
        /// <summary>
        /// ̫��������
        /// </summary>
        public SunController sun;
        /// <summary>
        /// �ƿ�����
        /// </summary>
        public CloudController cloud;
    }

    /// <summary>
    /// ̫��������
    /// </summary>
    [Serializable]
    public class SunController
    {
        #region ���ƶ���
        public Transform transform;
        public Light light;
        public LensFlare lightFlare;
        #endregion

        #region ����
        public DeltaTimer deltaTimer;
        [Space]
        public Date.DateTime dateTime = Date.DateTime.Default;
        [Tooltip("ʱ������ Ĭ������ʵʱ��� 72 ��")]
        public float delta = 72f;

        [Tooltip("̫���� Y �� Z ��Ӧ����ô��ת")]
        public Vector2 YZRotation = new(90, 0);
        [Tooltip("һ��Ӧ����ת���ٶ�\nĬ�Ϲ�ʽ�� 360 / 86400")]
        public float secondRotation = 360f / 86400f;
        #endregion

        #region ����
        /// <summary>
        /// ��ʼ�� ���� WeatherController ֮ǰ����
        /// </summary>
        public void Start() => deltaTimer = new(1f / delta, (DeltaTimer timer) => { UpdateTransform(); dateTime.AddSecond(); }, new(null, null, null, null, true, true), true, "DateTimeUpdater"); 

        /// <summary>
        /// ���±任
        /// </summary>
        public void UpdateTransform()
        {
            Vector3 rotation = new(secondRotation * dateTime.secondOfDay, YZRotation.x, YZRotation.y);
            transform.localEulerAngles = rotation;
        }

        /// <summary>
        /// �Ƿ������Ź�
        /// </summary>
        public void SetFlare(bool enable) => lightFlare.enabled = enable;

        public void SetFlareBrightness(float target) => lightFlare.brightness = target;
        public void SetFlareBrightnessSmooth(float target, float time = 1) => DOTween.To(() => lightFlare.brightness, setter => lightFlare.brightness = setter, target, time);

        /// <summary>
        /// ƽ��������ɫ
        /// </summary>
        public void SetColorSmooth(Color to, float time = 1) => light.DOColor(to, time);
        /// <summary>
        /// ƽ������ǿ��
        /// </summary>
        public void SetIntensitySmooth(float to, float time = 1) => light.DOIntensity(to, time);
        #endregion
    }

    /// <summary>
    /// �ƿ�����
    /// </summary>
    [Serializable]
    public class CloudController
    {
        /// <summary>
        /// ������
        /// </summary>
        [Serializable]
        public class CloudData
        {
            public string id;
            public MeshRenderer meshRenderer;
            public Color color = new(1, 1, 1, 0.1960784f);
            [Range(0, 2)] public float density = 1;
            [Range(-1, 1)] public float speed = 0.5f;

            private Material material;
            public Material Material
            {
#if UNITY_EDITOR
                get
                {
                    if (Application.isPlaying)
                    {
                        material = material != null ? material : meshRenderer.material;
                    }
                    else
                    {
                        material = material != null ? material : meshRenderer.sharedMaterial;
                    }
                    return material;
                }
#else
                get => material = material != null ? material : meshRenderer.material;
#endif
                set => material = value;
            }

            public void SetColor(Color color, string target = "_CloudColor") { Material.SetColor(target, color); this.color = color; }
            public void SetColorSmooth(Color targetColor, float time, string target = "_CloudColor") => Material.DOColor(targetColor, target, time).onComplete += () => color = targetColor;

            public void SetDensity(float density, string target = "_Density") { Material.SetFloat(target, density); this.density = density; }
            public void SetDensitySmooth(float targetDensity, float time, string target = "_Density") => Material.DOFloat(targetDensity, target, time).onComplete += () => density = targetDensity;

            public void SetSpeed(float speed, string target = "_Speed") { Material.SetFloat(target, speed); this.speed = speed; }
            public void SetSpeedSmooth(float targetSpeed, float time, string target = "_Speed") => Material.DOFloat(targetSpeed, target, time).onComplete += () => speed = targetSpeed;

            public void Update()
            {
                SetSpeed(speed);
                SetDensity(density);
                SetColor(color);
            }
        }

        public List<CloudData> clouds = new()
        {
            new()
            {
                id = "lowCloud",
                color = new(1, 1, 1, 0.1960784f),
                density = 1.3f,
                speed = -0.25f
            },
            new()
            {
                id = "highCloud",
                color = new(1, 1, 1, 0.8627451f),
                density = 1.5f,
                speed = -0.3f
            }
        };

        public void SetColor(Color color, string target = "_CloudColor") { foreach (var value in clouds) value.SetColor(color, target); }
        public void SetColor(Color color, string id, string target = "_CloudColor")
        {
            foreach (var value in clouds)
            {
                if (value.id == id) value.SetColor(color, target);
            }
        }
        public void SetColor(Color color, int index, string target = "_CloudColor") => clouds[index - 1].SetColor(color, target);
        public void SetColorSmooth(Color targetColor, float time, string target = "_CloudColor") { foreach (var value in clouds) value.SetColorSmooth(targetColor, time, target); }
        public void SetColorSmooth(Color targetColor, string id, float time, string target = "_CloudColor")
        {
            foreach (var value in clouds)
            {
               if (value.id == id) value.SetColorSmooth(targetColor, time, target);
            }
        }
        public void SetColorSmooth(Color targetColor, int index, float time, string target = "_CloudColor") => clouds[index - 1].SetColorSmooth(targetColor, time, target);

        public void SetDensity(float density, string target = "_Density") { foreach (var value in clouds) value.SetDensity(density, target); }
        public void SetDensity(float density, string id, string target = "_Density")
        {
            foreach (var value in clouds)
            {
                if (value.id == id) value.SetDensity(density, target);
            }
        }
        public void SetDensity(float density, int index, string target = "_Density") => clouds[index - 1].SetDensity(density, target);
        public void SetDensitySmooth(float targetDensity, float time, string target = "_Density") { foreach (var value in clouds) value.SetDensitySmooth(targetDensity, time, target); }
        public void SetDensitySmooth(float targetDensity, string id, float time, string target = "_Density")
        {
            foreach (var value in clouds)
            {
                if (value.id == id) value.SetDensitySmooth(targetDensity, time, target);
            }
        }
        public void SetDensitySmooth(float targetDensity, int index, float time, string target = "_Density") => clouds[index - 1].SetDensitySmooth(targetDensity, time, target);

        public void SetSpeed(float speed, string target = "_Speed") { foreach (var value in clouds) value.SetSpeed(speed, target); }
        public void SetSpeed(float speed, string id, string target = "_Speed")
        {
            foreach (var value in clouds)
            {
                if (value.id == id) value.SetSpeed(speed, target);
            }
        }
        public void SetSpeed(float speed, int index, string target = "_Speed") => clouds[index - 1].SetSpeed(speed, target);
        public void SetSpeedSmooth(float targetSpeed, float time, string target = "_Speed") { foreach (var value in clouds) value.SetSpeedSmooth(targetSpeed, time, target); }
        public void SetSpeedSmooth(float targetSpeed, string id, float time, string target = "_Speed")
        {
            foreach (var value in clouds)
            {
                if (value.id == id) value.SetSpeedSmooth(targetSpeed, time, target);
            }
        }
        public void SetSpeedSmooth(float targetSpeed, int index, float time, string target = "_Speed") => clouds[index - 1].SetSpeedSmooth(targetSpeed, time, target);

        /// <summary>
        /// ����
        /// </summary>
        public void Update() { foreach (var value in clouds) value.Update(); }
    }
}
