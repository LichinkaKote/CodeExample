using Assets.Scripts.Core.PlayerSystems.Perks;
using Assets.Scripts.UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Windows
{
    public class LearnPerkWindow : Window
    {
        [SerializeField] private Transform grid;
        [SerializeField] private ImageTextButtonView perkPf;
        [SerializeField] private Button OkBtn;

        private PlayerPerk selectedPerk;
        private void Awake()
        {
            UpdatePerks();
            OkBtn.onClick.AddListener(() =>
            {
                if (selectedPerk != null)
                {
                    Game.Player.Perks.LearnPerk(selectedPerk);
                    Close();
                }
            });
        }

        private void UpdatePerks()
        {
            var unlearnedPerks = Game.Player.Perks.UnlearnedPerks;
            foreach (var perk in unlearnedPerks)
            {
                var inst = Instantiate(perkPf, grid);
                inst.Set(perk.Icon, perk.Name);
                inst.OnClick(() => selectedPerk = perk);
            }
        }
    }
}