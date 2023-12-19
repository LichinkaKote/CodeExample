using System;
using UnityEngine;

namespace Assets.Scripts.UI.Windows
{
    public abstract class Window : MonoBehaviour
    {
        [SerializeField] protected Transform content;
        protected object[] windowParams;

        public Action<Window> close;
        private Action closeCallback;
        private Action afterCloseCallback;
        public void Close()
        {
            closeCallback?.Invoke();
            close?.Invoke(this);
            afterCloseCallback?.Invoke();
        }
        public void OnClose(Action callback)
        {
            closeCallback = callback;
        }
        public void AfterClose(Action callback)
        {
            afterCloseCallback = callback;
        }
        public void Init(object[] p)
        {
            windowParams = p;
        }
    }
}