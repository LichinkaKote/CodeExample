using Assets.Scripts.UI.HUD;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Windows.Character
{
    public class AttributeBar : MonoBehaviour
    {
        [SerializeField] private TMP_Text attrName;
        [SerializeField] private MinMaxBar slider;
        [SerializeField] private Image attrIcon;
        [SerializeField] private Button plusBtn;

        public void SetMaxValue(float maxValue, int digits = default, string customText = default)
        {
            slider.SetMaxValue(maxValue, digits, customText);
        }
        public void SetValue(float value, int digits = default, string customText = default)
        {
            slider.SetMinValue(value, digits, customText);
        }
        public void SetName(string name)
        {
            attrName.text = name;
        }
        public void SetIcon(Sprite sprite)
        {
            attrIcon.sprite = sprite;
        }
        public void OnClick(Action action)
        {
            plusBtn.onClick.RemoveAllListeners();
            plusBtn.onClick.AddListener(action.Invoke);
        }
        public void DisablePlusBtn(bool isDisabled)
        {
            plusBtn.interactable = !isDisabled;
        }
    }
}