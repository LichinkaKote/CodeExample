using Assets.Scripts.Core.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Core
{
    public class Inventory
    {

        private List<Item> items = new List<Item>();
        private List<Item> actionBar = new List<Item>();
        private List<Item> armorBar = new List<Item>();

        public int ActionBarItemCount => actionBar.Count;
        public ushort InventorySize { get; set; } = 16;
        public byte ActionBarSize { get; set; } = 5;
        public byte ArmorSlotsCount { get; set; } = 2;

        public event Action armorChanged;

        public bool TryAddItem(Item item)
        {
            if (items.Count < InventorySize)
            {
                items.Add(item);
                return true;
            }
            return false;
        }
        public void ClearActionBar()
        {
            actionBar.Clear();
        }
        public bool TryAddToActionBar(Item item)
        {
            if (actionBar.Count < ActionBarSize)
            {
                actionBar.Add(item);
                return true;
            }
            return false;
        }
        public bool TryGetActionBarItem(byte index, out Item item)
        {
            item = null;
            if (actionBar.Count <= index)
                return false;
            item = actionBar[index];
            return true;
        }
        public bool TryAddToArmorBar(Item item)
        {
            if (armorBar.Count < ArmorSlotsCount)
            {
                armorBar.Add(item);
                armorChanged?.Invoke();
                return true;
            }
            return false;
        }
        public void RemoveFromArmorBar(Item item)
        {
            armorBar.Remove(item);
            armorChanged?.Invoke();
        }
        public void ClearArmorBar()
        {
            armorBar.Clear();
        }
        public List<Item> GetActionBarItems()
        {
            return actionBar.ToList();
        }
        public List<Item> GetArmorBarItems()
        {
            return armorBar.ToList();
        }
    }
}