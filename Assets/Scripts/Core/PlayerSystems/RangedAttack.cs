using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Items;
using System;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.Core.PlayerSystems
{
    public class RangedAttack : IWepContrlollerAttackPreset, IWeaponReloable
    {
        private RangedWeapon weapon;
        private WeaponController controller;
        private Func<bool> attack;
        private PlayerModifiers mods;

        private float reloadMod;
        private float inacuracyMod;
        private float rangedDmgMod;
        private IDamage damage;

        private float cooldown;
        private bool IsOnCD => cooldown > 0f;
        private bool IsOnReload => ReloadProgress.Value > 0f;
        private bool HaveAmmo => Ammo.Value > 0;
        private bool AmmoIsFull => Ammo.Value == weapon.MagazineSize;

        public ReactiveProperty<int> Ammo { get; private set; } = new ReactiveProperty<int>();
        public ReactiveProperty<float> ReloadProgress { get; private set; } = new ReactiveProperty<float>();
        public float ReloadTime => weapon.Reload * Mathf.Clamp(1 - reloadMod, 0f, float.MaxValue);



        private bool isReadyToShoot = true;
        public RangedAttack(WeaponController controller, RangedWeapon wep, PlayerModifiers mods)
        {
            this.mods = mods;
            weapon = wep;
            Ammo.Value = weapon.MagazineSize;
            damage = weapon.Damage;
            this.controller = controller;
            attack = weapon.IsAutomatic ? AutomaticShoot : ManualShoot;
            mods.modsUpdated += UpdateMods;
            UpdateMods();
        }
        public bool Attack()
        {
            if (attack.Invoke())
            {
                ShootProjectileToTarget();
                return true;
            }
            var delta = Time.deltaTime;
            ReduceCooldown(delta);
            ReduceReload(delta);
            return false;
        }
        private bool AutomaticShoot()
        {
            if (Game.GameManager.LevelInputListener.Button0)
            {
                StartReloadIfNoAmmo();
                if (!IsOnCD && HaveAmmo && !IsOnReload)
                {
                    IncreseCooldownByFireRate(weapon.FireRate);
                    Ammo.Value -= 1;
                    return true;
                }
            }
            return false;
        }
        private bool ManualShoot()
        {
            bool result = false;
            if (Game.GameManager.LevelInputListener.ButtonDown0)
            {
                StartReloadIfNoAmmo();
                if (isReadyToShoot && !IsOnCD && HaveAmmo && !IsOnReload)
                {
                    IncreseCooldownByFireRate(weapon.FireRate);
                    Ammo.Value -= 1;
                    isReadyToShoot = false;
                    result = true;
                }
            }
            if (Game.GameManager.LevelInputListener.ButtonUp0)
            {
                isReadyToShoot = true;
            }
            return result;
        }
        private void IncreseCooldownByFireRate(float fireRate)
        {
            cooldown += 60f / fireRate;
        }
        private void ReduceCooldown(float deltaTime)
        {
            if (IsOnCD)
                cooldown -= deltaTime;
        }
        private void ShootProjectileToTarget()
        {
            var from = controller.firePoint.position;
            var debuffs = Game.GameManager.Player.AttackDebuffs;
            int projectileID = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
            for (int i = 0; i < weapon.Pellets; i++)
            {
                var bullet = UnityEngine.Object.Instantiate(Game.Prefabs.Projectile, Game.GameManager.World);
                bullet.PenetrationForce = weapon.PenetrationForce;
                bullet.Velocity = weapon.ProjectileVelocity;
                bullet.LifeTime = weapon.ProjectileLifetime;
                bullet.ProjectileID = projectileID;
                bullet.Ragdoll = weapon.Ragdoll;
                bullet.Debuffs = debuffs;
                bullet.transform.position = from;
                var inac = weapon.Inacuracy * Mathf.Clamp(1 - inacuracyMod, 0f, float.MaxValue);
                bullet.Fire(weapon.GetInacuracyDirection((Vector3)controller.Target - from, inac), damage);
            }
        }

        public void Reload()
        {
            if (!IsOnReload && !AmmoIsFull)
                ReloadProgress.Value = ReloadTime;
        }
        private void ReduceReload(float deltaTime)
        {
            if (IsOnReload)
            {
                ReloadProgress.Value -= deltaTime;
                if (!IsOnReload)
                    Ammo.Value = weapon.MagazineSize;
            }
        }
        private void StartReloadIfNoAmmo()
        {
            if (!IsOnReload && !HaveAmmo)
                Reload();
        }
        private void UpdateMods()
        {
            var rangedMods = mods.RangedWeaponMods;
            reloadMod = 0f;
            inacuracyMod = 0f;
            rangedDmgMod = 0f;
            foreach (var mod in rangedMods)
            {
                inacuracyMod += mod.InacuracyMod;
                reloadMod += mod.ReloadMod;
                rangedDmgMod += mod.RangedDamageMod;
            }
            damage = weapon.Damage.Multiply(1 + rangedDmgMod);
        }
        ~RangedAttack()
        {
            mods.modsUpdated -= UpdateMods;
        }
    }
}