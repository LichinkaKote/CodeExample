using Assets.Scripts.Core.Interfaces;
using System.Collections.Generic;

namespace Assets.Scripts.Core.Data
{
    public class PawnResistances : IResistance
    {
        public float PhysicalResistance { get; private set; }
        public float PoisonResistance { get; private set; }
        public float FireResistance { get; private set; }
        public float FrostResistance { get; private set; }
        public float LightningResistance { get; private set; }

        public void Update(List<IResistance> resistanceInfos)
        {
            foreach (var info in resistanceInfos)
            {
                PhysicalResistance += info.PhysicalResistance;
                PoisonResistance += info.PoisonResistance;
                FireResistance += info.FireResistance;
                FrostResistance += info.FrostResistance;
                LightningResistance += info.LightningResistance;
            }
        }
        public void Update(IResistance resistanceInfo)
        {
            PhysicalResistance = resistanceInfo.PhysicalResistance;
            PoisonResistance = resistanceInfo.PoisonResistance;
            FireResistance = resistanceInfo.FireResistance;
            FrostResistance = resistanceInfo.FrostResistance;
            LightningResistance = resistanceInfo.LightningResistance;
        }
    }
}