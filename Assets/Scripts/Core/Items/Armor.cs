using Assets.Scripts.Core.Data;
using Assets.Scripts.Core.Interfaces;

namespace Assets.Scripts.Core.Items
{
    public class Armor : Item, IResistance
    {
        public float PhysicalResistance { get; private set; }
        public float PoisonResistance { get; private set; }
        public float FireResistance { get; private set; }
        public float FrostResistance { get; private set; }
        public float LightningResistance { get; private set; }
        public float MoveSpeedMult { get; private set; }
        public float HealthRegen { get; private set; }
        public Armor(ArmorData data) : base(data)
        {
            PhysicalResistance = data.physRes.HasValue ? data.physRes.Value : 0f;
            PoisonResistance = data.poisonRes.HasValue ? data.poisonRes.Value : 0f;
            FireResistance = data.fireRes.HasValue ? data.fireRes.Value : 0f;
            FrostResistance = data.frostRes.HasValue ? data.frostRes.Value : 0f;
            LightningResistance = data.lightningRes.HasValue ? data.lightningRes.Value : 0f;
            MoveSpeedMult = data.moveSpeedMult.HasValue ? data.moveSpeedMult.Value : 1f;
            HealthRegen = data.healthRegen.HasValue ? data.healthRegen.Value : 0f;
        }
    }
}