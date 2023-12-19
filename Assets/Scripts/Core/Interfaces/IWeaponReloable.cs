using UniRx;

namespace Assets.Scripts.Core.Interfaces
{
    public interface IWeaponReloable
    {
        public void Reload();
        public ReactiveProperty<float> ReloadProgress { get; }
        public float ReloadTime { get; }
        public ReactiveProperty<int> Ammo { get; }
    }
}