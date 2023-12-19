using Assets.Scripts.Core.Enemies;
using Assets.Scripts.Core.Interfaces;
using System;
using UniRx;

namespace Assets.Scripts.Core
{
    public class GameEvents
    {
        public event Action<Enemy> actorDeath;
        public event Action<DamageInfo> actorHit;
        public event Action<(IHitInfo hitInfo, IDamage damage)> actorGotDamage;
        public ReactiveProperty<bool> Pause { get; private set; } = new ReactiveProperty<bool>(false);
        public void InvokeActorDeathEvent(Enemy enemy)
        {
            actorDeath?.Invoke(enemy);
        }
        public void InvokeEnemyHitEvent(DamageInfo info)
        {
            actorHit?.Invoke(info);
        }
        public void InvokeEnemyGotDamage(IHitInfo hitInfo, IDamage damage)
        {
            actorGotDamage?.Invoke((hitInfo, damage));
        }
        public void InvokePause(bool value)
        {
            Pause.Value = value;
        }
    }
}