namespace Assets.Scripts.Core.Data
{
    public class RangedWeaponData : WeaponData
    {
        public bool isAutomatic;
        public float inacuracy;
        public ushort projectileID;
        public ushort penetrationForce;
        public float projectileVelocity;
        public float projectileLifetime;
        public byte? pellets;
        public float ragdoll;
        public int magazineSize;
        public float reload;
    }
}