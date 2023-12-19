using Assets.Scripts.Core.Interfaces;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.Core.PlayerSystems
{
    public class PlayerStats : Stats
    {
        public ReactiveProperty<float> HealthReact { get; private set; } = new ReactiveProperty<float>();
        public ReactiveProperty<float> MaxHealthReact { get; private set; } = new ReactiveProperty<float>();

        private List<IStatModifier> statModifiers;
        private float maxHealthMod = 0f;
        private float moveSpeedMod = 1f;
        private float healthRegen;
        protected override float BaseMaxHealth { get => base.BaseMaxHealth + maxHealthMod; set => base.BaseMaxHealth = value; }
        protected override float BaseMoveSpeed { get => base.BaseMoveSpeed * moveSpeedMod; set => base.BaseMoveSpeed = value; }
        public PlayerStats(float maxHealth, float speed, IResistance resistance) : base(maxHealth, speed, resistance)
        {
            HealthReact.Value = BaseMaxHealth;
            MaxHealthReact.Value = BaseMaxHealth;
        }
        public override IDamage DoDamage(IDamage value)
        {
            var dmg = base.DoDamage(value);
            HealthReact.Value = Health;
            return dmg;
        }

        public void SetStatModifiers(List<IStatModifier> mods)
        {
            statModifiers = mods;
            CalculateMods();
            UpdateMoveSpeed();
            UpdateMaxHealth();
        }
        private void CalculateMods()
        {
            var healthBonus = 0f;
            var speedBonus = 0f;
            var regen = 0f;
            foreach (var item in statModifiers)
            {
                healthBonus += item.HealthMod;
                speedBonus += item.MoveSpeedMod;
                regen += item.RegenMod;
            }
            maxHealthMod = healthBonus;
            moveSpeedMod = Mathf.Clamp(1f + speedBonus, 0f, MAX_MULT_VALUE);
            healthRegen = regen;
        }
        protected override void UpdateMaxHealth()
        {
            base.UpdateMaxHealth();
            MaxHealthReact.Value = MaxHealth;
        }
        public void Update(float deltaTime)
        {
            Health = Mathf.Clamp(Health + healthRegen * deltaTime, 0f, MaxHealth);
            HealthReact.Value = Health;
        }
    }
}