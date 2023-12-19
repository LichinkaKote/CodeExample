using Assets.Scripts.Core.Enums;
using Assets.Scripts.Core.Passives;
using System.Collections.Generic;

namespace Assets.Scripts.Core.Interfaces
{
    public interface IWeaponModifier
    {
        public float AttackSpeedMod { get; }
        public List<Debuff> Debuffs { get; }
        public WeaponType ApplyType { get; }
    }
}