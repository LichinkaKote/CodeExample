using UnityEngine;

namespace Assets.Scripts.Core.Interfaces
{
    public interface IHitInfo
    {
        public uint HittedObjectId { get; }
        public Vector3 HitDirection { get; }
        public Vector3 HitPosition { get; }
        public float RagdollForce { get; set; }
    }
}