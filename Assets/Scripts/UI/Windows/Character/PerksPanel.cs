using Assets.Scripts.Core;
using Assets.Scripts.Core.PlayerSystems;
using Assets.Scripts.UI.Base;
using UnityEngine;

namespace Assets.Scripts.UI.Windows.Character
{
    public class PerksPanel : MonoBehaviour
    {
        [SerializeField] private Transform content;
        [SerializeField] private ImageTextView perkView;
        [SerializeField] private ImageTextButtonView newPerkBtnPf;
        private PlayerPerks Perks => Game.Player.Perks;
        private void Awake()
        {
            UpdateUI();
        }
        private void UpdateUI()
        {
            content.RemoveAllChilds();
            UpdatePerks();
            UpdateNewPerkBtn();
        }
        private void UpdatePerks()
        {
            var perks = Perks.LearnedPerks;
            foreach (var perk in perks)
            {
                var inst = Instantiate(perkView, content);
                inst.Set(perk.Icon, perk.Name);
            }
        }
        private void UpdateNewPerkBtn()
        {
            if (Perks.HaveFreePerkPoints && Perks.HaveUnlearnedPerks)
            {
                var inst = Instantiate(newPerkBtnPf, content);
                inst.OnClick(() =>
                {
                    var wnd = Game.WindowController.Open<LearnPerkWindow>(false);
                    wnd.OnClose(UpdateUI);
                });
            }
        }
    }
}