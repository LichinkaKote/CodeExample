namespace Assets.Scripts.Core.Interfaces
{
    public interface IResistance
    {
        public float PhysicalResistance { get; }
        public float PoisonResistance { get; }
        public float FireResistance { get; }
        public float FrostResistance { get; }
        public float LightningResistance { get; }
    }
}