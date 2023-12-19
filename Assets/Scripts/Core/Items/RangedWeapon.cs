using Assets.Scripts.Core.Data;
using Assets.Scripts.Core.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Core.Items
{
    public class RangedWeapon : Weapon
    {
        public bool IsAutomatic { get; private set; }
        public float Inacuracy { get; private set; }
        public ushort ProjectileID { get; private set; }
        public ushort PenetrationForce { get; private set; }
        public float ProjectileVelocity { get; private set; }
        public float ProjectileLifetime { get; private set; }
        public byte Pellets { get; private set; } = 1;
        public float Ragdoll { get; private set; }
        public int MagazineSize { get; private set; }
        public float Reload { get; private set; }

        private float baseReload;
        private int baseMagazineSize;
        private float baseInacuracy;

        public RangedWeapon(RangedWeaponData data) : base(data)
        {
            IsAutomatic = data.isAutomatic;
            ProjectileID = data.projectileID;
            PenetrationForce = data.penetrationForce;
            ProjectileVelocity = data.projectileVelocity;
            ProjectileLifetime = data.projectileLifetime;
            if (data.pellets.HasValue) Pellets = data.pellets.Value;
            Ragdoll = data.ragdoll;
            baseReload = data.reload;
            baseInacuracy = data.inacuracy;
            baseMagazineSize = data.magazineSize;
            Inacuracy = data.inacuracy;
            Reload = data.reload;
            MagazineSize = data.magazineSize;
        }

        public Vector2 GetInacuracyDirection(Vector2 direction, float inacuracy)
        {
            var randomAngle = Random.Range(-inacuracy / 2f, inacuracy / 2f);
            var angle = Quaternion.AngleAxis(randomAngle, Vector3.forward) * direction;
            return angle;
        }
        public override void ApplyMods()
        {
            base.ApplyMods();
            float inacuracyMod = 0f;
            float reloadMod = 0f;
            float bonusDmg = 0f;
            int magSizeMod = 0;
            for (int i = 0; i < Mods.Length; i++)
            {
                if (Mods[i] is IRangedWeaponModifier mod)
                {
                    inacuracyMod += mod.InacuracyMod;
                    reloadMod += mod.ReloadMod;
                    magSizeMod += mod.MagazineSizeMod;
                    bonusDmg += mod.RangedDamageMod;
                }
            }
            Inacuracy = Mathf.Clamp(baseInacuracy * Mathf.Clamp(1 - inacuracyMod, 0f, float.MaxValue), 0f, 180f);
            Reload = baseReload * Mathf.Clamp(1 - reloadMod, 0f, float.MaxValue);
            MagazineSize = baseMagazineSize + magSizeMod;
            Damage = baseDamage.Multiply(1 + bonusDmg);
        }
        public override bool IsCompatible(ItemMod mod)
        {
            return base.IsCompatible(mod) && mod is not MeleWeaponMod;
        }
    }
}