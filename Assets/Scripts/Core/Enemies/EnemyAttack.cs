using Assets.Scripts.Core.Data;
using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Passives;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.Enemies
{
    public class EnemyAttack : MonoBehaviour
    {
        public event Action<DamageInfo> Attack;

        private float cd;
        private EnemyData data;
        private EnemyMovement movement;
        private Action attackType;
        private IDamage damage;
        private List<Debuff> debuffs;

        private float AttackDistance => data.attackDistance;
        private bool IsRanged => data.isRanged;
        private float AttackCD => data.attackCD;
        private bool InAttackRange => movement.DistanceToPlayer <= AttackDistance;
        private bool CanAttack => InAttackRange && cd <= 0f;

        public void Init(EnemyData enemyData, EnemyMovement enemyMovement, List<Debuff> debuffs)
        {
            data = enemyData;
            movement = enemyMovement;
            movement.Stop = false;
            this.debuffs = debuffs;
            damage = data.attackDamage.GetDamage();
            attackType = IsRanged ? RangedAttack : MeleAttack;
        }
        private void Update()
        {
            attackType?.Invoke();
            ReduceCD();
        }
        private void ReduceCD()
        {
            if (cd > 0)
            {
                cd = Mathf.Clamp(cd - Time.deltaTime, 0f, AttackCD);
            }
        }
        private void MeleAttack()
        {
            if (CanAttack)
            {
                Attack?.Invoke(new DamageInfo(damage, debuffs, new HitInfo()));
                cd = AttackCD;
            }
        }
        private void RangedAttack()
        {
            movement.Stop = InAttackRange;
            if (CanAttack)
            {
                Shoot();
                cd = AttackCD;
            }
        }
        private void Shoot()
        {
            var proj = Instantiate(Game.Prefabs.GetEnemyProjectile(0));
            proj.transform.position = transform.position;
            proj.Velocity = 10f;
            proj.LifeTime = 2f;
            proj.Debuffs = debuffs;
            proj.Fire(Game.GameManager.PlayerTranform.position - transform.position, damage);
        }
    }
}