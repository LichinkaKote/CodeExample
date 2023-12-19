using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Base
{
    public class ImageTextButtonView : ImageTextView
    {
        [SerializeField] private Button btn;

        public void OnClick(Action action)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(action.Invoke);
        }
        public void AddOnClick(Action action)
        {
            btn.onClick.AddListener(action.Invoke);
        }
        public void InvokeClick()
        {
            btn.onClick.Invoke();
        }
        public void SetButtonGameobjectVisibility(bool visible)
        {
            btn.gameObject.SetActive(visible);
        }
    }
}