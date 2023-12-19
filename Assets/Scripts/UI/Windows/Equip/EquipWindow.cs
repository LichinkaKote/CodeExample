using Assets.Scripts.Core;
using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Items;
using Assets.Scripts.UI.Base;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Windows.Equip
{
    public class EquipWindow : Window
    {
        [SerializeField] private AutoGridLayout inventoryGrid;
        [SerializeField] private EquippedItemsPanel equippedWeapons, equippedArmors;
        [SerializeField] private InventoryItemView itemViewPf;
        [SerializeField] private Button okBtn;
        [SerializeField] private EquipmentInfoPanel infoPanel;
        [SerializeField] private Tabs inventoryTabs;

        private Dictionary<Item, InventoryItemView> inventoryView = new Dictionary<Item, InventoryItemView>();
        private InventoryItemView selectedView;

        private Inventory Inventory => Game.Player.Inventory;
        private ItemLib ItemLib => Game.Library.ItemLib;

        private void Awake()
        {
            equippedWeapons.SetCapacity(Inventory.ActionBarSize);
            equippedArmors.SetCapacity(Inventory.ArmorSlotsCount);
            BindTabs();
            okBtn.onClick.AddListener(OnOkClick);
        }
        private void Start()
        {
            AddEquippedItemsToBelts();
            inventoryTabs.InvokeClick(0);
        }
        private void BindTabs()
        {
            inventoryTabs.OnTabClick(0, () =>
            {
                UpdateInventory(ItemLib.Items);
            });
            inventoryTabs.OnTabClick(1, () =>
            {
                UpdateInventory(ItemLib.Weapons);
            });
            inventoryTabs.OnTabClick(2, () =>
            {
                UpdateInventory(ItemLib.Armors);
            });
        }
        private void UpdateInventory(List<Item> items)
        {
            inventoryGrid.transform.RemoveAllChilds();
            inventoryView.Clear();
            foreach (Item item in items)
            {
                var view = Instantiate(itemViewPf, inventoryGrid.transform);
                view.Init(item.Icon);
                if (item is IModifyableItem modItem)
                    view.UpdaveModsView(modItem);
                inventoryView.Add(item, view);

                if (equippedWeapons.Contains(item) || equippedArmors.Contains(item))
                    view.SetRemoveMode();

                BindIventoryView(view, item);

                view.ShowModBtn(item is IModifyableItem);

                if (!(item is Armor || item is Weapon))
                    view.SetNoButtonsMode();
            }
            FillInventoryWithEmptySlots(5);

            if (inventoryView.Count > 0)
                inventoryView.First().Value.InvokeClick();
        }
        private void BindIventoryView(InventoryItemView view, Item item)
        {
            view.OnClick(() =>
            {
                SelectItemView(view);
                infoPanel.UpdateItemInfo(item);
            });
            view.OnAddClick(() =>
            {
                if (TryAddItemToBelt(item))
                    view.SetRemoveMode();
            });
            view.OnRemoveClick(() =>
            {
                view.SetAddMode();
                RemoveItemFromBelt(item);
            });
            view.OnModClick(() =>
            {
                ModifyItem(item);
            });
        }
        private void ModifyItem(Item item)
        {
            if (item is IModifyableItem modItem)
            {
                var win = Game.WindowController.Open<ItemModifyWindow>(false);
                win.Init(modItem, ItemLib.Mods.Where(mod => modItem.IsCompatible(mod)).ToList());
                win.AfterClose(() =>
                {
                    infoPanel.UpdateCurrentItemInfo();
                    infoPanel.UpdatePlayerInfo();
                    inventoryView[item].UpdaveModsView(modItem);
                    equippedWeapons.UpdateModsView(modItem);
                    equippedArmors.UpdateModsView(modItem);
                });
            }
        }
        private void SelectItemView(InventoryItemView view)
        {
            if (selectedView != null)
                selectedView.Select(false);
            view.Select(true);
            selectedView = view;
        }
        private void FillInventoryWithEmptySlots(byte count)
        {
            for (byte i = 0; i < count; i++)
            {
                var view = Instantiate(itemViewPf, inventoryGrid.transform);
                view.Init(null);
            }
        }
        private bool TryAddItemToBelt(Item item)
        {
            bool isArmor = item is Armor;
            EquippedItemsPanel panel = isArmor ? equippedArmors : equippedWeapons;

            if (panel.Contains(item) || panel.IsFull) return false;
            panel.TryAddItem(item, onClick, onRemoveClick, () => ModifyItem(item));

            void onClick()
            {
                SelectItemView(panel.GetItemView(item));
                infoPanel.UpdateItemInfo(item);
            }
            void onRemoveClick()
            {
                panel.RemoveItem(item);
                RemoveItemFromBelt(item);
                if (inventoryView.ContainsKey(item))
                    inventoryView[item].SetAddMode();
            }
            if (isArmor) EquipArmor(item);
            infoPanel.UpdatePlayerInfo();
            return true;
        }
        private void RemoveItemFromBelt(Item item)
        {
            bool isArmor = item is Armor;
            EquippedItemsPanel panel = isArmor ? equippedArmors : equippedWeapons;
            panel.RemoveItem(item);
            if (isArmor) UnEquipArmor(item);
            infoPanel.UpdatePlayerInfo();
        }
        private void EquipArmor(Item armor)
        {
            Inventory.TryAddToArmorBar(armor);
        }
        private void UnEquipArmor(Item armor)
        {
            Inventory.RemoveFromArmorBar(armor);
        }
        private void OnOkClick()
        {
            Inventory.ClearActionBar();
            foreach (var item in equippedWeapons.GetItems())
                Inventory.TryAddToActionBar(item);
            Close();
        }
        private void AddEquippedItemsToBelts()
        {
            foreach (var item in Inventory.GetActionBarItems())
                TryAddItemToBelt(item);
            foreach (var item in Inventory.GetArmorBarItems())
                TryAddItemToBelt(item);
        }
    }
}