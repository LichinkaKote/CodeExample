using UnityEngine;

namespace Assets.Scripts.Core.Enemies
{
    public class EnemyBodyDecals : MonoBehaviour
    {
        [SerializeField] private Renderer render;
        private Enemy self;

        [Header("Decals positions")]
        [SerializeField] private float xMin = 0.1f;
        [SerializeField] private float xMax = 0.9f;
        [SerializeField] private float yMin = 0.1f;
        [SerializeField] private float yMax = 0.7f;

        private static readonly int decalPos1 = Shader.PropertyToID("_Pos1");
        private static readonly int decalPos2 = Shader.PropertyToID("_Pos2");
        private static readonly int decalPos3 = Shader.PropertyToID("_Pos3");

        private MaterialPropertyBlock materialProperty;
        private byte hitCount;
        private int[] properties = new int[3];

        private void Awake()
        {
            self = GetComponent<Enemy>();
            properties[0] = decalPos1;
            properties[1] = decalPos2;
            properties[2] = decalPos3;
        }
        private void Start()
        {
            self.hitInfo += OnHit;
            self.death += ResetDecals;
        }
        private void OnHit(DamageInfo info)
        {
            if (materialProperty == null) materialProperty = new MaterialPropertyBlock();
            SetProperty();
            IncreseHitCount();
        }
        private void SetProperty()
        {
            if (hitCount >= properties.Length) return;

            materialProperty.SetVector(properties[hitCount], GetRandomPosition());
            render.SetPropertyBlock(materialProperty);
        }
        private void IncreseHitCount()
        {
            if (hitCount < properties.Length)
                hitCount++;
        }
        private Vector4 GetRandomPosition()
        {
            return new Vector4(Random.Range(-xMax, -xMin), Random.Range(-yMax, -yMin));
        }
        private void ResetDecals()
        {
            materialProperty = new MaterialPropertyBlock();
            render.SetPropertyBlock(materialProperty);
            hitCount = 0;
        }
        private void OnDestroy()
        {
            self.hitInfo -= OnHit;
            self.death -= ResetDecals;
        }
    }
}