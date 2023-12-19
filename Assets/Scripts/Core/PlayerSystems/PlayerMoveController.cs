using UnityEngine;

namespace Assets.Scripts.Core.PlayerSystems
{
    public class PlayerMoveController : MonoBehaviour
    {
        private Stats stats;

        public void Init(Stats stats)
        {
            this.stats = stats;
        }
        private void Update()
        {
            Move();
        }

        private void Move()
        {
            transform.position += Time.deltaTime * stats.MoveSpeed * Game.GameManager.LevelInputListener.AxisInput.normalized;
        }
    }
}