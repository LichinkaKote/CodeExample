using Assets.Scripts.Core.Enums;
using RSG;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Core
{
    public class SceneLoader
    {
        public Promise LoadGameScene()
        {
            return LoadScene(Scenes.GameScene);
        }
        public Promise LoadLoadingScene()
        {
            return LoadScene(Scenes.Loading);
        }
        public Promise LoadMainMenu()
        {
            return LoadScene(Scenes.MainMenu);
        }
        private Promise LoadScene(Scenes scene)
        {
            var result = new Promise();
            SceneManager.LoadSceneAsync(scene.ToString()).completed += (h) =>
            {
                result.Resolve();
            };
            return result;
        }
    }
}