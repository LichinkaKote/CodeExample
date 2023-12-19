using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Passives;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public class DebuffController : IStatusEffectController
    {
        public const float TICK_INTERVAL = 0.2f;
        private Stats stats;
        private Dictionary<StatsDebuff, DebuffStatus> statDebuffs = new Dictionary<StatsDebuff, DebuffStatus>();
        private Dictionary<DoTDebuff, DebuffStatus> dots = new Dictionary<DoTDebuff, DebuffStatus>();
        private IDisposable timer;
        private Transform parent;
        private bool isAlive;
        private Action death;

        public event Action effetsUpdated;
        public List<IStatusEffect> StatusEffects { get; private set; } = new List<IStatusEffect>();

        public DebuffController(Transform parent, Action death, Stats stats)
        {
            isAlive = true;
            this.parent = parent;
            this.stats = stats;
            this.death = death;
            this.death += OnDeath;
        }
        private void OnDeath()
        {
            isAlive = false;
            statDebuffs.Clear();
            dots.Clear();
            death -= OnDeath;
            RemoveTimer();
        }
        public void Add(List<Debuff> debuff)
        {
            if (debuff == null) return;

            foreach (var value in debuff)
            {
                if (value is StatsDebuff sd)
                    AddStatDebuff(sd);
                else if (value is DoTDebuff dot)
                    AddDoTDebuff(dot);
            }
            if (timer == null)
                timer = Observable.Interval(TimeSpan.FromSeconds(TICK_INTERVAL), Scheduler.MainThreadEndOfFrame).Subscribe(_ => Tick()).AddTo(parent);
        }
        private void AddStatDebuff(StatsDebuff debuff)
        {
            if (statDebuffs.ContainsKey(debuff))
                statDebuffs[debuff].duration = debuff.Duration;
            else
            {
                var status = new DebuffStatus { duration = debuff.Duration };
                debuff.Apply(stats);
                statDebuffs.Add(debuff, status);
                UpdateStatusEffects();
            }
        }
        private void AddDoTDebuff(DoTDebuff dot)
        {
            if (dots.ContainsKey(dot))
                dots[dot].duration = dot.Duration;
            else
            {
                var status = new DebuffStatus { duration = dot.Duration };
                dots.Add(dot, status);
                UpdateStatusEffects();
            }
        }

        private void Tick()
        {
            DoTTick();
            if (!isAlive) return;
            DebuffTick();
            if (statDebuffs.Count == 0 && dots.Count == 0)
                RemoveTimer();
        }
        private void DebuffTick()
        {
            var keysToRemove = new List<StatsDebuff>();

            foreach (var debuff in statDebuffs)
            {
                debuff.Value.duration -= TICK_INTERVAL;
                if (debuff.Value.Remove)
                {
                    debuff.Key.Remove(stats);
                    keysToRemove.Add(debuff.Key);
                }
            }

            foreach (var key in keysToRemove)
                statDebuffs.Remove(key);

            if (keysToRemove.Count > 0)
                UpdateStatusEffects();
        }
        private void DoTTick()
        {
            var keysToRemove = new List<DoTDebuff>();

            foreach (var dot in dots)
            {
                dot.Key.Tick(stats);
                if (!isAlive) return;
                dot.Value.duration -= TICK_INTERVAL;
                if (dot.Value.Remove)
                    keysToRemove.Add(dot.Key);
            }

            foreach (var key in keysToRemove)
                dots.Remove(key);

            if (keysToRemove.Count > 0)
                UpdateStatusEffects();
        }

        private void RemoveTimer()
        {
            timer?.Dispose();
            timer = null;
        }
        private class DebuffStatus
        {
            public bool Remove => duration <= 0f;
            public float duration;
            public byte stacks;
        }
        private void UpdateStatusEffects()
        {
            StatusEffects.Clear();
            foreach (var effect in statDebuffs)
            {
                if (effect.Key is IStatusEffect stEff)
                {
                    StatusEffects.Add(stEff);
                }
            }
            effetsUpdated?.Invoke();
        }
    }
}