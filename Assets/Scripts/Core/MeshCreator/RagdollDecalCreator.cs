using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Core.MeshCreator
{
    public class RagdollDecalCreator : AbstractMeshCreator
    {
        public void SpawnDecal((Vector3 position, Vector3 direction) decalParams)
        {
            var angle = Vector2.Angle(Vector2.up, decalParams.direction);
            if (decalParams.direction.x > 0f)
                angle = -angle;

            AddQuad(decalParams.position, new Vector2(2f, 1.5f), angle);
        }
    }
}