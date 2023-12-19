using Assets.Scripts.Core.Enemies;
using Assets.Scripts.Core.Items;
using Assets.Scripts.Core.LevelManagment;
using RSG;

namespace Assets.Scripts.Core
{
    public class Library
    {
        public MaterialLib MaterialLib { get; private set; } = new MaterialLib();
        public LevelLib LevelLib { get; private set; } = new LevelLib();
        public ItemLib ItemLib { get; private set; } = new ItemLib();
        public EnemyLib EnemyLib { get; private set; } = new EnemyLib();
        public DebuffsLib DebuffsLib { get; private set; } = new DebuffsLib();
        public MapLib MapLib { get; private set; } = new MapLib();
        public SpriteLib SpriteLib { get; private set; } = new SpriteLib();

        public IPromise Load()
        {
            return Promise.All(
            LevelLib.LoadLevels(),
            MapLib.Load(),
            ItemLib.LoadItems(),
            DebuffsLib.Load(),
            EnemyLib.LoadEnemies(),
            SpriteLib.Load()
            );
        }
    }
}