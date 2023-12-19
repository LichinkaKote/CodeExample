using Assets.Scripts.Core.Items;
using Assets.Scripts.UI.Base;
using UnityEngine;

namespace Assets.Scripts.UI.Windows.Equip
{
    public class EquipmentInfoPanel : MonoBehaviour
    {
        [SerializeField] private Tabs tabs;
        [SerializeField] private ItemInfo itemInfo;
        [SerializeField] private PlayerInfo playerInfo;

        private void Awake()
        {
            playerInfo.Init(Game.Player.Statistics);
            tabs.OnTabClick(0, () =>
            {
                SwitchToPlayer(false);
                UpdateCurrentItemInfo();
            });
            tabs.OnTabClick(1, ShowPlayerInfo);
        }

        private void Start()
        {
            tabs.InvokeClick(0);
        }
        public void UpdateItemInfo(Item item) => itemInfo.UpdateItemInfo(item);
        public void UpdateCurrentItemInfo() => itemInfo.UpdateCurrentItemInfo();
        public void UpdatePlayerInfo() => playerInfo.UpdatePlayerInfo();

        private void ShowPlayerInfo()
        {
            SwitchToPlayer(true);
            UpdatePlayerInfo();
        }
        private void SwitchToPlayer(bool value)
        {
            itemInfo.gameObject.SetActive(!value);
            playerInfo.gameObject.SetActive(value);
        }
    }
}