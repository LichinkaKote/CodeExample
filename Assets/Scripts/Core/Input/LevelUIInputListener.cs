using Assets.Scripts.UI.Windows;
using UnityEngine;

namespace Assets.Scripts.Core.Input
{
    public class LevelUIInputListener : MonoBehaviour
    {
        private WindowController controller => Game.WindowController;

        private void Update()
        {
            ExitHandle();
        }
        private void ExitHandle()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
            {
                if (controller.IsEmpty)
                {
                    Game.GameManager.PauseManager.Pause(true);
                    controller.Open<LevelMenuWindow>()
                        .OnClose(() => Game.GameManager.PauseManager.Pause(false));
                }
                else
                    controller.CloseTopWindow();
            }
        }
    }
}