using Assets.Scripts.Core.Interfaces;
using UnityEngine;

namespace Assets.Scripts.UI.Windows.Equip
{
    public class ItemModView : MonoBehaviour
    {
        [SerializeField] private GameObject[] mod;
        private void Awake()
        {
            gameObject.SetActive(false);
        }
        public void UpdateMods(IModifyableItem item)
        {
            gameObject.SetActive(true);
            for (int i = 0; i < mod.Length; i++)
            {
                bool haveModCell = i < item.Mods.Length;
                mod[i].transform.parent.gameObject.SetActive(haveModCell);

                if (haveModCell)
                {
                    mod[i].SetActive(item.Mods[i] == null);
                }
            }
        }
    }
}