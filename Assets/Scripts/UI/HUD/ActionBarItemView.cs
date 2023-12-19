using Assets.Scripts.UI.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.HUD
{
    public class ActionBarItemView : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Image reload;
        [SerializeField] private TMP_Text botText;
        [SerializeField] private TMP_Text topText;
        [SerializeField] private UISlotSelector selector;

        public UISlotSelector Selector { get { return selector; } }
        public void SetIcon(Sprite sprite)
        {
            icon.sprite = sprite;
        }
        public void SetReloadProgress(float progress)
        {
            reload.fillAmount = progress;
        }
        public void SetBotText(string text)
        {
            botText.text = text;
        }
        public void SetTopText(string text)
        {
            topText.text = text;
        }
    }
}