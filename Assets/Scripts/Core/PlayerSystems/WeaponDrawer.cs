using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.PlayerSystems
{
    public class WeaponDrawer
    {
        private readonly SpriteRenderer spriteRenderer;
        private Dictionary<byte, Sprite> weaponImages = new Dictionary<byte, Sprite>();
        private Sprite[] Weapons => Game.Library.SpriteLib.Weapons;

        public WeaponDrawer(SpriteRenderer spriteRenderer)
        {
            this.spriteRenderer = spriteRenderer;
        }
        
        public void Clear()
        {
            weaponImages.Clear();
        }
        public void Add(byte key, int imageId)
        {
            weaponImages.Add(key, Weapons[imageId]);
        }
        public void SetIcon(byte key) 
        {
            spriteRenderer.sprite = weaponImages[key];
        }
    }
}