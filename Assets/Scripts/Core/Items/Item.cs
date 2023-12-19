using Assets.Scripts.Core.Data;
using Assets.Scripts.Core.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Core.Items
{
    public abstract class Item : IUIIcon
    {
        public int ID { get; private set; }
        public byte Quality { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int ImageID { get; private set; }
        public Sprite Icon => Game.Library.SpriteLib.Items[ImageID];
        public Item(ItemData data)
        {
            ID = data.id;
            Name = data.name;
            Description = data.description;
            ImageID = data.imgid;
            if (data.quality.HasValue) Quality = data.quality.Value;
        }
    }
}