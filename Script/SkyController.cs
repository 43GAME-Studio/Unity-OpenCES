﻿using UnityEngine;
using System;
using DG.Tweening;
using System.Collections.Generic;
using FTGAMEStudio.Date;

namespace FTGAMEStudio.Environment
{
    /// <summary>
    /// 天空控制器
    /// </summary>
    [Serializable]
    public class SkyController
    {
        /// <summary>
        /// 太阳控制器
        /// </summary>
        public SunController sun;
        /// <summary>
        /// 云控制器
        /// </summary>
        public CloudController cloud;
    }

    /// <summary>
    /// 太阳控制器
    /// </summary>
    [Serializable]
    public class SunController
    {
        public Transform transform;
        public Light light;
        public LensFlare lightFlare;

        [Space]
        public DateTimer dateTimer;
        [Tooltip("日期时间更新器的名称")]
        public string dateTimerName = "DateTimeUpdater";
        [Tooltip("时间增量 默认是现实时间的 72 倍")]
        public float delta = 72f;
        [Tooltip("太阳在 Y 和 Z 轴应该怎么旋转")]
        public Vector2 YZRotation = new(90, 0);
        public float hourAmbientIntensity = 23 / 86400f;
        public AnimationCurve dayAmbientIntensity = new(new Keyframe[] { new(0, 0.5f), new(6, 1.25f), new(12, 0.5f), new(18, 0), new(23, 0.5f) });
        [Tooltip("一秒应该旋转多少度\n默认公式是 360 / 86400")]
        public float secondRotation = 360f / 86400f;

        /// <summary>
        /// 初始化 请在 WeatherController 之前调用
        /// </summary>
        public void Start() => dateTimer = new(UpdateSun, 1f / delta, new(null, null, null, null, true, true), true, dateTimerName);

        public void UpdateSun(Date.DateTime dateTime)
        {
            RenderSettings.ambientIntensity = dayAmbientIntensity.Evaluate(hourAmbientIntensity * dateTime.SecondOfDay);
            transform.localEulerAngles = new(secondRotation * dateTime.SecondOfDay, YZRotation.x, YZRotation.y);
        }

        public void SetFlare(bool enable) => lightFlare.enabled = enable;

        public void SetFlareBrightness(float target) => lightFlare.brightness = target;
        public void SetFlareBrightnessSmooth(float target, float time = 1) => DOTween.To(() => lightFlare.brightness, setter => lightFlare.brightness = setter, target, time);

        public void SetColorSmooth(Color to, float time = 1) => light.DOColor(to, time);
        public void SetIntensitySmooth(float to, float time = 1) => light.DOIntensity(to, time);
    }

    /// <summary>
    /// 云控制器
    /// </summary>
    [Serializable]
    public class CloudController
    {
        /// <summary>
        /// 云数据
        /// </summary>
        [Serializable]
        public class CloudData
        {
            public string id;
            public MeshRenderer meshRenderer;
            public Color color = new(1, 1, 1, 0.3490196f);
            [Range(0, 2)] public float density = 2;
            [Range(-1, 1)] public float speed = -0.25f;

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
            },
            new()
            {
                id = "highCloud",
                color = new(1, 1, 1, 0.2980392f),
                density = 2,
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
        /// 更新
        /// </summary>
        public void Update() { foreach (var value in clouds) value.Update(); }
    }
}
