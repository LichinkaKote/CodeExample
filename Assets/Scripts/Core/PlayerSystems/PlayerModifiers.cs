using Assets.Scripts.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Assets.Scripts.Core.PlayerSystems
{
    public class PlayerModifiers
    {
        public event Action modsUpdated;
        public List<IStatModifier> StatMods { get; private set; }
        public List<IRangedWeaponModifier> RangedWeaponMods { get; private set; }
        private readonly PlayerPerks perks;
        private readonly PlayerAttributes attributes;
        private readonly PlayerSkills skills;

        public PlayerModifiers(PlayerPerks perks, PlayerAttributes attributes, PlayerSkills Skills)
        {
            this.perks = perks;
            this.attributes = attributes;
            skills = Skills;
            UpdateMods();
            Subscribe();
        }

        private void Subscribe()
        {
            perks.perksChanged += UpdateMods;
            attributes.statChanged += UpdateMods;
            skills.skillChanged += UpdateMods;
        }

        private void UpdateMods()
        {
            StatMods = GetStatMods();
            RangedWeaponMods = GetRangedMods();
            modsUpdated?.Invoke();
        }
        private List<IStatModifier> GetStatMods()
        {
            var result = new List<IStatModifier>();
            var perkMods = perks.LearnedPerks.Where(p => p is IStatModifier).Select(p => p as IStatModifier).ToList();
            result.AddRange(perkMods);
            result.Add(attributes);
            return result;
        }
        private List<IRangedWeaponModifier> GetRangedMods()
        {
            var result = new List<IRangedWeaponModifier>();
            var perkMods = perks.LearnedPerks.Where(p => p is IRangedWeaponModifier).Select(p => p as IRangedWeaponModifier).ToList();
            result.AddRange(perkMods);
            result.Add(attributes);
            result.Add(skills);
            return result;
        }
    }
}