using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Windows
{
    public class WindowCloser : MonoBehaviour
    {
        private Button btn;
        [SerializeField] private Window window;
        private void Awake()
        {
            btn = GetComponent<Button>();
            btn.onClick.AddListener(CloseWindow);
        }
        private void CloseWindow()
        {
            window.Close();
        }
    }
}