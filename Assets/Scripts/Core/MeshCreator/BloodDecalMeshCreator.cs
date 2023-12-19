using Assets.Scripts.Core.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Core.MeshCreator
{
    public class BloodDecalMeshCreator : AbstractMeshCreator
    {
        private uint enemyID;
        private byte decalsCount = 2;
        private float DecalSize => Random.Range(0.4f, 0.8f);
        private float DecalRotation => Random.Range(0f, 360f);
        private Vector3 Offset => new Vector2(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f));
        protected override byte SortingOrder => 9;

        public override void Init(Material material, byte materialRows = 1, ushort quadsCount = 15000)
        {
            base.Init(material, materialRows, quadsCount);
            Game.Events.actorHit += SpawnBloodDecal;
        }
        public void SetEnemyID(uint id)
        {
            enemyID = id;
        }
        private void SpawnBloodDecal(DamageInfo info)
        {
            if (enemyID != info.HitInfo.HittedObjectId) return;
            for (int i = 0; i < decalsCount; i++)
            {
                int xMatPos = Random.Range(0, MaterialCols);
                int yMatPos = Random.Range(0, MaterialRows);
                Vector3 dir;
                if (info.HitInfo is IRangedHitInfo rangeHit)
                {
                    dir = rangeHit.Penetrated ? rangeHit.HitDirection : -rangeHit.HitDirection;
                }
                else
                {
                    dir = -info.HitInfo.HitDirection;
                }

                AddQuad(info.HitInfo.HitPosition + dir * 0.8f + Offset, Vector2.one * DecalSize, DecalRotation, xMatPos, yMatPos);
            }
        }

        private void OnDestroy()
        {
            Game.Events.actorHit -= SpawnBloodDecal;
        }
    }
}