using System;
using UnityEngine;

namespace Assets.Scripts.UI.Windows.Character
{
    public class SkillsPanel : MonoBehaviour
    {
        [SerializeField] private AttributeBar mele, ranged;

        private void Awake()
        {
            var skills = Game.Player.Skills;

            string maxSkill = skills.MaxSkill.ToString() + "%";
            mele.SetName("Mele combat");
            mele.SetMaxValue(skills.MaxSkill, customText: maxSkill);
            mele.SetValue(skills.MeleSkill, 1);

            ranged.SetName("Ranged combat");
            ranged.SetMaxValue(skills.MaxSkill, customText: maxSkill);
            ranged.SetValue(skills.RangedSkill, 1);
        }
    }
}