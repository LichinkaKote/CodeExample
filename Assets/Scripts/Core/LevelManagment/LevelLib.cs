using Assets.Scripts.Core.Data;
using RSG;
using System.Linq;

namespace Assets.Scripts.Core.LevelManagment
{
    public class LevelLib
    {
        public Level[] Levels { get; private set; }

        public Promise LoadLevels()
        {
            var result = new Promise();
            AdressableLoader.LoadTextAssetAsync<LevelData[]>("JSON/Levels").Then(levelDatas =>
            {
                Levels = new Level[levelDatas.Length];
                for (int i = 0; i < Levels.Length; i++)
                    Levels[i] = new Level(levelDatas[i]);

                result.Resolve();
            })
                .Catch(result.Reject);

            return result;
        }
        public bool TryGetLevel(uint id, out Level level)
        {
            level = Levels.FirstOrDefault(lv => lv.ID == id);
            return level != default;
        }
        public Level GetFirstLevel()
        {
            return Levels.First();
        }
    }
}