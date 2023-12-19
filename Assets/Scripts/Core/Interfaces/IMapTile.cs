using UnityEngine;

namespace Assets.Scripts.Core.Interfaces
{
    public interface IMapTile
    {
        public Sprite Sprite { get; }
        public float Rotation { get; }
        public bool Walkable { get; }
    }
}