using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Windows
{
    public class LevelCompleteWindow : Window
    {
        [SerializeField] private Button menuBtn;
        [SerializeField] private Button nextBtn;

        public void OnNextClick(Action action)
        {
            AfterClose(action);
            nextBtn.onClick.RemoveAllListeners();
            nextBtn.onClick.AddListener(Close);
        }
        public void OnMenuClick(Action action)
        {
            menuBtn.onClick.RemoveAllListeners();
            menuBtn.onClick.AddListener(() => AfterClose(null));
            menuBtn.onClick.AddListener(Close);
            menuBtn.onClick.AddListener(action.Invoke);
        }

    }
}