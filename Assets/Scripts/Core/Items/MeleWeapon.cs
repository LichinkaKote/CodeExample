using Assets.Scripts.Core.Data;

namespace Assets.Scripts.Core.Items
{
    public class MeleWeapon : Weapon
    {
        public float AttackArc { get; private set; }
        public float Range { get; private set; }
        public MeleWeapon(MeleWeaponData data) : base(data)
        {
            AttackArc = data.attackArc;
            Range = data.range;
        }
        public override bool IsCompatible(ItemMod mod)
        {
            return base.IsCompatible(mod) && mod is not RangedWeaponMod;
        }
    }
}