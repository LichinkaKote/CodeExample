using Assets.Scripts.Core.Data;
using Assets.Scripts.Core.Interfaces;

namespace Assets.Scripts.Core.Items
{
    public class RangedWeaponMod : WeaponMod, IRangedWeaponModifier
    {
        public RangedWeaponMod(RangedWeaponModData data) : base(data)
        {
            if (data.inacuracyMod.HasValue) InacuracyMod = data.inacuracyMod.Value;
            if (data.magazineSizeMod.HasValue) MagazineSizeMod = data.magazineSizeMod.Value;
            if (data.reloadMod.HasValue) ReloadMod = data.reloadMod.Value;
            if (data.rangedDamageMod.HasValue) RangedDamageMod = data.rangedDamageMod.Value;
        }

        public float ReloadMod { get; private set; }
        public short MagazineSizeMod { get; private set; }
        public float InacuracyMod { get; private set; }
        public float RangedDamageMod { get; private set; }
    }
}