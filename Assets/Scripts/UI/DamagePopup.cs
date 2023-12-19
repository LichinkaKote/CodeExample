using Assets.Scripts.Core;
using Assets.Scripts.Core.Interfaces;
using System;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class DamagePopup : MonoBehaviour
    {
        [SerializeField] private TMP_Text dmg;
        private Vector3 moveDirection;
        public void Show(IDamage damage, Vector3 direction, float animSpeed = 1f)
        {
            var incacDir = ((Vector2)direction).GetInacuracyVector(45f);
            moveDirection = incacDir * animSpeed;
            SetText(damage);
        }
        private void SetText(IDamage damage)
        {
            if (damage.TotalDamage < 1f)
            {
                dmg.text = MathF.Round(damage.TotalDamage, 1).ToString();
            }
            else
            {
                dmg.text = Mathf.RoundToInt(damage.TotalDamage).ToString();
            }
            if (damage is IDamageColor dmgCol)
                dmg.color = dmgCol.DamageColor;

        }

        private void Update()
        {
            transform.position += moveDirection * Time.deltaTime;
        }
    }
}