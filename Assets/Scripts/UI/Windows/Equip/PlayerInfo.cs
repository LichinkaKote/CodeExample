using Assets.Scripts.Core;
using Assets.Scripts.Core.PlayerSystems;
using Assets.Scripts.UI.Base;
using System;
using UnityEngine;

namespace Assets.Scripts.UI.Windows.Equip
{
    public class PlayerInfo : MonoBehaviour
    {
        [SerializeField] private ImageTextButtonView textPf;
        [SerializeField] private Transform leftCol, rightCol;

        private PlayerStatistics stat;
        public void Init(PlayerStatistics playerStatistics)
        {
            stat = playerStatistics;
            UpdatePlayerInfo();
        }
        public void UpdatePlayerInfo()
        {
            leftCol.RemoveAllChilds();
            rightCol.RemoveAllChilds();

            AddStat($"Health: {stat.Helalth}", leftCol);
            AddStat($"Health regen: {stat.HelalthRegen}", leftCol);
            AddStat($"Move Speed: {stat.MoveSpeed}", leftCol);

            var res = stat.Resistance;
            var pcMod = 100f;
            AddStat($"Physical res: {res.PhysicalResistance * pcMod}%", rightCol);
            AddStat($"Poison res: {res.PoisonResistance * pcMod}%", rightCol);
            AddStat($"Fire res: {res.FireResistance * pcMod}%", rightCol);
            AddStat($"Frost res: {res.FrostResistance * pcMod}%", rightCol);
            AddStat($"Lightning res: {res.LightningResistance * pcMod}%", rightCol);
        }
        private void AddStat(string text, Transform content, Action click = null)
        {
            var inst = Instantiate(textPf, content);
            inst.SetText(text);
            if (click != null)
            {
                inst.SetButtonGameobjectVisibility(true);
                inst.OnClick(click.Invoke);
            }
        }
    }
}