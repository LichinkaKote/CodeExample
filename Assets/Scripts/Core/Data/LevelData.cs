using System.Collections.Generic;

namespace Assets.Scripts.Core.Data
{
    public class LevelData
    {
        public uint id;
        public string name;
        public string desc;
        public ushort spawnPool;
        public uint mapId;
        public List<SpawnInfo> enemyData;
        public class SpawnInfo
        {
            public uint id;
            public uint count;
        }
    }
}