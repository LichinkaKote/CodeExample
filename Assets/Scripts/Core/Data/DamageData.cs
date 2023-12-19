using Assets.Scripts.Core.Interfaces;

namespace Assets.Scripts.Core.Data
{
    public class DamageData
    {
        public float? physicalDmg;
        public float? poisonDmg;
        public float? firelDmg;
        public float? frostDmg;
        public float? lightningDmg;

        public IDamage GetDamage()
        {
            var damage = new Damage();
            damage.PhysicalDamage = physicalDmg.HasValue ? physicalDmg.Value : 0f;
            damage.PoisonDamage = poisonDmg.HasValue ? poisonDmg.Value : 0f;
            damage.FireDamage = firelDmg.HasValue ? firelDmg.Value : 0f;
            damage.FrostDamage = frostDmg.HasValue ? frostDmg.Value : 0f;
            damage.LightningDamage = lightningDmg.HasValue ? lightningDmg.Value : 0f;
            return damage;
        }
    }
}