using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Base
{
    public class Tabs : MonoBehaviour
    {
        [SerializeField] private Button[] buttons;
        private UISlotSelector[] selectors;
        private UISlotSelector current;
        private bool haveSelectors;
        private void Awake()
        {
            selectors = new UISlotSelector[buttons.Length];
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].transform.TryGetComponent(out UISlotSelector sel))
                    selectors[i] = sel;
            }
            haveSelectors = selectors.Length == buttons.Length;
        }
        public void OnTabClick(int index, Action onClick)
        {
            buttons[index].onClick.RemoveAllListeners();
            buttons[index].onClick.AddListener(click);

            void click()
            {
                onClick.Invoke();
                if (haveSelectors)
                    Select(index);
            }
        }
        private void Select(int index)
        {
            if (current != null)
                current.Select(false);
            current = selectors[index];
            current.Select(true);
        }
        public void InvokeClick(int tabIndex)
        {
            buttons[tabIndex].onClick.Invoke();
        }
    }
}