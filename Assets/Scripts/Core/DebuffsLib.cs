using Assets.Scripts.Core.Data;
using Assets.Scripts.Core.Passives;
using Newtonsoft.Json;
using RSG;
using System.Collections.Generic;

namespace Assets.Scripts.Core
{
    public class DebuffsLib
    {
        private const string PATH = "JSON/PassiveSkils";

        [JsonProperty] private DoTsData[] DoTs;
        [JsonProperty] private DebuffsData[] debuffs;

        private Dictionary<uint, Debuff> allDebuffs = new Dictionary<uint, Debuff>();

        public List<Debuff> GetDebuffs(uint[] uints)
        {
            var result = new List<Debuff>();
            if (uints == null) return result;
            for (int i = 0; i < uints.Length; i++)
            {
                result.Add(allDebuffs[uints[i]]);
            }
            return result;
        }

        public Promise Load()
        {
            var result = new Promise();
            AdressableLoader.LoadTextAssetAsync<DebuffsLib>(PATH).Then(data =>
            {
                InitData(data);
                result.Resolve();
            })
                .Catch(result.Reject);
            return result;
        }
        private void InitData(DebuffsLib data)
        {
            DoTs = data.DoTs;
            debuffs = data.debuffs;
            Update();
        }
        private void Update()
        {
            foreach (var data in DoTs)
                allDebuffs.TryAdd(data.id, new DoTDebuff(data));
            foreach (var data in debuffs)
                allDebuffs.TryAdd(data.id, new StatsDebuff(data));
        }
    }
}