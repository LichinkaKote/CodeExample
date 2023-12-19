using Assets.Scripts.Core.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Core.LevelManagment
{
    public class Level
    {
        public uint ID { get; private set; }
        public ushort SpawnPool { get; private set; }
        public uint MapID { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public List<LevelData.SpawnInfo> EnemyData { get; private set; }

        public List<uint> EnemyIDs => GetEnemyIDs();
        public Level(LevelData data)
        {
            ID = data.id;
            Name = data.name;
            Description = data.desc;
            SpawnPool = data.spawnPool;
            EnemyData = data.enemyData;
            MapID = data.mapId;
        }
        public Dictionary<uint, uint> GetEnemyCount()
        {
            var result = new Dictionary<uint, uint>();
            foreach (var enemy in EnemyData)
            {
                if (!result.TryAdd(enemy.id, enemy.count))
                {
                    result[enemy.id] = result[enemy.id] + enemy.count;
                }
            }
            return result;
        }
        private List<uint> GetEnemyIDs()
        {
            var result = new List<uint>();
            foreach (var e in EnemyData)
            {
                if (!result.Contains(e.id))
                    result.Add(e.id);
            }
            return result;
        }
        public Task<Sprite[]> GetPreviewSprite()
        {
            var mapData = Game.Library.MapLib.GetData(MapID);
            return AdressableLoader.LoadAssetAsyncTask<Sprite[]>(Strings.MapTile + mapData.spriteSheetId.ToString());
        }
    }
}