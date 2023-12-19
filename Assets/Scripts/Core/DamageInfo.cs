using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Passives;
using System.Collections.Generic;

namespace Assets.Scripts.Core
{
    public class DamageInfo
    {
        public IDamage Damage { get; private set; }
        public List<Debuff> Debuffs { get; private set; }
        public IHitInfo HitInfo { get; private set; }
        public DamageInfo(IDamage damage, List<Debuff> debuffs, IHitInfo hitInfo)
        {
            Damage = damage;
            Debuffs = debuffs;
            HitInfo = hitInfo;
        }
    }
}