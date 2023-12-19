namespace Assets.Scripts.Core.Interfaces
{
    public interface IRangedWeaponModifier : IWeaponModifier
    {
        public float ReloadMod { get; }
        public short MagazineSizeMod { get; }
        public float InacuracyMod { get; }
        public float RangedDamageMod { get; }
    }
}