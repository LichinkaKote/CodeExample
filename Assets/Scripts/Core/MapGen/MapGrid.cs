using Assets.Scripts.Core.Interfaces;
using RSG;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Core.MapGen
{
    public class MapGrid : MonoBehaviour
    {
        [SerializeField] private Tilemap walkable;
        [SerializeField] private Tilemap notWalkable;

        public void Draw(IMapTile[,] map)
        {
            var size = map.GetLength(0);
            var offset = size / 2;
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    var xcoord = size - x;
                    var ycoord = y;
                    var tile = ScriptableObject.CreateInstance<Tile>();
                    var cell = map[x, y];
                    var pos = new Vector3Int(ycoord - offset, xcoord - offset);

                    tile.transform = Matrix4x4.TRS(tile.transform.GetPosition(), Quaternion.Euler(0, 0, cell.Rotation), Vector3.one);
                    tile.sprite = cell?.Sprite;

                    if (cell.Walkable)
                        walkable.SetTile(pos, tile);
                    else
                        notWalkable.SetTile(pos, tile);
                    //Instantiate(Game.Prefabs.Obstacle, new Vector3(pos.x + .5f, pos.y + .5f, pos.z), Quaternion.identity);
                }
            }
        }
       
    }
}