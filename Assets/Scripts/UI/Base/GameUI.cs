using Assets.Scripts.Core.PlayerSystems;
using Assets.Scripts.UI.HUD;
using UnityEngine;

namespace Assets.Scripts.UI.Base
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private GameHUD gameHUD;

        //public GameHUD GameHUD => gameHUD;
        //public Transform Windows => windows;
        //public Transform TopLayer => topLayer;
        private DamagePopUpController damagePopUpController;
        public void Init(Player player, Transform worldUi)
        {
            gameHUD.Init(player);
            damagePopUpController = GetComponent<DamagePopUpController>();
            damagePopUpController.Init(worldUi);
        }
    }
}