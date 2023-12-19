using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.UI.Base;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Windows.Equip
{
    public class InventoryItemView : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Button btnMain, btnAdd, btnRemove, btnMod;
        [SerializeField] private GameObject panel;
        [SerializeField] private UISlotSelector selector;
        [SerializeField] private ItemModView modView;
        public void Init(Sprite sprite)
        {
            bool haveSprite = sprite != null;
            if (haveSprite)
            {
                icon.sprite = sprite;
            }
            panel.SetActive(haveSprite);
        }
        public void UpdaveModsView(IModifyableItem item)
        {
            modView.UpdateMods(item);
        }
        public void OnClick(Action action)
        {
            btnMain.onClick.RemoveAllListeners();
            btnMain.onClick.AddListener(action.Invoke);
        }
        public void OnAddClick(Action action)
        {
            btnAdd.onClick.RemoveAllListeners();
            btnAdd.onClick.AddListener(action.Invoke);
        }
        public void OnRemoveClick(Action action)
        {
            btnRemove.onClick.RemoveAllListeners();
            btnRemove.onClick.AddListener(action.Invoke);
        }
        public void OnModClick(Action action)
        {
            btnMod.onClick.RemoveAllListeners();
            btnMod.onClick.AddListener(action.Invoke);
        }
        public void Select(bool selected)
        {
            selector.Select(selected);
        }
        public void InvokeClick()
        {
            btnMain.onClick.Invoke();
        }
        public void SetAddMode()
        {
            btnAdd.gameObject.SetActive(true);
            btnRemove.gameObject.SetActive(false);
        }
        public void SetRemoveMode()
        {
            btnAdd.gameObject.SetActive(false);
            btnRemove.gameObject.SetActive(true);
        }
        public void ShowModBtn(bool show)
        {
            btnMod.gameObject.SetActive(show);
        }
        public void SetNoButtonsMode()
        {
            btnAdd.gameObject.SetActive(false);
            btnRemove.gameObject.SetActive(false);
            ShowModBtn(false);
        }
    }
}