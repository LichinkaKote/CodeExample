namespace Assets.Scripts.Core.Interfaces
{
    public interface IDamage
    {
        public float PhysicalDamage { get; }
        public float PoisonDamage { get; }
        public float FireDamage { get; }
        public float FrostDamage { get; }
        public float LightningDamage { get; }
        public float PureDamage { get; }
        public float TotalDamage { get; }
    }
}