namespace Assets.Scripts.Core.Interfaces
{
    public interface IRangedHitInfo : IHitInfo
    {
        public int ProjectileID { get; }
        public bool Penetrated { get; }
    }
}