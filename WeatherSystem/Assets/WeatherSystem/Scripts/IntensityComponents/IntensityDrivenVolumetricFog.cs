using UnityEngine;
using System.Collections;
using WeatherSystem.Internal;
using System;

namespace WeatherSystem.IntensityComponents
{
    /// <summary>
    /// Drives volumetric fog using intensity data
    /// </summary>
    public class IntensityDrivenVolumetricFog : IntensityDrivenBehaviour
    {
        [SerializeField]
        private new VolumetricLight light;

        [SerializeField]
        [Range(0.0f, 0.025f)]
        private float maxMovementSkyboxExtinction = 0.025f;
        [SerializeField]
        [Range(0.0f, 0.006f)]
        private float maxMovemenScatteringCoef = 0.005f;
        [SerializeField]
        [Range(0.0f, 0.0005f)]
        private float maxMovementExtinctionCoef = 0.0005f;
        [SerializeField]
        [Range(0.6f, 0.25f)]
        private float maxMovementNoiseIntensity = 0.25f;

        //DEBUGGING
        [SerializeField]
        private AnimationCurve curve = new AnimationCurve();

        protected override void UpdateWithIntensity(IntensityData intensityData)
        {
            curve.AddKey(new Keyframe(Time.timeSinceLevelLoad, intensityData.intensity));

            //light.SkyboxExtinctionCoef = (intensityData.intensity/2.0f) + 0.5f; //between 0.5f -> 1.0f
            float target = (intensityData.intensity / 2.0f) + 0.5f;
            light.SkyboxExtinctionCoef = Mathf.MoveTowards(light.SkyboxExtinctionCoef, target, maxMovementSkyboxExtinction);

            target = Scale(0.02f, 0.14f, intensityData.intensity);
            light.ScatteringCoef = Mathf.MoveTowards(light.ScatteringCoef, target, maxMovemenScatteringCoef);

            //light.ExtinctionCoef = Scale(0.003f, 0.05f, intensityData.intensity); //this looks good for storms, not so much for rain

            target = Scale(0.003f, 0.01f, intensityData.intensity);
            light.ExtinctionCoef = Mathf.MoveTowards(light.ExtinctionCoef, target, maxMovementExtinctionCoef);

            target = Scale(0.6f, 5.0f, IntensityData.intensity);
            light.NoiseIntensity = Mathf.MoveTowards(light.NoiseIntensity, target, maxMovementNoiseIntensity);
        }

        protected override void ActivationBehaviour()
        {
            light.enabled = true;
        }

        protected override void FadeDelegate(float t)
        {
            light.SkyboxExtinctionCoef = Mathf.MoveTowards(light.SkyboxExtinctionCoef, 0f, maxMovementSkyboxExtinction);
            light.ScatteringCoef = Mathf.MoveTowards(light.ScatteringCoef, 0f, maxMovemenScatteringCoef);
            light.ExtinctionCoef = Mathf.MoveTowards(light.ExtinctionCoef, 0f, maxMovementExtinctionCoef);
            light.NoiseIntensity = Mathf.MoveTowards(light.NoiseIntensity, 0f, maxMovementNoiseIntensity);
            if (t == 1f)
            {
                light.enabled = false;
            }
        }

        /// <summary>
        /// Scale the difference between two values such that it remains between the given minimum and maximum
        /// </summary>
        /// <param name="min">The minimum value for the resultant value</param>
        /// <param name="max">The maximum value for the resultant value</param>
        /// <param name="scale">The scale</param>
        /// <returns>The resultant scaled value</returns>
        private float Scale(float min, float max, float scale)
        {
            float diff = max - min;
            return (diff * scale) + min;
        }
    }
}