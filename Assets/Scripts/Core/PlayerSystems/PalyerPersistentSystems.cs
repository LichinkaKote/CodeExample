namespace Assets.Scripts.Core.PlayerSystems
{
    public class PalyerPersistentSystems
    {
        public Inventory Inventory { get; private set; }
        public PlayerExperience Experience { get; private set; }
        public PlayerAttributes Attributes { get; private set; }
        public PlayerSkills Skills { get; private set; }
        public PlayerPerks Perks { get; private set; }
        public PlayerModifiers Mods { get; private set; }
        public PlayerStatistics Statistics { get; private set; }
        public PalyerPersistentSystems()
        {
            Inventory = new Inventory();
            Experience = new PlayerExperience();
            Attributes = new PlayerAttributes();
            Skills = new PlayerSkills();
            Perks = new PlayerPerks(Experience);
            Mods = new PlayerModifiers(Perks, Attributes, Skills);
            Statistics = new PlayerStatistics(Mods, Inventory);
        }
    }
}