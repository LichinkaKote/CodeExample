using Assets.Scripts.Core.PlayerSystems.Perks;
using System;
using System.Collections.Generic;
using UniRx;

namespace Assets.Scripts.Core.PlayerSystems
{
    public class PlayerPerks
    {
        public event Action perksChanged;
        private Dictionary<PlayerPerk, bool> allPerks = new Dictionary<PlayerPerk, bool>();
        public ReactiveProperty<short> FreePerkPoints { get; private set; } = new ReactiveProperty<short>();
        public List<PlayerPerk> LearnedPerks { get; private set; } = new List<PlayerPerk>();
        public List<PlayerPerk> UnlearnedPerks { get; private set; } = new List<PlayerPerk>();

        public bool HaveUnlearnedPerks => UnlearnedPerks.Count > 0;
        public bool HaveFreePerkPoints => FreePerkPoints.Value > 0;

        private const byte LEVEL_THRESHOLD = 2;


        public PlayerPerks(PlayerExperience playerExp)
        {
            InitPerks();
            UpdatePearksStatus();
            playerExp.levelUp += OnLevelUp;
        }

        private void OnLevelUp(short level)
        {
            if (level % LEVEL_THRESHOLD == 0)
                FreePerkPoints.Value++;
        }
        public void LearnPerk(PlayerPerk perk)
        {
            if (HaveFreePerkPoints)
            {
                FreePerkPoints.Value--;
                allPerks[perk] = true;
                UpdatePearksStatus();
            }
        }
        private void UpdatePearksStatus()
        {
            LearnedPerks.Clear();
            UnlearnedPerks.Clear();
            foreach (var perk in allPerks)
            {
                if (perk.Value)
                    LearnedPerks.Add(perk.Key);
                else
                    UnlearnedPerks.Add(perk.Key);
            }
            perksChanged?.Invoke();
        }


        #region AllPerks
        private void InitPerks()
        {
            allPerks.Add(new IncreasedStatPerk() { Name = "Bull", HealthMod = 20f, IconID = 1, RegenMod = 1f }, false);
            allPerks.Add(new IncreasedStatPerk() { Name = "Move spd", MoveSpeedMod = 0.15f, IconID = 2 }, false);
            allPerks.Add(new RangedWeaponPerk() { Name = "Fast hand", ReloadMod = 0.25f, IconID = 3 }, false);
        }
        #endregion
    }
}