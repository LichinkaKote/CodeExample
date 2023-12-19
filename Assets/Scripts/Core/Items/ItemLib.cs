using Assets.Scripts.Core.Data;
using Newtonsoft.Json;
using RSG;
using System.Collections.Generic;

namespace Assets.Scripts.Core.Items
{
    public class ItemLib
    {
        private const string PATH = "JSON/ItemLib";

        [JsonIgnore] public List<Item> Items = new List<Item>();
        [JsonIgnore] public List<Item> Weapons = new List<Item>();
        [JsonIgnore] public List<Item> Armors = new List<Item>();
        [JsonIgnore] public List<ItemMod> Mods = new List<ItemMod>();

        [JsonProperty] private RangedWeaponData[] rangedWeapons;
        [JsonProperty] private ArmorData[] armors;
        [JsonProperty] private RangedWeaponModData[] rangedMods;

        public Promise LoadItems()
        {
            var result = new Promise();
            AdressableLoader.LoadTextAssetAsync<ItemLib>(PATH).Then(data =>
            {
                InitData(data);
                result.Resolve();
            })
                .Catch(result.Reject);
            return result;
        }
        private void InitData(ItemLib data)
        {
            rangedWeapons = data.rangedWeapons;
            armors = data.armors;
            rangedMods = data.rangedMods;
            UpdateItems();
        }
        private void UpdateItems()
        {
            foreach (var item in rangedWeapons)
                Weapons.Add(new RangedWeapon(item));

            foreach (var item in armors)
                Armors.Add(new Armor(item));

            foreach (var item in rangedMods)
                Mods.Add(new RangedWeaponMod(item));

            Items.AddRange(Weapons);
            Items.AddRange(Armors);
            Items.AddRange(Mods);
        }
    }
}