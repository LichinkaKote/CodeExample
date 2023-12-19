using Assets.Scripts.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public class Stats
    {
        protected const float MAX_MULT_VALUE = 20f;
        public event Action death;

        public float SpeedMult { get; protected set; } = 1f;
        public float HealthMult { get; protected set; } = 1f;
        public float Health { get; protected set; }
        public float MaxHealth { get; protected set; }
        public float MoveSpeed { get; protected set; }
        public IResistance resistance { get; protected set; }

        protected virtual float BaseMaxHealth { get; set; }
        protected virtual float BaseMoveSpeed { get; set; }

        private Dictionary<uint, float> healthMults = new Dictionary<uint, float>();
        private Dictionary<uint, float> speedMults = new Dictionary<uint, float>();

        public Stats(float maxHealth, float speed, IResistance resistance)
        {
            BaseMaxHealth = maxHealth;
            BaseMoveSpeed = speed;
            UpdateMaxHealth();
            Health = BaseMaxHealth;
            UpdateMoveSpeed();
            this.resistance = resistance;
        }
        public virtual IDamage DoDamage(IDamage incDamage)
        {
            var damage = GetDamage(incDamage, resistance);
            Health = Mathf.Clamp(Health - damage.TotalDamage, 0f, MaxHealth);
            if (Health <= 0f)
            {
                death?.Invoke();
            }
            return damage;
        }
        protected void UpdateMoveSpeed()
        {
            MoveSpeed = BaseMoveSpeed * SpeedMult;
        }
        protected virtual void UpdateMaxHealth()
        {
            MaxHealth = BaseMaxHealth * HealthMult;
        }
        public void AddSpeedDebuff(uint debuffID, float value)
        {
            speedMults.Add(debuffID, value);
            var minValue = GetMinValue(speedMults);
            SpeedMult = Mathf.Clamp(minValue, 0f, MAX_MULT_VALUE);
            UpdateMoveSpeed();
        }
        public void AddHealthDebuff(uint debuffID, float value)
        {
            healthMults.Add(debuffID, value);
            var minValue = GetMinValue(healthMults);
            HealthMult = Mathf.Clamp(minValue, 0f, MAX_MULT_VALUE);
            MaxHealth = Mathf.Clamp(HealthMult * BaseMaxHealth, 1f, float.MaxValue);
        }
        public void RemoveSpeedDebuff(uint debuffID)
        {
            speedMults.Remove(debuffID);
            var minValue = GetMinValue(speedMults);
            SpeedMult = Mathf.Clamp(minValue, 0f, MAX_MULT_VALUE);
            UpdateMoveSpeed();
        }
        public void RemoveHealthDebuff(uint debuffID)
        {
            healthMults.Remove(debuffID);
            var minValue = GetMinValue(healthMults);
            HealthMult = Mathf.Clamp(minValue, 0f, MAX_MULT_VALUE);
            MaxHealth = Mathf.Clamp(HealthMult * BaseMaxHealth, 1f, float.MaxValue);
        }
        private float GetMinValue(Dictionary<uint, float> dict)
        {
            if (dict.Count == 0) return 1f;
            float minValue = dict.Values.First();
            foreach (var item in dict)
            {
                if (item.Value < minValue)
                    minValue = item.Value;
            }
            return minValue;
        }

        private IDamage GetDamage(IDamage incDamage, IResistance resistance)
        {
            var outDamage = new Damage();
            outDamage.PhysicalDamage += incDamage.PhysicalDamage * Mathf.Clamp(1f - resistance.PhysicalResistance, 0f, float.MaxValue);
            outDamage.PoisonDamage += incDamage.PoisonDamage * Mathf.Clamp(1f - resistance.PoisonResistance, 0f, float.MaxValue);
            outDamage.FireDamage += incDamage.FireDamage * Mathf.Clamp(1f - resistance.FireResistance, 0f, float.MaxValue);
            outDamage.FrostDamage += incDamage.FrostDamage * Mathf.Clamp(1f - resistance.FrostResistance, 0f, float.MaxValue);
            outDamage.LightningDamage += incDamage.LightningDamage * Mathf.Clamp(1f - resistance.LightningResistance, 0f, float.MaxValue);
            outDamage.PureDamage += incDamage.PureDamage;
            return outDamage;
        }
    }
}