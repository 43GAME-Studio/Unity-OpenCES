using System;
using UnityEngine;

namespace FTGAMEStudio.Environment
{
    /// <summary>
    /// 天气控制器
    /// </summary>
    [Serializable]
    public class WeatherController
    {
        [NonSerialized] public CloudController cloud;
        [NonSerialized] public SunController sun;

        public string currentWeatherId = "Sunny";
        private string lastCurrentWeatherId = "Sunny";
        public ParticleController particleController;
        public SoundController soundController;

        [Header("Override")]
        public WeatherOverrideData overrideWeather;

        /// <summary>
        /// 根据自身天气 Id 更新天气
        /// </summary>
        public void UpdateWeather()
        {
            if (lastCurrentWeatherId != currentWeatherId)
            {
                lastCurrentWeatherId = currentWeatherId;

                foreach (var value in overrideWeather.overrideDatas)
                {
                    if (value.id == currentWeatherId)
                    {
                        if (value.soundSystem) soundController.Play(currentWeatherId);
                        else soundController.playing.Stop();

                        if (value.particleSystem)
                        {
                            particleController.Stop();
                            foreach (var data in value.enableParticles)
                            {
                                particleController.Set(true, value.particleLevel, data.id);
                                particleController.SetVelocityOverLifetime(data.windDirection.normalized * data.windPower, data.id);
                            }
                        }
                        else particleController.Stop();

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