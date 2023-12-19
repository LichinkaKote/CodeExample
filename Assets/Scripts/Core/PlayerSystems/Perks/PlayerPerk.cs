using Assets.Scripts.Core.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Core.PlayerSystems.Perks
{
    public abstract class PlayerPerk : IUIIcon
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Sprite Icon => Game.Library.SpriteLib.UIicons[IconID];
        public ushort ReqLevel { get; set; }
        public int IconID { get; set; }
    }
}