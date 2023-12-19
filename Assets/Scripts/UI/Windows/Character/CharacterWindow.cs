using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Windows.Character
{
    public class CharacterWindow : Window
    {
        [SerializeField] private TMP_Text levelText;
        //[SerializeField] private AttributesPanel attributesPanel;
        //[SerializeField] private SkillsPanel skillsPanel;
        //[SerializeField] private PerksPanel perksPanel;

        private void Awake()
        {
            levelText.text = "Level: " + Game.Player.Experience.Level.ToString();
        }
    }
}