using Assets.Scripts.Core.Data;
using Assets.Scripts.Core.Enums;
using Assets.Scripts.Core.Interfaces;

namespace Assets.Scripts.Core.Passives
{
    public class StatsDebuff : Debuff, IStatusEffect
    {
        private enum StatType
        {
            Health = 0,
            Speed = 1,
        }
        private StatType statType;

        public StatusEffectType EffectType { get; private set; }

        public StatsDebuff(DebuffsData data) : base(data)
        {
            statType = (StatType)data.typeId;
            Duration = data.duration;
            if (data.typeId == (uint)StatType.Speed)
                EffectType = StatusEffectType.Slow;
        }

        public void Apply(Stats stats)
        {
            switch (statType)
            {
                case StatType.Health:
                    stats.AddHealthDebuff(ID, Magnitude);
                    break;
                case StatType.Speed:
                    stats.AddSpeedDebuff(ID, Magnitude);
                    break;
            }
        }

        public void Remove(Stats stats)
        {
            switch (statType)
            {
                case StatType.Health:
                    stats.RemoveHealthDebuff(ID);
                    break;
                case StatType.Speed:
                    stats.RemoveSpeedDebuff(ID);
                    break;
            }
        }
    }
}