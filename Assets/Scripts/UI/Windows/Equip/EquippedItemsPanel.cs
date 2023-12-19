using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Items;
using Assets.Scripts.UI.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.UI.Windows.Equip
{
    public class EquippedItemsPanel : MonoBehaviour
    {
        [SerializeField] private AutoGridLayout itemGrid;
        [SerializeField] private InventoryItemView itemViewPf;

        private Dictionary<Item, InventoryItemView> items = new Dictionary<Item, InventoryItemView>();
        private List<GameObject> emptySlots = new List<GameObject>();

        public byte Capacity { get; private set; }
        public bool IsFull => items.Count >= Capacity;
        public void SetCapacity(byte capacity)
        {
            Capacity = capacity;
            FillWithEmptySlots();
        }
        public bool TryAddItem(Item item, Action onClick, Action onRemoveClick, Action onModClick)
        {
            if (items.ContainsKey(item) || IsFull) return false;

            var view = Instantiate(itemViewPf, itemGrid.transform);
            view.Init(item.Icon);
            if (item is IModifyableItem modItem)
                view.UpdaveModsView(modItem);
            items.Add(item, view);
            view.SetRemoveMode();
            view.OnClick(() =>
            {
                onClick.Invoke();
            });
            view.OnRemoveClick(() =>
            {
                onRemoveClick.Invoke();
            });
            view.OnModClick(() =>
            {
                onModClick.Invoke();
            });
            view.ShowModBtn(item is IModifyableItem);
            FillWithEmptySlots();
            return true;
        }
        public void RemoveItem(Item item)
        {
            if (items.ContainsKey(item))
            {
                Destroy(items[item].gameObject);
                items.Remove(item);
                FillWithEmptySlots();
            }
        }
        private void FillWithEmptySlots()
        {
            foreach (var go in emptySlots)
                Destroy(go);
            emptySlots.Clear();
            for (int i = items.Count; i < Capacity; i++)
            {
                var view = Instantiate(itemViewPf, itemGrid.transform);
                view.Init(null);
                emptySlots.Add(view.gameObject);
            }
        }
        public bool Contains(Item item)
        {
            return items.ContainsKey(item);
        }
        public InventoryItemView GetItemView(Item item)
        {
            return items[item];
        }

        public List<Item> GetItems() => items.Keys.ToList();
        public void UpdateModsView(IModifyableItem item)
        {
            if (items.TryGetValue(item as Item, out InventoryItemView view))
                view.UpdaveModsView(item);
        }
    }
}