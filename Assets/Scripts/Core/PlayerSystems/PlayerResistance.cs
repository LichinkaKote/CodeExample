using Assets.Scripts.Core.Interfaces;
using System.Collections.Generic;

namespace Assets.Scripts.Core.PlayerSystems
{
    public class PlayerResistance : IResistance
    {
        public float PhysicalResistance { get; private set; }
        public float PoisonResistance { get; private set; }
        public float FireResistance { get; private set; }
        public float FrostResistance { get; private set; }
        public float LightningResistance { get; private set; }

        public void Update(List<IResistance> resistances)
        {
            PhysicalResistance = 0f;
            PoisonResistance = 0f;
            FireResistance = 0f;
            FrostResistance = 0f;
            LightningResistance = 0f;

            foreach (var resistance in resistances)
            {
                PhysicalResistance += resistance.PhysicalResistance;
                PoisonResistance += resistance.PoisonResistance;
                FireResistance += resistance.FireResistance;
                FrostResistance += resistance.FrostResistance;
                LightningResistance += resistance.LightningResistance;
            }
        }
    }
}