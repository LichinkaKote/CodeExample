using Assets.Scripts.Core.Data;
using Assets.Scripts.Core.Enums;
using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Passives;
using System.Collections.Generic;

namespace Assets.Scripts.Core.Items
{
    public class WeaponMod : ItemMod, IWeaponModifier
    {
        public WeaponMod(WeaponModData data) : base(data)
        {
            if (data.attackSpeedMod.HasValue) AttackSpeedMod = data.attackSpeedMod.Value;
            if (data.applyType.HasValue) ApplyType = (WeaponType)data.applyType.Value;
            Debuffs = Game.Library.DebuffsLib.GetDebuffs(data.debuffs);
        }
        public float AttackSpeedMod { get; private set; }
        public List<Debuff> Debuffs { get; private set; }
        public WeaponType ApplyType { get; private set; }
    }
}