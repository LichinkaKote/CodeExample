using UnityEngine;

namespace Assets.Scripts.Core
{
    public class PauseManager
    {
        public PauseManager()
        {
            Pause(false);
        }
        public void Pause(bool value)
        {
            Time.timeScale = value ? 0 : 1;
            Game.Events.InvokePause(value);
        }
    }
}