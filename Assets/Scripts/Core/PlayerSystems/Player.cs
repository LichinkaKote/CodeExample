using Assets.Scripts.Core.Animators;
using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Passives;
using Spine.Unity;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.Core.PlayerSystems
{
    public class Player : MonoBehaviour, IDamagable
    {

        [HideInInspector] public ReactiveProperty<float> Health = new ReactiveProperty<float>();
        [HideInInspector] public ReactiveProperty<float> MaxHealth = new ReactiveProperty<float>();
        public uint DamagableID => 0;
        public List<Debuff> AttackDebuffs => null;
        public event Action death;
        public float MoveSpeed => stats.MoveSpeed;
        public WeaponController WeaponController { get; private set; }

        private PlayerModifiers Mods => Game.Player.Mods;
        private PlayerStatistics Statistics => Game.Player.Statistics;
        private PlayerMoveController playerController;
        private PlayerStats stats;
        private DebuffController effectController;
        private FogDamage fogDamage;
        private PlayerAnimationController animationController;

        private void Awake()
        {
            stats = new PlayerStats(PlayerAttributes.BASE_HEALTH, PlayerAttributes.BASE_MOVE_SPD, Statistics.Resistance);

            playerController = gameObject.AddComponent<PlayerMoveController>();
            playerController.Init(stats);
            WeaponController = gameObject.GetComponent<WeaponController>();
            MaxHealth = stats.MaxHealthReact;
            Health = stats.HealthReact;
            effectController = new DebuffController(transform, death, stats);
            fogDamage = gameObject.AddComponent<FogDamage>();
            fogDamage.Init(stats, 44f);
            animationController = gameObject.AddComponent<PlayerAnimationController>();
            animationController.Init(GetComponent<SpineAnimator>(), stats, Game.GameManager.LevelInputListener, WeaponController);
            Mods.modsUpdated += ModsUpdated;
            ModsUpdated();
        }

        private void ModsUpdated()
        {
            stats.SetStatModifiers(Mods.StatMods);
        }

        public void Damage(DamageInfo damageInfo)
        {
            stats.DoDamage(damageInfo.Damage);
            effectController.Add(damageInfo.Debuffs);
            Health = stats.HealthReact;
        }

        private void FixedUpdate()
        {
            stats.Update(Time.deltaTime);
        }
    }
}