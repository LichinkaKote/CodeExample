using Assets.Scripts.Core.Data;
using RSG;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Core.LevelManagment
{
    public class MapLib
    {
        private Dictionary<uint, MapData> maps;
        public (uint spriteSheetId, TileData[] data) GetData(uint levelID)
        {
            var map = maps[levelID];
            return (map.spriteSheetId, map.tileData);
        }

        public Promise Load()
        {
            var result = new Promise();
            AdressableLoader.LoadTextAssetAsync<MapData[]>(Strings.MapData).Then(data =>
            {
                maps = new Dictionary<uint, MapData>();
                foreach (var map in data)
                {
                    maps.Add(map.id, map);
                }
                result.Resolve();
            })
                .Catch(result.Reject);

            return result;
        }
    }
}