using Assets.Scripts.Core;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.UI.Windows
{
    public class WindowController : MonoBehaviour
    {
        [SerializeField] private Transform content;
        [SerializeField] private WindowPrefabs windowPrefabsSO;

        private Stack<Window> windows = new Stack<Window>();
        public bool IsEmpty => windows.Count == 0;
        public T Open<T>(bool hideCurrent = true, params object[] p) where T : Window
        {
            var wnd = windowPrefabsSO.Windows.FirstOrDefault(x => x is T);
            var windInst = Instantiate(wnd, content) as T;
            windInst.close += Close;
            windInst.Init(p);
            ShowTopWindow(!hideCurrent);
            windows.Push(windInst);
            return windInst;
        }
        public void CloseTopWindow()
        {
            if (IsEmpty) return;

            windows.Peek().Close();
        }
        private void Close(Window window)
        {
            Debug.Log($"Close {window.name}");
            window.close -= Close;
            bool isTop = window == windows.Peek();
            if (isTop)
            {
                Destroy(windows.Pop().gameObject);
                ShowTopWindow(true);
            }
            else
                Destroy(window.gameObject);
        }
        private void ShowTopWindow(bool value)
        {
            if (windows.TryPeek(out Window window))
                window.gameObject.SetActive(value);
        }
    }
}