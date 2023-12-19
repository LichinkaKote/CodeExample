using Assets.Scripts.Core.Interfaces;
using Newtonsoft.Json;

namespace Assets.Scripts.Core.Data
{
    public class EnemyDamageResistanceData : IResistance
    {
        public float? physRes;
        public float? poisonRes;
        public float? fireRes;
        public float? frostRes;
        public float? lightningRes;

        [JsonIgnore] public float PhysicalResistance => physRes.HasValue ? physRes.Value : 0f;
        [JsonIgnore] public float PoisonResistance => poisonRes.HasValue ? poisonRes.Value : 0f;
        [JsonIgnore] public float FireResistance => fireRes.HasValue ? fireRes.Value : 0f;
        [JsonIgnore] public float FrostResistance => frostRes.HasValue ? frostRes.Value : 0f;
        [JsonIgnore] public float LightningResistance => lightningRes.HasValue ? lightningRes.Value : 0f;

    }
}