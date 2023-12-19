using Assets.Scripts.Core.Enemies;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.MeshCreator
{
    public class BodyMeshCreator : AbstractMeshCreator
    {
        private uint enemyID;
        private float Rotation => UnityEngine.Random.Range(-60f, 60f);
        private List<RagdollHandler> ragdolls = new List<RagdollHandler>();
        protected override byte SortingOrder => 10;
        public override void Init(Material material, byte materialRows = 1, ushort quadsCount = 15000)
        {
            base.Init(material, materialRows, quadsCount);
            Game.Events.actorDeath += SpawnBodyOnDeath;
        }

        private void SpawnBodyOnDeath(Enemy enemy)
        {
            if (enemyID != enemy.ID) return;
            var index = AddQuad(enemy.transform.position, size: Vector2.one * enemy.Size, rotation: Rotation, flip: !enemy.IsLookLeft.Value);
            ragdolls.Add(new RagdollHandler(index, enemy));
        }
        public void SetEnemyID(uint id)
        {
            enemyID = id;
        }
        private void OnDestroy()
        {
            Game.Events.actorDeath -= SpawnBodyOnDeath;
        }

        public event Action<(Vector3 position, Vector3 direction)> ragdollStep;
        private void Update()
        {
            for (int i = 0; i < ragdolls.Count; i++)
            {
                MoveQuad(ragdolls[i].Index, ragdolls[i].Velocity * Time.deltaTime);
            }
            ApplyQuadsMovement();

            for (int i = 0; i < ragdolls.Count; i++)
            {
                bool ragdollEnd = ragdolls[i].ReduceRagdollVelocity(out bool isRagdollStep);
                if (isRagdollStep)
                {
                    ragdollStep?.Invoke((ragdolls[i].Position, ragdolls[i].Direction));
                }

                if (ragdollEnd)
                {
                    ragdolls.RemoveAt(i);
                    i--;
                }
            }
        }

    }
}