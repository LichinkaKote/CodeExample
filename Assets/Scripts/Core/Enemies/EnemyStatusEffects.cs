using Assets.Scripts.Core.Enums;
using Assets.Scripts.Core.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.Enemies
{
    public class EnemyStatusEffects : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer[] slots;
        private IStatusEffectController statusEffectController;
        private List<StatusEffectType> effects = new List<StatusEffectType>();
        private SpriteLib SpriteLib => Game.Library.SpriteLib;
        private void Awake()
        {
            foreach (var slot in slots)
            {
                slot.gameObject.SetActive(false);
            }
        }
        public void Init(IStatusEffectController statusEffectController)
        {
            this.statusEffectController = statusEffectController;
            statusEffectController.effetsUpdated += EffectsUpdated;
        }

        private void EffectsUpdated()
        {
            effects.Clear();
            for (int i = 0; i < statusEffectController.StatusEffects.Count; i++)
            {
                if (!effects.Contains(statusEffectController.StatusEffects[i].EffectType))
                    effects.Add(statusEffectController.StatusEffects[i].EffectType);
            }
            UpdateSlotsVisibility();
            DrawRffects();
        }
        private void UpdateSlotsVisibility()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].gameObject.SetActive(effects.Count > i);
            }
        }
        private void DrawRffects()
        {
            for (int i = 0; i < effects.Count && i < slots.Length; i++)
            {
                slots[i].sprite = GetSprite(effects[i]);
            }
        }
        private Sprite GetSprite(StatusEffectType type)
        {
            switch (type)
            {
                case StatusEffectType.PhysDoT:
                    return SpriteLib.UIicons[(int)UIicons.PhysicalDamage];
                case StatusEffectType.PoisonDoT:
                    return SpriteLib.UIicons[(int)UIicons.PoisonDamage];
                case StatusEffectType.FireDoT:
                    return SpriteLib.UIicons[(int)UIicons.FireDamage];
                case StatusEffectType.Slow:
                    return SpriteLib.UIicons[(int)UIicons.FrostDamage];
                default: return null;
            }
        }
        private void OnDestroy()
        {
            if (statusEffectController != null)
                statusEffectController.effetsUpdated -= EffectsUpdated;
        }
    }
}