using System;
using UnityEngine;
using System.Collections.Generic;

namespace FSGAMEStudio.Environment
{
    /// <summary>
    /// 天气控制器
    /// </summary>
    [Serializable]
    public class WeatherController
    {
        /// <summary>
        /// 储存云的变化数据
        /// </summary>
        [Serializable]
        public class CloudOverrideData
        {
            public string id;
            public Color color = new(1, 1, 1, 0.1960784f);
            [Range(0, 2)] public float density = 1;
            [Range(-1, 1)] public float speed = 0.5f;
        }

        /// <summary>
        /// 储存太阳的变化数据
        /// </summary>
        [Serializable]
        public class SunOverrideData
        {
            public Color color = new(1, 1, 1, 1);
            public float intensity = 3;
            public bool flare = true;
            public float flareBrightness = 1;
        }

        /// <summary>
        /// 变化数据的集合
        /// </summary>
        [Serializable]
        public class OverrideData
        {
            public string id;

            [Header("声音")]
            public bool soundSystem;

            [Header("粒子")]
            public bool particleSystem;
            public List<string> particleId;
            public float smoothSpeed = 2;
            public ParticleController.ParticleLevel particleLevel;

            [Header("天空")]
            public List<CloudOverrideData> overrideClouds;
            public SunOverrideData overrideSun;
        }

        [NonSerialized] public CloudController cloud;
        [NonSerialized] public SunController sun;

        public string currentWeatherId = "Sunny";
        private string lastCurrentWeatherId = "Sunny";
        public ParticleController particleController;
        public SoundController soundController;

        [Header("数据")]
        public List<OverrideData> overrideDatas = new()
        {
            new()
            {
                id = "Sunny",
                soundSystem = false,
                particleSystem = false,
                particleLevel = ParticleController.ParticleLevel.Little,
                overrideClouds = new()
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
                },
                overrideSun = new()
                {
                    color = new(1, 1, 1, 1),
                    intensity = 3,
                    flare = true
                }
            },
            new()
            {
                id = "Cloudy",
                soundSystem = true,
                particleSystem = false,
                particleLevel = ParticleController.ParticleLevel.Little,
                overrideClouds = new()
                {
                    new()
                    {
                        id = "lowCloud",
                        color = new(0.8f, 0.8f, 0.8f, 0.1960784f),
                        density = 0.8f,
                        speed = -0.125f
                    },
                    new()
                    {
                        id = "highCloud",
                        color = new(0.8f, 0.8f, 0.8f, 0.8627451f),
                        density = 1.1f,
                        speed = -0.15f
                    }
                },
                overrideSun = new()
                {
                    color = new(0.8f, 0.8f, 0.8f, 1),
                    intensity = 2,
                    flare = true,
                    flareBrightness = 0.8f,
                }
            },
            new()
            {
                id = "Rain",
                soundSystem = true,
                particleSystem = true,
                particleId = new()
                {
                    "Rain"
                },
                particleLevel = ParticleController.ParticleLevel.Middle,
                overrideClouds = new()
                {
                    new()
                    {
                        id = "lowCloud",
                        color = new(0.6f, 0.6f, 0.6f, 0.1960784f),
                        density = 0.6f,
                        speed = -0.5f
                    },
                    new()
                    {
                        id = "highCloud",
                        color = new(0.6f, 0.6f, 0.6f, 0.8627451f),
                        density = 0.8f,
                        speed = -0.7f
                    }
                },
                overrideSun = new()
                {
                    color = new(0.6f, 0.6f, 0.6f, 1),
                    intensity = 1.5f,
                    flare = false,
                }
            },
            new()
            {
                id = "Storm",
                soundSystem = true,
                particleSystem = true,
                particleId = new()
                {
                    "Fog",
                    "Rain"
                },
                particleLevel = ParticleController.ParticleLevel.Large,
                overrideClouds = new()
                {
                    new()
                    {
                        id = "lowCloud",
                        color = new(0.4f, 0.4f, 0.4f, 0.1960784f),
                        density = 0.4f,
                        speed = -1f
                    },
                    new()
                    {
                        id = "highCloud",
                        color = new(0.4f, 0.4f, 0.4f, 0.8627451f),
                        density = 0.6f,
                        speed = -1f
                    }
                },
                overrideSun = new()
                {
                    color = new(0.4f, 0.4f, 0.4f, 1),
                    intensity = 0.5f,
                    flare = false
                }
            }
        };

        /// <summary>
        /// 根据自身天气 Id 更新天气
        /// </summary>
        public void UpdateWeather()
        {
            if (lastCurrentWeatherId != currentWeatherId)
            {
                lastCurrentWeatherId = currentWeatherId;

                foreach (var value in overrideDatas)
                {
                    if (value.id == currentWeatherId)
                    {
                        if (value.soundSystem)
                        {
                            soundController.Play(currentWeatherId);
                        }
                        else
                        {
                            soundController.playing.Stop();
                        }

                        if (value.particleSystem)
                        {
                            particleController.Stop();
                            foreach (var id in value.particleId) particleController.Set(true, value.particleLevel, id);
                        }
                        else
                        {
                            particleController.Stop();
                        }

                        foreach (var data in value.overrideClouds)
                        {
                            cloud.SetColorSmooth(data.color, data.id, value.smoothSpeed);
                            cloud.SetDensitySmooth(data.density, data.id, value.smoothSpeed);
                            cloud.SetSpeedSmooth(data.speed, data.id, value.smoothSpeed);
                        }

                        sun.SetColorSmooth(value.overrideSun.color);
                        sun.SetFlare(value.overrideSun.flare);
                        if (value.overrideSun.flare) sun.SetFlareBrightnessSmooth(value.overrideSun.flareBrightness);
                        sun.SetIntensitySmooth(value.overrideSun.intensity);

                        break;
                    }
                }
            }
        }
        /// <summary>
        /// 根据参数 Id 更新天气
        /// </summary>
        public void UpdateWeather(string weatherId)
        {
            currentWeatherId = weatherId;
            UpdateWeather();
        }
    }
}