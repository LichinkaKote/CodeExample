using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Base
{
    public class ImageTextView : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text text;

        public void Set(Sprite img, string txt)
        {
            SetImage(img);
            SetText(txt);
        }
        public void SetImage(Sprite img)
        {
            image.sprite = img;
        }
        public void SetText(string txt)
        {
            text.text = txt;
        }
        public void SetColor(float r, float g, float b, float a)
        {
            image.color = new Color(r, g, b, a);
        }
        public void SetColor(Color color)
        {
            image.color = color;
        }
    }
}