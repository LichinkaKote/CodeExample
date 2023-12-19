using Assets.Scripts.Core;
using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.PlayerSystems;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.UI.HUD
{
    public class ActionBar : MonoBehaviour
    {
        [SerializeField] private ActionBarItemView itemViewPf;
        private List<ActionBarItemView> itemViews = new List<ActionBarItemView>();

        public byte CurrentSlotIndex { get; private set; }

        public void Init(Player player, Inventory inventory)
        {
            Clear();
            var weapons = player.WeaponController.Presets;
            var abItems = inventory.GetActionBarItems();
            for (byte i = 0; i < abItems.Count; i++)
            {
                var inst = Instantiate(itemViewPf, transform);

                inst.SetBotText((i + 1).ToString());
                inst.SetIcon(abItems[i].Icon);

                if (weapons.ContainsKey(i) && weapons[i] is IWeaponReloable reloadable)
                {
                    reloadable.ReloadProgress.Subscribe(reloadProgress =>
                    {
                        inst.SetReloadProgress(reloadProgress / reloadable.ReloadTime);
                    }).AddTo(this);

                    reloadable.Ammo.Subscribe(ammo =>
                    {
                        inst.SetTopText(ammo.ToString());
                    }).AddTo(this);
                }
                itemViews.Add(inst);
            }
        }
        private void Clear()
        {
            transform.RemoveAllChilds();
            itemViews.Clear();
        }
        public void Select(byte index)
        {
            itemViews[CurrentSlotIndex].Selector.Select(false);
            itemViews[index].Selector.Select(true);
            CurrentSlotIndex = index;
        }
    }
}