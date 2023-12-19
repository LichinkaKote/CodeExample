using Assets.Scripts.UI.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Base
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private Button startBtn;

        private void Awake()
        {
            startBtn.onClick.AddListener(StartGame);
        }

        private void StartGame()
        {
            startBtn.enabled = false;
            Game.WindowController.Open<LevelHubWindow>();
            //Game.GameManager.StartGame(Game.Library.LevelLib.GetFirstLevel().ID);
        }
    }
}