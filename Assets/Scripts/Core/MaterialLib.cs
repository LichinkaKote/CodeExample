using Assets.Scripts.Core.Data;
using Assets.Scripts.Core.LevelManagment;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public class MaterialLib
    {
        private Dictionary<string, Material> cache;

        public async Task PreLoadEnemyMaterialsForLevel(Level level)
        {
            cache = new Dictionary<string, Material>();
            var enemies = Game.Library.EnemyLib.GetDatas(level.EnemyIDs);
            var paths = GetTexurePaths(enemies);
            var result = new List<Task<Material>>();

            foreach (var path in paths)
            {
                var task = AdressableLoader.LoadAssetAsyncTask<Material>(path);
                result.Add(task);
            }
            await Task.WhenAll(result);

            for (int i = 0; i < result.Count; i++)
                cache.Add(paths[i], result[i].Result);
        }
        private List<string> GetTexurePaths(List<EnemyData> enemyDatas)
        {
            var result = new List<string>();
            foreach (var enemy in enemyDatas)
            {
                if (!result.Contains(enemy.Texture))
                    result.Add(enemy.Texture);
                if (!result.Contains(enemy.DeadTexture))
                    result.Add(enemy.DeadTexture);
                if (!result.Contains(enemy.BloodTexture))
                    result.Add(enemy.BloodTexture);
            }
            return result;
        }

        public Material GetMaterial(string material)
        {
            return cache[material];
        }
    }
}