using Assets.Scripts.Core.Data;

namespace Assets.Scripts.Core.Items
{
    public abstract class ItemMod : Item
    {
        protected ItemMod(ItemData data) : base(data)
        {
        }
    }
}