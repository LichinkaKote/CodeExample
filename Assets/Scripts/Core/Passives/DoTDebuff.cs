using Assets.Scripts.Core.Data;
using Assets.Scripts.Core.Enums;
using Assets.Scripts.Core.Interfaces;

namespace Assets.Scripts.Core.Passives
{
    public class DoTDebuff : Debuff, IStatusEffect
    {
        public uint DamageTypeId { get; protected set; }
        public StatusEffectType EffectType { get; private set; }

        private Damage damage;

        public DoTDebuff(DoTsData data) : base(data)
        {
            DamageTypeId = data.damageTypeId;
            Duration = data.duration;
            switch (DamageTypeId)
            {
                case 1:
                    damage.PhysicalDamage = Magnitude * DebuffController.TICK_INTERVAL;
                    EffectType = StatusEffectType.PhysDoT;
                    break;
                case 2:
                    damage.PoisonDamage = Magnitude * DebuffController.TICK_INTERVAL;
                    EffectType = StatusEffectType.PoisonDoT;
                    break;
                case 3:
                    damage.FireDamage = Magnitude * DebuffController.TICK_INTERVAL;
                    EffectType = StatusEffectType.FireDoT;
                    break;
                case 4:
                    damage.FrostDamage = Magnitude * DebuffController.TICK_INTERVAL;
                    break;
                case 5:
                    damage.LightningDamage = Magnitude * DebuffController.TICK_INTERVAL;
                    break;
                default:
                    break;
            }
        }

        public void Tick(Stats stats)
        {
            stats.DoDamage(damage);
        }
    }
}