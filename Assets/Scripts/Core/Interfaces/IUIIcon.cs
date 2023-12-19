using UnityEngine;

namespace Assets.Scripts.Core.Interfaces
{
    public interface IUIIcon
    {
        public string Name { get; }
        public string Description { get; }
        public Sprite Icon { get; }
    }
}