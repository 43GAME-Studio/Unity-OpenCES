using UnityEngine;
using System;
using System.Collections.Generic;

namespace FSGAMEStudio.Environment
{
    /// <summary>
    /// ��������
    /// </summary>
    [AddComponentMenu("FSGAMEStudio/Environment/Particle Controller")]
    public class ParticleController : MonoBehaviour
    {
        /// <summary>
        /// ���Ӽ���
        /// </summary>
        public enum ParticleLevel
        {
            /// <summary>
            /// С��
            /// </summary>
            Little,
            /// <summary>
            /// �е�
            /// </summary>
            Middle,
            /// <summary>
            /// ���
            /// </summary>
            Large
        }

        [Serializable]
        public class ParticleData
        {
            public string id;
            public ParticleSystem particleSystem;
            public float little;
            public float middle;
            public float large;
            public int subIndex = 1;
            public float subLittle;
            public float subMiddle;
            public float subLarge;

            /// <param name="particleLevel">particleLevel ���� enable Ϊ true ʱ����</param>
            public void Set(bool enable, ParticleLevel particleLevel)
            {
                if (enable)
                {
                    Play(particleLevel);
                }
                else
                {
                    Stop();
                }
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
                    switch (particleLevel)
                    {
                        case ParticleLevel.Little:
                            subEmitters.SetSubEmitterEmitProbability(subIndex, subLittle);
                            break;
                        case ParticleLevel.Middle:
                            subEmitters.SetSubEmitterEmitProbability(subIndex, subMiddle);
                            break;
                        case ParticleLevel.Large:
                            subEmitters.SetSubEmitterEmitProbability(subIndex, subLarge);
                            break;
                    }
                }

                particleSystem.Play();
            }
            public void Stop() => particleSystem.Stop();
        }

        public List<ParticleData> particleDatas = new()
        {
            new()
            {
                id = "Rain",
                little = 125,
                middle = 250,
                large = 500,
                subLittle = 0.1f,
                subMiddle = 0.5f,
                subLarge = 1
            },
            new()
            {
                id = "Fog",
                little = 20,
                middle = 40,
                large = 80
            }
        };

        /// <summary>
        /// �Ƿ���������ϵͳ
        /// </summary>
        public void Set(bool enable, ParticleLevel particleLevel) { foreach (var value in particleDatas) value.Set(enable, particleLevel); }
        public void Set(bool enable, ParticleLevel particleLevel, string id)
        {
            foreach (var value in particleDatas)
            {
                if (value.id == id) value.Set(enable, particleLevel);
            }
        }
        public void Set(bool enable, ParticleLevel particleLevel, int index) => particleDatas[index].Set(enable, particleLevel);

        public void Play(ParticleLevel particleLevel) { foreach (var value in particleDatas) value.Play(particleLevel); }
        public void Stop() { foreach (var value in particleDatas) value.Stop(); }
    }
}