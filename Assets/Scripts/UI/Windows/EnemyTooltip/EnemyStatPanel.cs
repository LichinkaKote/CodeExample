using Assets.Scripts.Core;
using Assets.Scripts.UI.Base;
using UnityEngine;

namespace Assets.Scripts.UI.Windows.EnemyTooltip
{
    public class EnemyStatPanel : MonoBehaviour
    {
        [SerializeField] private Transform content;
        [SerializeField] private ImageTextView statPf;

        public void Clear()
        {
            content.RemoveAllChilds();
        }
        public void AddStat(string text, Sprite icon, Color color = default)
        {
            var inst = Instantiate(statPf, content);
            inst.Set(icon, text);

            if (color != default)
                inst.SetColor(color);
        }
    }
}