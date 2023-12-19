using Assets.Scripts.Core.Interfaces;

namespace Assets.Scripts.Core.PlayerSystems.Perks
{
    public class IncreasedStatPerk : PlayerPerk, IStatModifier
    {

        public float HealthMod { get; set; }

        public float MoveSpeedMod { get; set; }

        public float RegenMod { get; set; }
    }
}