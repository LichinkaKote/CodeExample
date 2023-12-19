using Assets.Scripts.Core.Enemies;
using System;
using UniRx;

namespace Assets.Scripts.Core.PlayerSystems
{
    public class PlayerExperience
    {
        public const short MAX_LVL = 40;
        public short Level { get; private set; }
        public ReactiveProperty<int> Experience { get; private set; } = new ReactiveProperty<int>();
        public int ExpToNextLevel { get; private set; }
        public event Action<short> levelUp;


        private int expIcr = 25;
        private int baseExpToLevelup = 50;

        public PlayerExperience(short level = 1, int experience = 0)
        {
            Level = level;
            SetExpToNext();
            Experience.Value = experience;
            Game.Events.actorDeath += OnEnemyDeath;
        }

        private void OnEnemyDeath(Enemy enemy)
        {
            AddExp(enemy.Experience);
        }

        public void AddExp(int value)
        {
            var nextExpValue = Experience.Value + value;
            var delta = ExpToNextLevel - nextExpValue;
            if (delta <= 0)
            {
                LevelUp();
                Experience.Value = delta * -1;
            }
            else
            {
                Experience.Value = nextExpValue;
            }

        }

        private void LevelUp()
        {
            Level++;
            SetExpToNext();
            levelUp?.Invoke(Level);
        }
        private void SetExpToNext()
        {
            ExpToNextLevel = baseExpToLevelup + Level * expIcr;
        }
        ~PlayerExperience()
        {
            Game.Events.actorDeath -= OnEnemyDeath;
        }
    }
}