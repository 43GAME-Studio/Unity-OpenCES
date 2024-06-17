using UnityEngine;
using System;
using System.Collections.Generic;

namespace FTGAMEStudio.Environment
{
    /// <summary>
    /// 环境粒子
    /// </summary>
    [AddComponentMenu("Game System/Environment/Particle Controller")]
    public class ParticleController : MonoBehaviour
    {
        /// <summary>
        /// 粒子级别
        /// </summary>
        public enum ParticleLevel
        {
            /// <summary>
            /// 小的
            /// </summary>
            Little,
            /// <summary>
            /// 中的
            /// </summary>
            Middle,
            /// <summary>
            /// 大的
            /// </summary>
            Large
        }

        [Serializable]
        public class SubParticeOverride
        {
            public int index;
            public float little;
            public float middle;
            public float large;
        }

        [Serializable]
        public class ParticleData
        {
            public string id;
            public ParticleSystem particleSystem;
            public float little;
            public float middle;
            public float large;
            public List<SubParticeOverride> subParticeOverrides;

            /// <param name="particleLevel">particleLevel 仅在 enable 为 true 时工作</param>
            public void Set(bool enable, ParticleLevel particleLevel)
            {
                if (enable) Play(particleLevel);
                else Stop();
            }

            public void Play(ParticleLevel particleLevel)
            {
                ParticleSystem.SubEmittersModule subEmitters = particleSystem.subEmitters;
                ParticleSystem.EmissionModule emission = particleSystem.emission;

                emission.rateOverTime = particleLevel switch
                {
                    ParticleLevel.Little => little,
                    ParticleLevel.Middle => middle,
                    ParticleLevel.Large => large,
                    _ => 0,
                };

                if (subEmitters.subEmittersCount > 0)
                {
                    foreach (var value in subParticeOverrides)
                    {
                        switch (particleLevel)
                        {
                            case ParticleLevel.Little:
                                subEmitters.SetSubEmitterEmitProbability(value.index, value.little);
                                break;
                            case ParticleLevel.Middle:
                                subEmitters.SetSubEmitterEmitProbability(value.index, value.middle);
                                break;
                            case ParticleLevel.Large:
                                subEmitters.SetSubEmitterEmitProbability(value.index, value.large);
                                break;
                        }
                    }
                }

                particleSystem.Play();
            }
            public void Stop() => particleSystem.Stop();

            private Vector3 voltBackup;
            /// <summary>
            /// 赋值为实例时将实例与 VelocityOverLifetime 相加<br/>
            /// 赋值为 Vector3.zero 时重置 VelocityOverLifetime 为在所有设置发生之前的状态
            /// </summary>
            public Vector3 VelocityOverLifetime
            {
                set
                {
                    ParticleSystem.VelocityOverLifetimeModule volt = particleSystem.velocityOverLifetime;
                    if (value != Vector3.zero)
                    {
                        voltBackup = voltBackup == null ? new Vector3(volt.x.constantMin, volt.y.constantMin, volt.z.constantMin) : voltBackup;
                        volt.enabled = volt.enabled == false || volt.enabled;
                        volt.x = new(volt.x.constantMin + value.x, volt.x.constantMax);
                        volt.y = new(volt.y.constantMin + value.y, volt.y.constantMax);
                        volt.z = new(volt.z.constantMin + value.z, volt.z.constantMax);
                    }
                    else
                    {
                        volt.x = new(voltBackup.x, volt.x.constantMax);
                        volt.y = new(voltBackup.y, volt.y.constantMax);
                        volt.z = new(voltBackup.z, volt.z.constantMax);
                        voltBackup = Vector3.zero;
                    }
                }
                get => new(particleSystem.velocityOverLifetime.x.constant, particleSystem.velocityOverLifetime.y.constant, particleSystem.velocityOverLifetime.z.constant);
            }
        }

        public List<ParticleData> particleDatas = new()
        {
            new()
            {
                id = "Rain",
                little = 200,
                middle = 400,
                large = 800,
                subParticeOverrides = new()
                {
                    new()
                    {
                        index = 1,
                        little = 0.25f,
                        middle = 0.5f,
                        large = 1,
                    },
                    new()
                    {
                        index = 2,
                        little = 0.25f,
                        middle = 0.5f,
                        large = 1,
                    }
                }
            },
            new()
            {
                id = "Fog",
                little = 20,
                middle = 40,
                large = 80
            }
        };

        public ParticleData Get(int index) => particleDatas[index];
        public ParticleData Get(string id)
        {
            foreach (var value in particleDatas)
            {
                if (value.id == id) return value;
            }
            return null;
        }

        public Vector3 VelocityOverLifetime { set { foreach (var data in particleDatas) data.VelocityOverLifetime = value; } }
        public void SetVelocityOverLifetime(Vector3 vector, string id) => Get(id).VelocityOverLifetime = vector;

        /// <summary>
        /// 是否启用整个系统
        /// </summary>
        public void Set(bool enable, ParticleLevel particleLevel) { foreach (var value in particleDatas) value.Set(enable, particleLevel); }
        public void Set(bool enable, ParticleLevel particleLevel, string id) => Get(id).Set(enable, particleLevel);
        public void Set(bool enable, ParticleLevel particleLevel, int index) => Get(index).Set(enable, particleLevel);

        public void Play(ParticleLevel particleLevel) { foreach (var value in particleDatas) value.Play(particleLevel); }
        public void Stop() { foreach (var value in particleDatas) value.Stop(); }
    }
}