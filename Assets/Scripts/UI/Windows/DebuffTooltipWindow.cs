using Assets.Scripts.Core.Enums;
using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Passives;
using Assets.Scripts.UI.Base;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Windows
{
    public class DebuffTooltipWindow : Window
    {
        [SerializeField] private ImageTextView header;
        [SerializeField] private TMP_Text desc;
        [SerializeField] private ImageTextView effectPf;
        [SerializeField] private Transform effectContent;
        private GameObject EffectPanel => effectContent.parent.gameObject;
        public void Init(Debuff debuff)
        {
            header.SetText(debuff.Name);
            desc.text = debuff.Description;

            bool isStatusEffect = debuff is IStatusEffect;
            EffectPanel.SetActive(isStatusEffect);

            if (isStatusEffect)
                ShowEffects(debuff as IStatusEffect, debuff);
        }
        private void ShowEffects(IStatusEffect effect, Debuff debuff)
        {
            switch (effect.EffectType)
            {
                case StatusEffectType.PhysDoT:
                    AddEffect($"Does {debuff.Magnitude * debuff.Duration} pysical damage over {debuff.Duration} sec", null);
                    break;
                case StatusEffectType.PoisonDoT:
                    AddEffect($"Does {debuff.Magnitude * debuff.Duration} poison damage over {debuff.Duration} sec", null);
                    break;
                case StatusEffectType.FireDoT:
                    AddEffect($"Does {debuff.Magnitude * debuff.Duration} fire damage over {debuff.Duration} sec", null);
                    break;
                case StatusEffectType.Slow:
                    AddEffect($"Slow victim's movement speed by {debuff.Magnitude * 100f}% for {debuff.Duration} sec", null);
                    break;
            }
        }
        private void AddEffect(string text, Sprite icon)
        {
            var inst = Instantiate(effectPf, effectContent);
            inst.Set(icon, text);
        }
    }
}