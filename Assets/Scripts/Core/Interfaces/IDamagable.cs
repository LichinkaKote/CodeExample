namespace Assets.Scripts.Core.Interfaces
{
    public interface IDamagable
    {
        public void Damage(DamageInfo damageInfo);
        public uint DamagableID { get; }

    }
}