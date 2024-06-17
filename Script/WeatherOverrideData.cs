using System.Collections.Generic;
using System;
using UnityEngine;

namespace FTGAMEStudio.Environment
{
    [CreateAssetMenu(fileName = "New Weather Override", menuName = "Environment/Weather Override")]
    public class WeatherOverrideData : ScriptableObject
    {
        /// <summary>
        /// 储存云的变化数据
        /// </summary>
        [Serializable]
        public class CloudOverrideData
        {
            public string id;
            public Color color = new(1, 1, 1, 0.3490196f);
            [Range(0, 2)] public float density = 2;
            [Range(-1, 1)] public float speed = -0.25f;
        }

        /// <summary>
        /// 储存太阳的变化数据
        /// </summary>
        [Serializable]
        public class SunOverrideData
        {
            [Header("Light")]
            public Color color = new(1, 0.9568627f, 0.8392157f, 1);
            public float intensity = 0.7f;

            [Header("Flare")]
            public bool flare = true;
            public float flareBrightness = 1.5f;
        }

        [Serializable]
        public class EnableParticle
        {
            public string id;

            [Header("风 (生命周期内速度 Override)")]
            public Vector3 windDirection;
            public float windPower;
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
            public List<EnableParticle> enableParticles;
            public float smoothSpeed = 2;
            public ParticleController.ParticleLevel particleLevel = ParticleController.ParticleLevel.Little;

            [Header("天空")]
            public List<CloudOverrideData> overrideClouds;
            public SunOverrideData overrideSun;
        }

        public List<OverrideData> overrideDatas = new()
        {
            new()
            {
                id = "Sunny",
                soundSystem = false,
                particleSystem = false,
                overrideClouds = new()
                {
                    new()
                    {
                        id = "lowCloud",
                        color = new(1, 1, 1, 0.3490196f),
                        density = 2,
                        speed = -0.25f
                    },
                    new()
                    {
                        id = "highCloud",
                        color = new(1, 1, 1, 0.2980392f),
                        density = 2,
                        speed = -0.3f
                    }
                },
                overrideSun = new()
            },
            new()
            {
                id = "Cloudy",
                soundSystem = true,
                particleSystem = false,
                overrideClouds = new()
                {
                    new()
                    {
                        id = "lowCloud",
                        color = new(0.8f, 0.8f, 0.8f, 0.1960784f),
                        density = 1.8f,
                        speed = -0.125f
                    },
                    new()
                    {
                        id = "highCloud",
                        color = new(0.8f, 0.8f, 0.8f, 0.8627451f),
                        density = 1.8f,
                        speed = -0.15f
                    }
                },
                overrideSun = new()
                {
                    color = new(0.8f, 0.7654902f, 0.6713726f, 1),
                    intensity = 0.5f,
                    flareBrightness = 1,
                },
            },
            new()
            {
                id = "Rain",
                soundSystem = true,
                particleSystem = true,
                enableParticles = new()
                {
                    new()
                    {
                        id = "Rain"
                    }
                },
                particleLevel = ParticleController.ParticleLevel.Middle,
                overrideClouds = new()
                {
                    new()
                    {
                        id = "lowCloud",
                        color = new(0.6f, 0.6f, 0.6f, 0.1960784f),
                        density = 1.3f,
                        speed = -0.5f
                    },
                    new()
                    {
                        id = "highCloud",
                        color = new(0.6f, 0.6f, 0.6f, 0.8627451f),
                        density = 1.3f,
                        speed = -0.7f
                    }
                },
                overrideSun = new()
                {
                    color = new(0.6f, 0.5741177f, 0.5035294f, 1),
                    intensity = 0.3f,
                    flare = false,
                }
            },
            new()
            {
                id = "Storm",
                soundSystem = true,
                particleSystem = true,
                enableParticles = new()
                {
                   new()
                   {
                       id = "Fog",
                       windDirection = Vector3.forward,
                       windPower = 0.5f
                   },
                    new()
                    {
                        id = "Rain",
                        windDirection = Vector3.forward,
                        windPower = 20
                    }
                },
                particleLevel = ParticleController.ParticleLevel.Large,
                overrideClouds = new()
                {
                    new()
                    {
                        id = "lowCloud",
                        color = new(0.4f, 0.4f, 0.4f, 0.1960784f),
                        density = 1,
                        speed = -1f
                    },
                    new()
                    {
                        id = "highCloud",
                        color = new(0.4f, 0.4f, 0.4f, 0.8627451f),
                        density = 1,
                        speed = -1f
                    }
                },
                overrideSun = new()
                {
                    color = new(0.4f, 0.3827451f, 0.3356863f, 1),
                    intensity = 0.1f,
                    flare = false
                },
            }
        };
    }
}
