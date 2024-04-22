using UnityEngine;
using System.Collections.Generic;
using System;
using DG.Tweening;

namespace FSGAMEStudio.Environment
{
    /// <summary>
    /// ÉùÒô¿ØÖÆÆ÷
    /// </summary>
    [AddComponentMenu("FSGAMEStudio/Environment/Sound Controller")]
    public class SoundController : MonoBehaviour
    {
        [Serializable]
        public class AudioObject
        {
            public string usageScenarios;
            public float smoothVolume = 1;
            public List<AudioSource> audioPack;

            public void Play() { foreach (var value in audioPack) { value.DOFade(1, smoothVolume).onPlay += () => { value.gameObject.SetActive(true); value.Play(); }; } }
            public void Play(ref AudioObject playing) { Play(); if (playing != this) playing?.Stop(); playing = this; }
            public void Stop() { foreach (var value in audioPack) value.DOFade(0, smoothVolume).onComplete += () => { value.Stop(); value.gameObject.SetActive(false); }; }
            public void Stop(ref AudioObject playing) { Stop(); playing = playing != this ? playing : null; }
        }

        [NonSerialized] public AudioObject playing;

        public List<AudioObject> soundList = new()
        {
            new()
            {
                usageScenarios = "Rain"
            },
            new()
            {
                usageScenarios = "Storm"
            },
            new()
            {
                usageScenarios = "Cloudy"
            }
        };

        public void Play(int id) => soundList[id].Play(ref playing);
        public void Play(string usageScenarios)
        {
            foreach (var value in soundList)
            {
                if (value.usageScenarios == usageScenarios) { value.Play(ref playing); break; }
            }
        }

        public void Stop(int id) => soundList[id].Stop(ref playing);
        public void Stop(string usageScenarios)
        {
            foreach (var value in soundList)
            {
                if (value.usageScenarios == usageScenarios) { value.Stop(ref playing); break; }
            }
        }
    }
}
