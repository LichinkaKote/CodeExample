using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Windows
{
    public class LevelMenuWindow : Window
    {
        [SerializeField] private Button exitBtn;

        private void Awake()
        {
            exitBtn.onClick.AddListener(OnExitClick);
        }

        private void OnExitClick()
        {
            Close();
            Game.SceneLoader.LoadMainMenu();
        }
    }
}