using Assets.Scripts.Core;
using Assets.Scripts.Core.Enums;
using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.UI.Base;
using UnityEngine;

namespace Assets.Scripts.UI.Windows
{
    public class DamageTooltipWindow : Window
    {
        [SerializeField] private RectTransform panel;
        [SerializeField] private Transform panelContent;
        [SerializeField] private ImageTextView imageTextPf;
        private SpriteLib SpriteLib => Game.Library.SpriteLib;
        public void Init(IDamage damage, Vector2 position)
        {
            panel.position = position;
            AddDamage(damage.PhysicalDamage, "Physical: ", SpriteLib.UIicons[(int)UIicons.PhysicalDamage]);
            AddDamage(damage.PoisonDamage, "Poison: ", SpriteLib.UIicons[(int)UIicons.PoisonDamage]);
            AddDamage(damage.FireDamage, "Fire: ", SpriteLib.UIicons[(int)UIicons.FireDamage]);
            AddDamage(damage.FrostDamage, "Frost: ", SpriteLib.UIicons[(int)UIicons.FrostDamage]);
            AddDamage(damage.LightningDamage, "Lightning: ", SpriteLib.UIicons[(int)UIicons.LightningDamage]);
        }
        private void AddDamage(float damage, string prefix, Sprite icon)
        {
            var inst = Instantiate(imageTextPf, panelContent);
            inst.Set(icon, prefix + damage.ToString());
        }
    }
}