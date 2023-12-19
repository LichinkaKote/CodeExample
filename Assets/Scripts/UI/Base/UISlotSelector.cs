using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Base
{
    public class UISlotSelector : MonoBehaviour
    {
        [SerializeField] private Image targetImage;
        [SerializeField] private Color selected;
        [SerializeField] private Color unselected;

        public void Select(bool value)
        {
            if (value)
                targetImage.color = selected;
            else
                targetImage.color = unselected;
        }
    }
}