using Assets.Scripts.Core.Data;
using Assets.Scripts.Core.Enums;
using Assets.Scripts.Core.Interfaces;

namespace Assets.Scripts.Core.Items
{
    public abstract class Weapon : Item, IModifyableItem
    {
        public float FireRate { get; private set; }
        public IDamage Damage { get; protected set; }
        public WeaponType Type { get; private set; }
        public ItemMod[] Mods { get; private set; }

        private float baseFireRate;
        protected IDamage baseDamage;

        public Weapon(WeaponData data) : base(data)
        {
            baseFireRate = data.fireRate;
            Type = (WeaponType)data.type;
            Mods = new ItemMod[Quality + 1];
            FireRate = baseFireRate;
            baseDamage = data.damage.GetDamage();
            Damage = baseDamage;
        }
        public virtual void ApplyMods()
        {
            float bonusFR = 0f;
            for (int i = 0; i < Mods.Length; i++)
            {
                if (Mods[i] is IWeaponModifier mod)
                {
                    bonusFR += mod.AttackSpeedMod;
                }
            }
            FireRate = baseFireRate + bonusFR;
        }

        public virtual bool IsCompatible(ItemMod mod)
        {
            if (mod is WeaponMod wm)
            {
                if (wm.ApplyType == WeaponType.Everything || wm.ApplyType == Type)
                    return true;
            }
            return false;
        }
    }
}