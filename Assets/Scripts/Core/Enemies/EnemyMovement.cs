using UniRx;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Core.Enemies
{
    public class EnemyMovement : MonoBehaviour
    {
        private Transform player;
        private Vector3 distance;
        private Vector3 direction;
        private Stats stats;
        public ReactiveProperty<bool> IsLookLeft { get; private set; } = new ReactiveProperty<bool>(true);
        public float DistanceToPlayer { get; private set; }
        public bool Stop { get; set; }
        //Rigidbody2D rb;
        private NavMeshAgent navMeshAgent;
        private void Awake()
        {
            player = Game.GameManager.PlayerTranform;
            //rb = GetComponent<Rigidbody2D>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }
        private void Start()
        {
            //UpdatePositionValues();
            navMeshAgent.enabled = true;
        }
        private void OnEnable()
        {
            UpdatePositionValues();
        }
        public void Init(Stats stats)
        {
            this.stats = stats;
        }
        /*private void Update()
        {
            UpdatePositionValues();
            UpdateLookDir();
            if (!Stop)
                transform.position += direction * Time.deltaTime * self.MoveSpeed;
        }*/
        private void FixedUpdate()
        {
            UpdatePositionValues();
            UpdateLookDir();
            navMeshAgent.speed = stats.MoveSpeed;
            if (!Stop)
            {
                navMeshAgent.SetDestination(player.position);
                //rb.MovePosition(transform.position + direction * Time.fixedDeltaTime * stats.MoveSpeed);
            }
            else
            {
                navMeshAgent.ResetPath();
            }
        }

        private void UpdatePositionValues()
        {

            distance = player.position - transform.position;
            DistanceToPlayer = distance.magnitude;
            direction = distance.normalized;
        }
        private void UpdateLookDir()
        {
            bool isLookLeft = direction.x <= 0;

            if (IsLookLeft.Value != isLookLeft)
            {
                IsLookLeft.Value = isLookLeft;
            }
        }
        public void Teleport(Vector3 pos)
        {
            navMeshAgent.transform.position = pos;
            navMeshAgent.enabled = false;
            navMeshAgent.enabled = true;
        }
    }
}