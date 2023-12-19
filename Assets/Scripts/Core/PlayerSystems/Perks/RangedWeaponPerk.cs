using Assets.Scripts.Core.Enums;
using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Passives;
using System.Collections.Generic;

namespace Assets.Scripts.Core.PlayerSystems.Perks
{
    public class RangedWeaponPerk : PlayerPerk, IRangedWeaponModifier
    {

        public float ReloadMod { get; set; }
        public short MagazineSizeMod { get; set; }
        public float InacuracyMod { get; set; }
        public float AttackSpeedMod { get; set; }
        public List<Debuff> Debuffs { get; set; }
        public WeaponType ApplyType { get; set; }
        public float RangedDamageMod { get; set; }
    }
}