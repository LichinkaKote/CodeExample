using Assets.Scripts.Core.Enums;
using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Passives;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.Core.PlayerSystems
{
    public class PlayerAttributes : IRangedWeaponModifier, IStatModifier
    {
        public const ushort MAX_ATTRIBUTE_VALLUE = 10;
        public const float BASE_HEALTH = 100f;
        public const float BASE_MOVE_SPD = 2.5f;
        public ReactiveProperty<short> Strength { get; private set; } = new ReactiveProperty<short>();
        public ReactiveProperty<short> Stamina { get; private set; } = new ReactiveProperty<short>();
        public ReactiveProperty<short> Perception { get; private set; } = new ReactiveProperty<short>();
        public ReactiveProperty<short> Agility { get; private set; } = new ReactiveProperty<short>();
        public ReactiveProperty<short> LearningPoints { get; private set; } = new ReactiveProperty<short>();

        public event Action statChanged;

        //===Interfaces======
        #region IRangedWeaponModifier
        public float ReloadMod => Agility.Value * 0.05f;
        public short MagazineSizeMod => 0;
        public float InacuracyMod => Perception.Value * pecperceptionAccuracyMult;
        public float AttackSpeedMod => 0;
        public List<Debuff> Debuffs { get; }
        public WeaponType ApplyType { get; }
        public float RangedDamageMod => Perception.Value * pecperceptionDmgMult;
        #endregion

        #region IStatModifier
        public float HealthMod => Stamina.Value * staminaHealthMult;
        public float MoveSpeedMod => speedMult * Agility.Value;
        public float RegenMod => staminaRegenMult * Stamina.Value;
        #endregion
        //==================

        //private float strengthDmgMult = 1.5f;
        private float pecperceptionAccuracyMult = 0.05f;
        private float pecperceptionDmgMult = 0.05f;
        private float speedMult = 0.05f;
        private float staminaHealthMult = 15f;
        private float staminaRegenMult = 0.5f;

        public void AddLP(short count = 1)
        {
            LearningPoints.Value += count;
        }
        public void AddStrength()
        {
            if (LearningPoints.Value <= 0) return;
            Set(Strength, (short)(Strength.Value + 1));
        }
        public void AddStamina()
        {
            if (LearningPoints.Value <= 0) return;
            Set(Stamina, (short)(Stamina.Value + 1));
        }
        public void AddPerception()
        {
            if (LearningPoints.Value <= 0) return;
            Set(Perception, (short)(Perception.Value + 1));
        }
        public void AddAgility()
        {
            if (LearningPoints.Value <= 0) return;
            Set(Agility, (short)(Agility.Value + 1));
        }
        private void Set(ReactiveProperty<short> attribute, short value)
        {
            var delta = (short)(attribute.Value - value);
            LearningPoints.Value += delta;
            attribute.Value = (short)Mathf.Clamp(value, 0, MAX_ATTRIBUTE_VALLUE);
            statChanged?.Invoke();
        }
    }
}