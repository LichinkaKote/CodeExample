using Assets.Scripts.Core;
using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Items;
using Assets.Scripts.UI.Base;
using System;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Windows.Equip
{
    public class ItemInfo : MonoBehaviour
    {
        [SerializeField] private TMP_Text itemName, itemDesc;
        [SerializeField] private ImageTextButtonView textPf;
        [SerializeField] private Transform leftCol, rightCol;

        private Item currentItem;

        public void UpdateItemInfo(Item item)
        {
            currentItem = item;
            itemName.text = item.Name;
            itemDesc.text = item.Description;
            ShowItemStats(item);
        }
        public void UpdateCurrentItemInfo()
        {
            if (currentItem != null)
                UpdateItemInfo(currentItem);
        }
        private void ShowItemStats(Item item)
        {
            leftCol.RemoveAllChilds();
            rightCol.RemoveAllChilds();

            if (item is Weapon wep)
                ShowWeapon(wep);
            else if (item is Armor armor)
                ShowArmor(armor);
            else if (item is ItemMod mod)
                ShowMod(mod);

        }

        private void ShowWeapon(Weapon wep)
        {
            RangedWeapon rwep = null;
            bool isRanged = wep is RangedWeapon;
            AddStat($"Type: {wep.Type.ToDesc()}", leftCol);
            string dmg = "";
            if (isRanged)
            {
                rwep = wep as RangedWeapon;
                dmg = rwep.Pellets > 1 ? $"Dmg: {wep.Damage.TotalDamage}x{rwep.Pellets}" : $"Dmg: {wep.Damage.TotalDamage}";
            }

            AddStat(dmg, leftCol, () =>
            Game.WindowController.Open<DamageTooltipWindow>(false)
            .Init(rwep.Damage, Input.mousePosition));

            AddStat($"Fire rate: {wep.FireRate} rpm", leftCol);
            if (isRanged)
            {
                AddStat($"Inacuracy: {rwep.Inacuracy} deg", leftCol);
                AddStat($"Bullet velocity: {rwep.ProjectileVelocity}", rightCol);
                var automatic = rwep.IsAutomatic ? "Yes" : "No";
                AddStat($"Automatic: {automatic}", rightCol);
                AddStat($"Penetration: {rwep.PenetrationForce}", rightCol);
                AddStat($"Range: {rwep.ProjectileLifetime * rwep.ProjectileVelocity}", rightCol);
            }
        }
        private void ShowArmor(Armor armor)
        {
            float resMod = 100f;
            AddStat($"Physical res: {armor.PhysicalResistance * resMod}%", leftCol);
            AddStat($"Poison res: {armor.PoisonResistance * resMod}%", leftCol);
            AddStat($"Fire res: {armor.FireResistance * resMod}%", leftCol);
            AddStat($"Frost res: {armor.FrostResistance * resMod}%", leftCol);

            AddStat($"Lightning res: {armor.LightningResistance * resMod}%", rightCol);
            AddStat($"Move Speed: {armor.MoveSpeedMult * resMod}%", rightCol);
            AddStat($"Health Regen: {armor.HealthRegen}", rightCol);
        }
        private void ShowMod(ItemMod mod)
        {
            if (mod is IRangedWeaponModifier rangeMod)
            {
                float percentMult = 100f;
                AddStat($"Ranged damage: +{rangeMod.RangedDamageMod * percentMult}%", leftCol);
                AddStat($"Frie rate: +{rangeMod.AttackSpeedMod * percentMult}%", leftCol);
                AddStat($"Inacuracy: -{rangeMod.InacuracyMod * percentMult}%", leftCol);
                AddStat($"Magazine Size: +{rangeMod.MagazineSizeMod}", leftCol);

                AddStat($"Apply to: {rangeMod.ApplyType}", rightCol);
                AddStat($"Reload time: -{rangeMod.ReloadMod * percentMult}%", rightCol);
            }
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