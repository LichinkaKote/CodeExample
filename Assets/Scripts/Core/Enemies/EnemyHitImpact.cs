using Assets.Scripts.Core.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Core.Enemies
{
    public class EnemyHitImpact : MonoBehaviour
    {
        [SerializeField] private ParticleSystem bloodPartsPierce;
        [SerializeField] private ParticleSystem bloodPartsFront;

        private Enemy self;
        private Vector3 hitDir;
        private float hitForce;
        private float maxHitForce = 2f;
        private float hitForceMult = 0.2f;
        private float hitForceRedution = 12f;
        Rigidbody2D rb;

        private void Awake()
        {
            self = GetComponent<Enemy>();
            rb = GetComponent<Rigidbody2D>();
        }
        private void Start()
        {
            self.hitInfo += OnHit;
        }
        private void OnEnable()
        {
            hitForce = 0f;
        }

        private void Update()
        {
            ApplyForce();
        }
        private void ApplyForce()
        {
            if (hitForce > 0)
            {
                transform.localPosition += hitDir * hitForce * Time.deltaTime;
                hitForce -= Time.deltaTime * hitForceRedution;
                if (hitForce < 0f) hitForce = 0f;
            }
        }
        private void OnHit(DamageInfo info)
        {
            hitDir = info.HitInfo.HitDirection;
            hitForce += (info.HitInfo.RagdollForce / rb.mass) * hitForceMult;
            if (hitForce > maxHitForce) hitForce = maxHitForce;
            ParticlesPlay(info);
        }
        private void ParticlesPlay(DamageInfo dmgInfo)
        {
            if (dmgInfo.HitInfo is IRangedHitInfo rangHit && rangHit.Penetrated)
            {
                bloodPartsPierce.transform.rotation = Quaternion.LookRotation(dmgInfo.HitInfo.HitDirection);
                bloodPartsPierce.Play();
            }
            else
            {
                bloodPartsFront.transform.rotation = Quaternion.LookRotation(-dmgInfo.HitInfo.HitDirection);
                bloodPartsFront.Play();
            }
        }
        private void OnDestroy()
        {
            self.hitInfo -= OnHit;
        }
    }
}