using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Items;
using Assets.Scripts.UI.Base;
using Assets.Scripts.UI.Windows.Equip;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.Windows
{
    public class ItemModifyWindow : Window
    {
        [SerializeField] private ImageTextView itemView;
        [SerializeField] private InventoryItemView modPf;
        [SerializeField] private Transform itemGrid, inventoryGrid;

        private Dictionary<InventoryItemView, ItemMod> modSlots = new Dictionary<InventoryItemView, ItemMod>();
        private Dictionary<InventoryItemView, ItemMod> inventory = new Dictionary<InventoryItemView, ItemMod>();

        public void Init(IModifyableItem item, List<ItemMod> availableMods)
        {
            InitItem(item);
            InitInventory(availableMods);
            OnClose(() => ApplyMods(item));
        }
        private void ApplyMods(IModifyableItem item)
        {
            int index = 0;
            foreach (var slot in modSlots)
            {
                item.Mods[index] = slot.Value;
                index++;
            }
            item.ApplyMods();
        }
        public void InitItem(IModifyableItem item)
        {
            if (item is IUIIcon uiIcon)
                itemView.Set(uiIcon.Icon, uiIcon.Name);
            for (int i = 0; i < item.Mods.Length; i++)
            {
                var view = Instantiate(modPf, itemGrid);
                var itemMod = item.Mods[i];
                view.OnRemoveClick(() =>
                {
                    modSlots[view] = null;
                    UpdateSlots();
                    UpdateInventory();
                });
                modSlots.Add(view, itemMod);
            }
            UpdateSlots();
        }

        private void InitInventory(List<ItemMod> availableMods)
        {
            for (int i = 0; i < availableMods.Count; i++)
            {
                var view = Instantiate(modPf, inventoryGrid);
                inventory.Add(view, availableMods[i]);
                var mod = availableMods[i];
                if (mod is IUIIcon uiIcon)
                    view.Init(uiIcon.Icon);

                if (modSlots.ContainsValue(mod))
                    view.SetRemoveMode();
                else
                    view.SetAddMode();

                view.OnRemoveClick(() =>
                {
                    foreach (var slot in modSlots)
                    {
                        if (slot.Value == inventory[view])
                        {
                            modSlots[slot.Key] = null;
                            break;
                        }
                    }
                    view.SetAddMode();
                    UpdateSlots();
                });
                view.OnAddClick(() =>
                {
                    if (TryAddToSlots(inventory[view]))
                    {
                        view.SetRemoveMode();
                        UpdateSlots();
                    }
                });
            }
        }
        private bool TryAddToSlots(ItemMod mod)
        {
            foreach (var slot in modSlots)
            {
                if (slot.Value == null)
                {
                    modSlots[slot.Key] = mod;
                    return true;
                }
            }
            return false;
        }
        private void UpdateSlots()
        {
            foreach (var slot in modSlots)
            {
                var view = slot.Key;
                var mod = slot.Value;
                if (mod == null)
                    view.Init(null);
                else
                {
                    if (mod is IUIIcon uiIcon)
                        view.Init(uiIcon.Icon);

                    view.SetRemoveMode();
                }
            }
        }
        private void UpdateInventory()
        {
            foreach (var slot in inventory)
            {
                if (modSlots.ContainsKey(slot.Key))
                    slot.Key.SetRemoveMode();
                else
                    slot.Key.SetAddMode();
            }
        }
    }
}