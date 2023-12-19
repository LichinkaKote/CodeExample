using Assets.Scripts.Core.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public struct HitInfo : IHitInfo
    {
        public uint HittedObjectId { get; set; }
        public Vector3 HitDirection { get; set; }
        public Vector3 HitPosition { get; set; }
        public float RagdollForce { get; set; }
    }
    public struct RangedHitInfo : IRangedHitInfo
    {
        public int ProjectileID { get; set; }
        public bool Penetrated { get; set; }
        public uint HittedObjectId { get; set; }
        public Vector3 HitDirection { get; set; }
        public Vector3 HitPosition { get; set; }
        public float RagdollForce { get; set; }
    }
}