using Assets.Scripts.Core.Interfaces;
using System.Linq;
using UnityEditor;

namespace Assets.Scripts.Core.PlayerSystems
{
    public class PlayerStatistics
    {
        private readonly PlayerModifiers mods;
        private readonly Inventory inventory;

        public float Helalth { get; private set; }
        public float HelalthRegen { get; private set; }
        public float MoveSpeed { get; private set; }
        public IResistance Resistance { get; private set; }

        public PlayerStatistics(PlayerModifiers mods, Inventory inventory)
        {
            this.mods = mods;
            this.inventory = inventory;
            Resistance = new PlayerResistance();
            Update();
            mods.modsUpdated += UpdateStats;
            inventory.armorChanged += UpdateResistance;
        }
        private void Update()
        {
            UpdateStats();
            UpdateResistance();
        }
        private void UpdateStats()
        {
            var bonusHP = 0f;
            var bonusSpeed = 0f;
            var bonusRegen = 0f;
            foreach (var stat in mods.StatMods)
            {
                bonusHP += stat.HealthMod;
                bonusSpeed += stat.MoveSpeedMod;
                bonusRegen += stat.RegenMod;
            }
            Helalth = PlayerAttributes.BASE_HEALTH + bonusHP;
            MoveSpeed = PlayerAttributes.BASE_MOVE_SPD * (1 + bonusSpeed);
            HelalthRegen = bonusRegen;
        }
        private void UpdateResistance()
        {
            var res = inventory.GetArmorBarItems().Where(i => i is IResistance inf).Select(i => i as IResistance).ToList();
            (Resistance as PlayerResistance).Update(res);
        }
    }
}