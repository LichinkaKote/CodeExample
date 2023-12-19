namespace Assets.Scripts.Core.Interfaces
{
    public interface IStatModifier
    {
        public float HealthMod { get; }
        public float MoveSpeedMod { get; }
        public float RegenMod { get; }
    }
}