using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.HUD
{
    public class MinMaxBar : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private TMP_Text min, max;

        public void SetMinValue(float value, int digits = default, string customText = default)
        {
            slider.value = value;
            if (customText == default)
                min.text = RoundValue(value, digits);
            else
                min.text = customText;
        }

        public void SetMaxValue(float value, int digits = default, string customText = default)
        {
            slider.maxValue = value;
            if (customText == default)
                max.text = RoundValue(value, digits);
            else
                max.text = customText;
        }
        private string RoundValue(float value, int digits)
        {
            string result;
            if (digits == default)
                result = Mathf.RoundToInt(value).ToString();
            else
                result = Math.Round(value, digits).ToString();
            return result;
        }
    }
}