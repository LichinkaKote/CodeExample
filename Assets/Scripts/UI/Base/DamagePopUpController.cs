using Assets.Scripts.Core.Interfaces;
using UnityEngine;

namespace Assets.Scripts.UI.Base
{
    public class DamagePopUpController : MonoBehaviour
    {
        private const float ANIM_SPEED = 2f;
        private const float LIFE_TIME = 0.25f;

        private Transform worlUI;
        private DamagePopup damagePopupPF;

        public void Init(Transform worlUI)
        {
            this.worlUI = worlUI;
            damagePopupPF = Game.Prefabs.DamagePopup;
            Game.Events.actorGotDamage += OnEnemyGotDamage;
        }

        private void OnEnemyGotDamage((IHitInfo hitInfo, IDamage damage) dmg)
        {
            var inst = Instantiate(damagePopupPF, worlUI);
            inst.Show(dmg.damage, dmg.hitInfo.HitDirection, ANIM_SPEED);
            inst.transform.position = dmg.hitInfo.HitPosition;
            Destroy(inst.gameObject, LIFE_TIME);
        }
        private void OnDestroy()
        {
            Game.Events.actorGotDamage -= OnEnemyGotDamage;
        }
    }
}