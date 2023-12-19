using Assets.Scripts.Core.Items;

namespace Assets.Scripts.Core.Interfaces
{
    public interface IModifyableItem
    {
        public ItemMod[] Mods { get; }
        public void ApplyMods();
        public bool IsCompatible(ItemMod mod);
    }
}