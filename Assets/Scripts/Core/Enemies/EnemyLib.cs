using Assets.Scripts.Core.Data;
using RSG;
using System.Collections.Generic;

namespace Assets.Scripts.Core.Enemies
{
    public class EnemyLib
    {
        private EnemyData[] enemyData;
        public EnemyData[] All => enemyData;
        public EnemyData GetData(uint id)
        {
            foreach (EnemyData enemy in enemyData)
            {
                if (enemy.id == id)
                {
                    return enemy;
                }
            }
            return null;
        }
        public List<EnemyData> GetDatas(List<uint> ids)
        {
            var result = new List<EnemyData>();
            foreach (var id in ids)
            {
                var data = GetData(id);
                if (data != null)
                    result.Add(data);
            }
            return result;
        }
        public Promise LoadEnemies()
        {
            var result = new Promise();

            AdressableLoader.LoadTextAssetAsync<EnemyData[]>(Strings.JSONEnemies).Then(data =>
            {
                enemyData = data;
                result.Resolve();
            })
                .Catch(result.Reject);

            return result;
        }
    }
}