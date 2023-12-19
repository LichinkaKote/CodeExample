using Assets.Scripts.Core;
using Assets.Scripts.Core.Data;
using Assets.Scripts.Core.Enums;
using Assets.Scripts.UI.Base;
using Assets.Scripts.UI.Windows.EnemyTooltip;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Windows
{
    public class EnemyTooltipWindow : Window
    {
        [SerializeField] private ImageTextView header;
        [SerializeField] private TMP_Text desc;
        [SerializeField] private GameObject abilities;
        [SerializeField] private Transform abilitiesContent;
        [SerializeField] private ImageTextButtonView abilityPf;
        [SerializeField] private EnemyStatPanel stats, damage, resist;
        private SpriteLib SpriteLib => Game.Library.SpriteLib;
        private DebuffsLib Debuffs => Game.Library.DebuffsLib;
        public void Init(EnemyData enemy)
        {
            header.Set(enemy.Icon, enemy.name);
            desc.SetText(enemy.desc);
            ShowAbilities(enemy);
            ShowInfo(enemy);
        }
        private void ShowInfo(EnemyData enemy)
        {
            var mass = enemy.mass.HasValue ? enemy.mass.Value : 1f;
            stats.AddStat($"Health: {enemy.health}", SpriteLib.UIicons[(int)UIicons.Plus], Color.red);
            stats.AddStat($"Speed: {enemy.moveSpeed}", SpriteLib.UIicons[(int)UIicons.Shoe]);
            stats.AddStat($"Mass: {mass}Kg", SpriteLib.UIicons[(int)UIicons.Mass]);

            var dmg = enemy.attackDamage;
            var phys = dmg.physicalDmg.HasValue ? dmg.physicalDmg.Value : 0;
            var poi = dmg.poisonDmg.HasValue ? dmg.poisonDmg.Value : 0;
            var fir = dmg.firelDmg.HasValue ? dmg.firelDmg.Value : 0;
            var fro = dmg.frostDmg.HasValue ? dmg.frostDmg.Value : 0;
            var lig = dmg.lightningDmg.HasValue ? dmg.lightningDmg.Value : 0;
            damage.AddStat($"Physical: {phys}", SpriteLib.UIicons[(int)UIicons.PhysicalDamage]);
            damage.AddStat($"Poison: {poi}", SpriteLib.UIicons[(int)UIicons.PoisonDamage]);
            damage.AddStat($"Fire: {fir}", SpriteLib.UIicons[(int)UIicons.FireDamage]);
            damage.AddStat($"Frost: {fro}", SpriteLib.UIicons[(int)UIicons.FrostDamage]);
            damage.AddStat($"Lightning: {lig}", SpriteLib.UIicons[(int)UIicons.LightningDamage]);

            var res = enemy.DamageResistance;
            var rphys = res.physRes.HasValue ? res.physRes.Value : 0;
            var rpoi = res.poisonRes.HasValue ? res.poisonRes.Value : 0;
            var rfir = res.fireRes.HasValue ? res.fireRes.Value : 0;
            var rfro = res.frostRes.HasValue ? res.frostRes.Value : 0;
            var rlig = res.lightningRes.HasValue ? res.lightningRes.Value : 0;
            float pecent = 100f;
            resist.AddStat($"Physical: {rphys * pecent}%", SpriteLib.UIicons[(int)UIicons.PhysicalDamage]);
            resist.AddStat($"Poison: {rpoi * pecent}%", SpriteLib.UIicons[(int)UIicons.PoisonDamage]);
            resist.AddStat($"Fire: {rfir * pecent}%", SpriteLib.UIicons[(int)UIicons.FireDamage]);
            resist.AddStat($"Frost: {rfro * pecent}%", SpriteLib.UIicons[(int)UIicons.FrostDamage]);
            resist.AddStat($"Lightning: {rlig * pecent}%", SpriteLib.UIicons[(int)UIicons.LightningDamage]);
        }

        private void ShowAbilities(EnemyData enemy)
        {
            var debuffs = Debuffs.GetDebuffs(enemy.debuffs);
            bool haveDebuffs = debuffs.Count > 0;
            abilities.SetActive(haveDebuffs);

            if (haveDebuffs)
            {
                foreach (var debuff in debuffs)
                {
                    var inst = Instantiate(abilityPf, abilitiesContent);
                    inst.SetText(debuff.Name);
                    inst.OnClick(() =>
                    {
                        Game.WindowController.Open<DebuffTooltipWindow>(false).Init(debuff);
                    });
                }
            }
        }

    }
}