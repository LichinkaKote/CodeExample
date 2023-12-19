using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Passives;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public abstract class AbstractProjectile : MonoBehaviour
    {
        [SerializeField] private LayerMask layerMask;
        protected Vector3 prevPosition;
        Vector3 direction;
        protected Collider2D currentCollider;

        protected ushort penetratedCount;
        protected IDamage damage;

        public float Velocity { get; set; }
        public float LifeTime { get; set; }
        public ushort PenetrationForce { get; set; }
        public float Ragdoll { get; set; }
        public int ProjectileID { get; set; }
        public List<Debuff> Debuffs { get; set; }

        private void Start()
        {
            OnStart();
            if (LifeTime > 0)
            {
                Destroy(gameObject, LifeTime);
            }
        }
        protected abstract void OnStart();
        public virtual void Fire(Vector3 direction, IDamage damage)
        {
            this.damage = damage;
            this.direction = direction.normalized;
            transform.rotation = Quaternion.FromToRotation(Vector3.right, direction);
        }
        private void Update()
        {
            OnUpdate();
        }
        protected virtual void OnUpdate()
        {
            prevPosition = transform.position;
            MoveToTarget();
            CastRay();
        }

        protected virtual void MoveToTarget()
        {
            transform.position += direction * Time.deltaTime * Velocity;
        }
        protected virtual void CastRay()
        {
            var direction = prevPosition - transform.position;
            var distance = direction.magnitude;
            var raycastHit2D = Physics2D.Raycast(transform.position, direction, distance, layerMask);
            if (raycastHit2D.collider != null && raycastHit2D.collider != currentCollider)
            {
                currentCollider = raycastHit2D.collider;

                bool penetrated = penetratedCount < PenetrationForce;
                if (!penetrated)
                {
                    Destroy(gameObject);
                }


                penetratedCount++;

                var damagable = raycastHit2D.collider.gameObject.GetComponent<IDamagable>();
                if (damagable != null)
                {
                    var hitInfo = new RangedHitInfo();
                    hitInfo.HittedObjectId = damagable.DamagableID;
                    hitInfo.HitDirection = -direction.normalized;
                    hitInfo.Penetrated = penetrated;
                    hitInfo.HitPosition = raycastHit2D.collider.transform.position;
                    hitInfo.RagdollForce = Ragdoll;
                    hitInfo.ProjectileID = ProjectileID;
                    damagable.Damage(new DamageInfo(damage, Debuffs, hitInfo));
                }
            }
        }
    }
}