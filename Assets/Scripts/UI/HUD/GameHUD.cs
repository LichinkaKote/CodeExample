using Assets.Scripts.Core;
using Assets.Scripts.Core.PlayerSystems;
using Assets.Scripts.UI.Windows.Character;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.HUD
{
    public class GameHUD : MonoBehaviour
    {
        [SerializeField] private ActionBar actionBar;
        [SerializeField] private TMP_Text mapName, playerLvl;
        [SerializeField] private MinMaxBar healthBar, expBar;
        [SerializeField] private Button levelupBtn;
        [SerializeField] private GameObject lvlupNotify;

        private PalyerPersistentSystems playerPesistent;
        private PauseManager PauseManager => Game.GameManager.PauseManager;
        public ActionBar ActionBar => actionBar;

        public void Init(Player player)
        {
            playerPesistent = Game.Player;
            actionBar.Init(player, playerPesistent.Inventory);
            Game.GameManager.LevelInputListener.numericKey.Subscribe(value => actionBar.Select(value)).AddTo(this);
            mapName.text = Game.GameManager.CurrentLevel.Name;
            InitHealthBar(player);
            InitExpBar(playerPesistent);
            playerPesistent.Experience.levelUp += OnLevelUp;
            OnLevelUp(playerPesistent.Experience.Level);
            levelupBtn.onClick.AddListener(ShowCharacterWindow);
            UpdateLevelupBtn();
            Game.Player.Attributes.LearningPoints.Subscribe((p)=> UpdateLevelupBtn()).AddTo(this);
            Game.Player.Perks.FreePerkPoints.Subscribe((p) => UpdateLevelupBtn()).AddTo(this);
        }

        private void InitHealthBar(Player player)
        {
            player.Health.Subscribe((health) =>
            {
                healthBar.SetMinValue(health);
            }).AddTo(this);
            player.MaxHealth.Subscribe((health) =>
            {
                healthBar.SetMaxValue(health);
                healthBar.SetMinValue(player.Health.Value);
            }).AddTo(this);
        }

        private void InitExpBar(PalyerPersistentSystems player)
        {
            player.Experience.Experience.Subscribe((exp) =>
            {
                expBar.SetMaxValue(player.Experience.ExpToNextLevel);
                expBar.SetMinValue(exp);
            }).AddTo(this);
        }
        private void OnLevelUp(short lvl)
        {
            playerLvl.text = lvl.ToString();
        }
        private void ShowCharacterWindow()
        {
            PauseManager.Pause(true);
            var charWind = Game.WindowController.Open<CharacterWindow>();
            charWind.OnClose(() => 
            {
                PauseManager.Pause(false);
            });
        }
        private void UpdateLevelupBtn()
        {
            bool showWarning = Game.Player.Attributes.LearningPoints.Value > 0 || 
                Game.Player.Perks.HaveFreePerkPoints && Game.Player.Perks.HaveUnlearnedPerks;

            lvlupNotify.SetActive(showWarning);
        }
        private void OnDestroy()
        {
            playerPesistent.Experience.levelUp -= OnLevelUp;
        }
    }
}