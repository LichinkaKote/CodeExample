using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Core.PlayerSystems
{
    public class FogDamage : MonoBehaviour
    {
        [SerializeField] private float PercentDmgSec = 15f;
        [SerializeField] private float SafeRange = 44f;

        private PlayerStats stats;
        private Damage damage;
        public void Init(PlayerStats stats, float safeRange)
        {
            this.stats = stats;
            SafeRange = safeRange;
            stats.death += Disable;
            damage = new Damage();
            damage.PureDamage = stats.MaxHealth * (PercentDmgSec / 100f);
            enabled = true;
            
        }
        private void Awake()
        {
            enabled = false;
        }
        private void FixedUpdate()
        {
            if (IsInFog())
                stats.DoDamage(damage.Multiply(Time.deltaTime));
        }

        private bool IsInFog()
        {
            var xAbs = Mathf.Abs(transform.position.x);
            var yAbs = Mathf.Abs(transform.position.y);
            var value = Mathf.Max(xAbs, yAbs);
            return value > SafeRange;
        }
        private void Disable()
        {
            enabled = false;
        }
        private void OnDestroy()
        {
            stats.death -= Disable;
        }
    }
}