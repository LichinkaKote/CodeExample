using Assets.Scripts.Core.Enums;
using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Passives;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.PlayerSystems
{
    public class PlayerSkills : IRangedWeaponModifier
    {
        public event Action skillChanged;
        public float MaxSkill => 100f;
        public float MeleSkill { get; private set; }
        public float RangedSkill { get; private set; }

        public float ReloadMod => RangedSkill * 0.001f;
        public short MagazineSizeMod { get; private set; }
        public float InacuracyMod => RangedSkill * 0.0015f;
        public float RangedDamageMod => RangedSkill * 0.005f;
        public float AttackSpeedMod { get; private set; }
        public List<Debuff> Debuffs { get; private set; }
        public WeaponType ApplyType { get; private set; }

        private const float MELE_XP_CAP = 1000f;
        private const float RANGED_XP_CAP = 1000f;
        private float meleCombatExp;
        private float rangedCombatExp;
        private float xpMult = 0.02f;
        private float prevRangeSkill;
        private int checkStep = 1;
        public PlayerSkills()
        {
            Game.Events.actorHit += OnEnemyHit;
            prevRangeSkill = Mathf.FloorToInt(RangedSkill);
        }

        private void OnEnemyHit(DamageInfo inf)
        {
            if (inf.HitInfo is IRangedHitInfo)
                AddRangedXP(inf.Damage.TotalDamage);
            else
                AddMeleXP(inf.Damage.TotalDamage);
        }

        private void AddMeleXP(float value)
        {
            meleCombatExp = Mathf.Clamp(meleCombatExp + value * xpMult, 0, MELE_XP_CAP);
            MeleSkill = meleCombatExp / MELE_XP_CAP * MaxSkill;
        }
        private void AddRangedXP(float value)
        {
            rangedCombatExp = Mathf.Clamp(rangedCombatExp + value * xpMult, 0, RANGED_XP_CAP);
            RangedSkill = rangedCombatExp / RANGED_XP_CAP * MaxSkill;
            CheckSkillIncrease();
        }

        private void CheckSkillIncrease()
        {
            if (RangedSkill - prevRangeSkill >= checkStep)
            {
                prevRangeSkill += checkStep;
                skillChanged?.Invoke();
            }
        }
    }
}