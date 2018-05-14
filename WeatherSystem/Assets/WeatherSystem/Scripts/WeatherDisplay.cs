using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
namespace WeatherSystem
{
    public class WeatherDisplay : MonoBehaviour
    {
        [SerializeField]
        private Text windOutput;

        [SerializeField]
        private Text tempOutput;

        [SerializeField]
        private Text humOutput;

        [SerializeField]
        private Text weatherTypeOutput;

        [SerializeField]
        private Text intensityOutput;

        [SerializeField]
        private Transform trackedTransform;

        [SerializeField]
        private WeatherManager weatherManager;

        private void Awake()
        {
            if(weatherManager == null)
            {
                weatherManager = FindObjectOfType<WeatherManager>();
            }
        }


        private void Update()
        {
            Vector2 windInstance, windTracked;
            float intensity;
            float temp;
            float hum;
            WeatherTypes weatherType;

            Vector2 location = new Vector2(trackedTransform.position.x, trackedTransform.position.z);

            windInstance = weatherManager.GetWindValueAt(location);
            windTracked = weatherManager.GetCumulativeWind();

            intensity = weatherManager.GetIntensityValueAt(location);

            temp = weatherManager.GetTemperatureValueAt(location);
            TemperatureVariables tempEnum = temp.ToTemperatureValue();

            hum = weatherManager.GetHumidityValueAt(location);
            HumidityVariables humEnum = hum.ToHumidityValue();

            weatherType = weatherManager.GetWeather(location);

            Output("Wind:", new object[] { windInstance, windTracked }, windOutput);
            Output("Intensity:", new object[] { intensity }, intensityOutput);
            Output("Temp:", new object[] { tempEnum, temp }, tempOutput);
            Output("Hum:", new object[] { humEnum, hum }, humOutput);
            Output("Weather:", new object[] { weatherType }, weatherTypeOutput);
        }

        private void Output(string preamble, object[] values, Text display)
        {
            display.text = preamble + " " + values.Aggregate((x,y) => x.ToString() + "," + y.ToString());
        }
    }
}
