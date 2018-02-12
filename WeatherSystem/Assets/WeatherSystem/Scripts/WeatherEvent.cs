using UnityEngine;
using System.Collections;

namespace WeatherSystem
{
    [CreateAssetMenu(menuName = "Weather System/Weather Event")]
    public class WeatherEvent : ScriptableObject
	{
        [Header("Identifier")]
        [SerializeField]
        private WeatherTypes weatherType;

        [Header("Audio")]
        [SerializeField]
        private AudioClip backgroundSound;
        [SerializeField]
        private AudioClip[] instanceSounds;

        [Header("Visuals")]
        [SerializeField]
        private ParticleSystem particleSystem;
        [SerializeField]
        private Shader shader;
    }
}