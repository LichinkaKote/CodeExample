using System;
using System.Collections.Generic;

namespace Assets.Scripts.Core.Interfaces
{
    public interface IStatusEffectController
    {
        public event Action effetsUpdated;
        public List<IStatusEffect> StatusEffects { get; }
    }
}