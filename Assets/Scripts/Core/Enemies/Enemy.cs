using Assets.Scripts.Core.Data;
using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.PlayerSystems;
using System;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.Core.Enemies
{
    public class Enemy : MonoBehaviour, IDamagable
    {
        public uint ID => data.id;
        public string Name => data.name;
        public float Size => data.size;
        public float Mass { get; private set; }
        public float DistanceToPlayer => enemyMovement.DistanceToPlayer;
        public ReactiveProperty<bool> IsLookLeft => enemyMovement.IsLookLeft;
        public uint DamagableID => ID;
        public int Experience { get; private set; }
        public float RagdollMult { get; private set; }
        public IHitInfo LastHitInfo { get; private set; }

        public event Action death;
        public Action<DamageInfo> hitInfo;

        private Player player;
        private EnemyData data;
        private Rigidbody2D rb;
        private MeshRenderer render;
        private Stats stats;
        private EnemyMovement enemyMovement;
        private EnemyAttack enemyAttack;
        private EnemyAnimator enemyAnimator;
        private DebuffController debuffController;
        private EnemyStatusEffects enemyStatusEffects;

        private void Awake()
        {
            player = Game.GameManager.Player;
            render = GetComponentInChildren<MeshRenderer>();
            rb = GetComponent<Rigidbody2D>();
            enemyStatusEffects = GetComponentInChildren<EnemyStatusEffects>();
            enemyMovement = gameObject.AddComponent<EnemyMovement>();
            enemyAnimator = gameObject.AddComponent<EnemyAnimator>();
            enemyAttack = gameObject.AddComponent<EnemyAttack>();
            enemyAttack.Attack += AttackPlayer;
        }
        public void Init(EnemyData data)
        {
            this.data = data;
            Experience = data.exp.HasValue ? data.exp.Value : 0;
            Mass = data.mass.HasValue ? data.mass.Value : 1f;
            RagdollMult = data.ragdollResistance.HasValue ? Mathf.Clamp01(1 - data.ragdollResistance.Value) : 1f;
            rb.mass = Mass;
            stats = new Stats(data.health, data.moveSpeed + UnityEngine.Random.Range(-data.moveSpeedRange, data.moveSpeedRange), data.DamageResistance);
            stats.death += OnDeath;
            render.transform.localScale = Vector3.one * data.size;
            render.material = Game.Library.MaterialLib.GetMaterial(data.Texture);
            enemyMovement.Init(stats);
            enemyAttack.Init(data, enemyMovement, Game.Library.DebuffsLib.GetDebuffs(data.debuffs));
            enemyAnimator.Init(enemyMovement);

            debuffController = new DebuffController(transform, death, stats);
            enemyStatusEffects.Init(debuffController);
        }
        public void SetVisble(bool value)
        {
            gameObject.SetActive(value);
        }

        public void Damage(DamageInfo dmgInfo)
        {
            var hitInfoCopy = dmgInfo.HitInfo;
            hitInfoCopy.RagdollForce *= RagdollMult;
            UpdateLastHitInfo(hitInfoCopy);
            hitInfo?.Invoke(dmgInfo);
            Game.Events.InvokeEnemyHitEvent(dmgInfo);
            var damage = stats.DoDamage(dmgInfo.Damage);
            Game.Events.InvokeEnemyGotDamage(dmgInfo.HitInfo, damage);
            debuffController.Add(dmgInfo.Debuffs);
        }
        private void UpdateLastHitInfo(IHitInfo info)
        {
            if (info is IRangedHitInfo newHit && LastHitInfo is IRangedHitInfo oldHit && oldHit.ProjectileID == newHit.ProjectileID)
            {
                LastHitInfo.RagdollForce += info.RagdollForce;
            }
            else
            {
                LastHitInfo = info;
            }
        }
        private void OnDeath()
        {
            stats.death -= OnDeath;
            death?.Invoke();
            Game.Events.InvokeActorDeathEvent(this);
        }
        private void AttackPlayer(DamageInfo damageInfo)
        {
            player.Damage(damageInfo);
        }
        private void OnDestroy()
        {
            enemyAttack.Attack -= AttackPlayer;
            stats.death -= OnDeath;
        }
        public void Teleport(Vector3 pos) => enemyMovement.Teleport(pos);
    }
}