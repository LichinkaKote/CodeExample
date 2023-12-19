using Assets.Scripts.Core.Enemies;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Core.Utils
{
    public class ValueTracker : MonoBehaviour
    {
        [SerializeField] TMP_Text m_Text;
        [SerializeField] Enemy enemy;

        private void Update()
        {
            //m_Text.text = Math.Round(enemy.MoveSpeed, 1).ToString();
        }
    }
}