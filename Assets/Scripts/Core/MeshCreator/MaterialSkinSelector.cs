using UnityEngine;

namespace Assets.Scripts.Core.MeshCreator
{
    public class MaterialSkinSelector : MonoBehaviour
    {
        [SerializeField] private MeshRenderer render;
        [SerializeField] private MeshFilter meshFilter;
        private Material mat;
        private byte materialRows, materialCols;

        private void Start()
        {
            InitRandomSkin(render.material, 1);
        }
        public void InitRandomSkin(Material material, byte materialRows)
        {
            if (materialRows == 0) return;

            mat = material;
            render.material = mat;
            meshFilter.mesh = Util.GetNewQuad();
            this.materialRows = materialRows;
            materialCols = Util.GetMaterialColumns(material, materialRows);
            SetRandomSkin();
        }
        private void SetRandomSkin()
        {
            byte x = (byte)Random.Range(0, materialCols);
            byte y = (byte)Random.Range(0, materialRows);
            meshFilter.mesh.uv = Util.GetQuadMeshUVForMaterialPart(materialRows, materialCols, x, y);
        }

    }
}