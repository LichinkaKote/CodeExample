using Assets.Scripts.Core.Enums;
using Assets.Scripts.Core.PlayerSystems;
using System;
using System.Drawing;
using TMPro;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.UI.Windows.Character
{
    public class AttributesPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text learningPoints;
        [SerializeField] private AttributeBar strength, stamina, agility, perception;
        private PlayerAttributes Attributes => Game.Player.Attributes;
        private Sprite[] uiIcons => Game.Library.SpriteLib.UIicons;
        private void Awake()
        {
            InitBar("Strength", strength, Attributes.Strength, Attributes.AddStrength, uiIcons[(int)UIicons.Strength]);
            InitBar("Stamina", stamina, Attributes.Stamina, Attributes.AddStamina, uiIcons[(int)UIicons.Stamina]);
            InitBar("Agility", agility, Attributes.Agility, Attributes.AddAgility, uiIcons[(int)UIicons.Agility]);
            InitBar("Perception", perception, Attributes.Perception, Attributes.AddPerception, uiIcons[(int)UIicons.Perception]);
            Attributes.LearningPoints.Subscribe(OnLPChanged).AddTo(this);
        }

        private void OnLPChanged(short lp)
        {
            string points = lp > 0 ? $"<color=#66CD51>{lp}<color=#FFFFFF>" : lp.ToString();
            learningPoints.text = $"You have {points} free attribute points";
        }

        private void InitBar(string name, AttributeBar bar, ReactiveProperty<short> attribute, Action click, Sprite icon)
        {
            var maxValue = PlayerAttributes.MAX_ATTRIBUTE_VALLUE;
            bar.SetMaxValue(maxValue);
            attribute.Subscribe((attr) =>
            {
                bar.SetValue(attr);
                UpdatePlusBtnVisibility();
            }).AddTo(this);
            bar.SetName(name);
            bar.SetIcon(icon);
            bar.OnClick(click);
        }
        private void UpdatePlusBtnVisibility()
        {
            bool hide = Attributes.LearningPoints.Value <= 0;
            var max = PlayerAttributes.MAX_ATTRIBUTE_VALLUE;
            strength.DisablePlusBtn(hide || Attributes.Strength.Value >= max);
            stamina.DisablePlusBtn(hide || Attributes.Stamina.Value >= max);
            agility.DisablePlusBtn(hide || Attributes.Agility.Value >= max);
            perception.DisablePlusBtn(hide || Attributes.Perception.Value >= max);
        }
    }
}