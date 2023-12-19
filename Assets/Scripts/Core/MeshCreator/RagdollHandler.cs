using Assets.Scripts.Core.Enemies;
using UnityEngine;

namespace Assets.Scripts.Core.MeshCreator
{
    public class RagdollHandler
    {
        public short Index { get; private set; }
        public Vector3 Velocity { get; private set; }
        public Vector3 Direction { get; private set; }
        public Vector3 Position { get; private set; }

        private float groundFriction = 350f;
        private byte ragdollSteps = 5;
        private float mass;
        private float ragdollForce;
        private float ragdollStepValue;

        public RagdollHandler(short index, Enemy enemy)
        {
            this.Index = index;
            Direction = enemy.LastHitInfo.HitDirection;
            mass = enemy.Mass;
            Velocity = enemy.LastHitInfo.HitDirection * enemy.LastHitInfo.RagdollForce;
            ragdollForce = enemy.LastHitInfo.RagdollForce;
            Position = enemy.transform.position;
            ragdollStepValue = ragdollForce / ragdollSteps;
        }

        public bool ReduceRagdollVelocity(out bool isRagdollStep)
        {
            ragdollForce -= mass * groundFriction * Time.deltaTime;
            Position += Velocity * Time.deltaTime;
            var ragdollThreshold = ragdollStepValue * ragdollSteps;
            isRagdollStep = ragdollForce <= ragdollThreshold;
            if (isRagdollStep)
            {
                ragdollSteps--;
            }
            bool ragdollEnd = ragdollForce <= 0;
            if (!ragdollEnd)
                Velocity = Direction * ragdollForce;
            return ragdollEnd;
        }
    }
}