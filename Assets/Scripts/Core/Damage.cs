using Assets.Scripts.Core.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public struct Damage : IDamage, IDamageColor
    {
        private float physicalDamage;
        private float poisonDamage;
        private float fireDamage;
        private float frostDamage;
        private float lightningDamage;
        private float pureDamage;

        public float PhysicalDamage { get { return physicalDamage; } set { physicalDamage = value; UpdateTotal(); } }
        public float PoisonDamage { get { return poisonDamage; } set { poisonDamage = value; UpdateTotal(); } }
        public float FireDamage { get { return fireDamage; } set { fireDamage = value; UpdateTotal(); } }
        public float FrostDamage { get { return frostDamage; } set { frostDamage = value; UpdateTotal(); } }
        public float LightningDamage { get { return lightningDamage; } set { lightningDamage = value; UpdateTotal(); } }
        public float PureDamage { get { return pureDamage; } set { pureDamage = value; UpdateTotal(); } }
        public float TotalDamage { get; private set; }

        public Color DamageColor { get; private set; }

        private void UpdateTotal()
        {
            TotalDamage = PhysicalDamage + PoisonDamage + FireDamage + FrostDamage + LightningDamage + PureDamage;
            SetColor();
        }

        private void SetColor()
        {
            Dictionary<Color, float> damages = new Dictionary<Color, float>();
            damages.Add(new Color(1f, 0.14f, 0.14f), PhysicalDamage);
            damages.Add(new Color(0.2f, .6f, 0.09f), PoisonDamage);
            damages.Add(new Color(1f, .23f, 1f), FireDamage);
            damages.Add(new Color(0f, .4f, 1f), FrostDamage);
            damages.Add(new Color(1f, 1f, 0f), LightningDamage);
            damages.Add(new Color(1f, 0f, 1f), PureDamage);
            float maxVal = -1f;
            foreach (var damage in damages)
            {
                if (damage.Value > maxVal)
                {
                    DamageColor = damage.Key;
                    maxVal = damage.Value;
                }
            }
        }
    }
}