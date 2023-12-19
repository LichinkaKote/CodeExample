using Assets.Scripts.Core.Enums;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static Assets.Scripts.Core.Util;

namespace Assets.Scripts.Core.MeshCreator
{
    public abstract class AbstractMeshCreator : MonoBehaviour
    {
        private const ushort MAX_QUADS = 15000;
        private ushort quadsCount;
        private Mesh mesh;
        private Queue<short> currentMeshes = new Queue<short>();
        private Queue<short> freeMeshes = new Queue<short>();

        private Vector3[] verts;
        private Vector2[] uvs;
        private int[] tris;

        private UVCoords[,] UVCoordsArray;

        protected virtual byte MaterialRows { get; private set; }
        protected byte MaterialCols { get; private set; }
        protected virtual byte SortingOrder => 0;
        protected ushort QuadsCount
        {
            get => quadsCount;
            set { quadsCount = value > MAX_QUADS ? MAX_QUADS : value; }
        }

        public virtual void Init(Material material, byte materialRows = 1, ushort quadsCount = 15000)
        {
            QuadsCount = quadsCount;

            verts = new Vector3[QuadsCount * 4];
            uvs = new Vector2[QuadsCount * 4];
            tris = new int[QuadsCount * 6];

            mesh = new Mesh();
            mesh.name = "custom";
            GetComponent<MeshFilter>().mesh = mesh;

            var render = GetComponent<MeshRenderer>();
            SetSorting();

            render.material = material;
            MaterialRows = materialRows;
            MaterialCols = Util.GetMaterialColumns(material, MaterialRows);
            UVCoordsArray = Util.GetUVCoordsArray(MaterialRows, MaterialCols);
            mesh.vertices = verts;
            mesh.uv = uvs;
            mesh.triangles = tris;

            var newBounds = mesh.bounds;
            newBounds.extents = Vector3.one * 1000f;
            mesh.bounds = newBounds;
        }
        private void SetSorting()
        {
            var sort = GetComponent<SortingGroup>();
            sort.sortingLayerName = SortingLayers.Ground.ToString();
            sort.sortingOrder = SortingOrder;
        }

        protected short AddQuad(Vector3 position, Vector2 size = default, float rotation = 0f, int matPartX = 0, int matPartY = 0, bool flip = false)
        {
            if (currentMeshes.Count >= QuadsCount)
                DelQuad();

            var topRightPoint = size == default ? Vector2.one * 0.5f : size * 0.5f;
            var topLeftPoint = new Vector2(-topRightPoint.x, topRightPoint.y);

            short freeIndex = GetFreeIndex();

            int vIndex = freeIndex * 4;

            float flipAngle = flip ? 180f : 0f;

            verts[vIndex] = position + Quaternion.Euler(0f, flipAngle, rotation + 180f) * topRightPoint;
            verts[vIndex + 1] = position + Quaternion.Euler(0f, flipAngle, rotation) * topLeftPoint;
            verts[vIndex + 2] = position + Quaternion.Euler(0f, flipAngle, rotation) * topRightPoint;
            verts[vIndex + 3] = position + Quaternion.Euler(0f, flipAngle, rotation + 180f) * topLeftPoint;


            uvs[vIndex] = UVCoordsArray[matPartX, matPartY].uv00;
            uvs[vIndex + 1] = UVCoordsArray[matPartX, matPartY].uv01;
            uvs[vIndex + 2] = UVCoordsArray[matPartX, matPartY].uv11;
            uvs[vIndex + 3] = UVCoordsArray[matPartX, matPartY].uv10;

            int tIndex = freeIndex * 6;
            tris[tIndex] = vIndex;
            tris[tIndex + 1] = vIndex + 1;
            tris[tIndex + 2] = vIndex + 2;
            tris[tIndex + 3] = vIndex;
            tris[tIndex + 4] = vIndex + 2;
            tris[tIndex + 5] = vIndex + 3;

            mesh.vertices = verts;
            mesh.uv = uvs;
            mesh.triangles = tris;

            currentMeshes.Enqueue(freeIndex);
            return freeIndex;
        }
        protected void DelQuad()
        {
            short index = currentMeshes.Dequeue();
            freeMeshes.Enqueue(index);

            int vIndex = index * 4;
            verts[vIndex] = default;
            verts[vIndex + 1] = default;
            verts[vIndex + 2] = default;
            verts[vIndex + 3] = default;

            uvs[vIndex] = default;
            uvs[vIndex + 1] = default;
            uvs[vIndex + 2] = default;
            uvs[vIndex + 3] = default;

            int tIndex = index * 6;
            tris[tIndex] = default;
            tris[tIndex + 1] = default;
            tris[tIndex + 2] = default;
            tris[tIndex + 3] = default;
            tris[tIndex + 4] = default;
            tris[tIndex + 5] = default;

            mesh.vertices = verts;
            mesh.uv = uvs;
            mesh.triangles = tris;
        }
        private short GetFreeIndex()
        {
            if (freeMeshes.Count == 0)
                return (short)currentMeshes.Count;
            else
                return freeMeshes.Dequeue();
        }
        protected void MoveQuad(short index, Vector3 delta)
        {
            int vIndex = index * 4;
            verts[vIndex] = verts[vIndex] + delta;
            verts[vIndex + 1] = verts[vIndex + 1] + delta;
            verts[vIndex + 2] = verts[vIndex + 2] + delta;
            verts[vIndex + 3] = verts[vIndex + 3] + delta;
        }
        protected void ApplyQuadsMovement()
        {
            mesh.vertices = verts;
        }
    }
}