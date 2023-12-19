using Assets.Scripts.Core.Enums;
using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.MeshCreator;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Scripts.Core
{
    public static class Util
    {
        public static T CreateMeshRenderer<T>() where T : AbstractMeshCreator
        {
            GameObject go = new GameObject(typeof(T).Name, typeof(MeshFilter), typeof(MeshRenderer), typeof(SortingGroup));
            return go.AddComponent<T>();
        }
        public static void RemoveAllChilds(this Transform tr)
        {
            for (int i = 0; i < tr.childCount; i++)
            {
                UnityEngine.Object.Destroy(tr.GetChild(i).gameObject);
            }
        }
        public static Vector2 GetInacuracyVector(this Vector2 vector, float inacuracy)
        {
            var randomAngle = Random.Range(-inacuracy / 2f, inacuracy / 2f);
            var angle = Quaternion.AngleAxis(randomAngle, Vector3.forward) * vector;
            return angle;
        }
        public static string ToDesc(this WeaponType type)
        {
            switch (type)
            {
                case WeaponType.Everything:
                    return "All";
                case WeaponType.AssaultRifle:
                    return "Assault Rifle";
                case WeaponType.SniperRifle:
                    return "Sniper Rifle";
                case WeaponType.MachineGun:
                    return "Machine Gun";

                default:
                    return type.ToString();
            }
        }
        public static IDamage Multiply(this IDamage damage, float mult)
        {
            var result = new Damage();
            result.PhysicalDamage = damage.PhysicalDamage * mult;
            result.PoisonDamage = damage.PoisonDamage * mult;
            result.FireDamage = damage.FireDamage * mult;
            result.FrostDamage = damage.FrostDamage * mult;
            result.LightningDamage = damage.LightningDamage * mult;
            result.PureDamage = damage.PureDamage * mult;
            return result;
        }


        #region Mesh
        public struct UVCoords
        {
            public Vector2 uv01 => new Vector2(uv00.x, uv11.y);
            public Vector2 uv10 => new Vector2(uv11.x, uv00.y);
            public Vector2 uv00 { get; set; }
            public Vector2 uv11 { get; set; }
        }
        public static Mesh GetNewQuad()
        {
            Mesh mesh = new Mesh();
            Vector3[] verts = new Vector3[4];
            Vector2[] uvs = new Vector2[4];
            int[] tris = new int[6];

            verts[0] = new Vector2(-0.5f, -0.5f);
            verts[1] = new Vector2(-0.5f, 0.5f);
            verts[2] = new Vector2(0.5f, 0.5f);
            verts[3] = new Vector2(0.5f, -0.5f);


            uvs[0] = new Vector2(0, 0);
            uvs[1] = new Vector2(0, 1);
            uvs[2] = new Vector2(1, 1);
            uvs[3] = new Vector2(1, 0);

            tris[0] = 0;
            tris[1] = 1;
            tris[2] = 2;
            tris[3] = 0;
            tris[4] = 2;
            tris[5] = 3;

            mesh.vertices = verts;
            mesh.uv = uvs;
            mesh.triangles = tris;

            return mesh;
        }
        public static byte GetMaterialColumns(Material material, byte materialRows)
        {
            if (material.mainTexture == null) return 1;
            var width = material.mainTexture.width;
            var height = material.mainTexture.height;
            var rowHeight = height / materialRows;
            return (byte)(width / rowHeight);
        }
        public static UVCoords[,] GetUVCoordsArray(byte materialRows, byte materialCols)
        {
            var uvXCellSize = 1f / materialCols;
            var uvYCellSize = 1f / materialRows;
            var UVCoordsArray = new UVCoords[materialCols, materialRows];
            for (int i = 0; i < materialCols; i++)
            {
                for (int j = 0; j < materialRows; j++)
                {
                    var x00 = i * uvXCellSize;
                    var y00 = (materialRows - (j + 1)) * uvYCellSize;
                    UVCoordsArray[i, j].uv00 = new Vector2(x00, y00);

                    var x11 = (i + 1) * uvXCellSize;
                    var y11 = (materialRows - j) * uvYCellSize;
                    UVCoordsArray[i, j].uv11 = new Vector2(x11, y11);
                }
            }
            return UVCoordsArray;
        }
        public static Vector2[] GetQuadMeshUVForMaterialPart(byte materialRows, byte materialCols, byte matPartX, byte matPartY)
        {
            var result = new Vector2[4];
            var UVCoordsArray = GetUVCoordsArray(materialRows, materialCols);
            result[0] = UVCoordsArray[matPartX, matPartY].uv00;
            result[1] = UVCoordsArray[matPartX, matPartY].uv01;
            result[2] = UVCoordsArray[matPartX, matPartY].uv11;
            result[3] = UVCoordsArray[matPartX, matPartY].uv10;
            return result;
        }
        #endregion
    }
}