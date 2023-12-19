using Assets.Scripts.Core.Data;
using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.LevelManagment;
using Assets.Scripts.Core.MapGen;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public class GameWorld : MonoBehaviour
    {
        [SerializeField] private NavMeshPlus.Components.NavMeshSurface meshSurface;
        [SerializeField] private MapGrid mapGrid;
        [SerializeField] private GameObject obstaclePF;

        [SerializeField] private Transform obstacles;
        [SerializeField] private Transform objects;
        [SerializeField] private Transform playerContainer;
        [SerializeField] private Transform enemiesContainer;
        [SerializeField] private Transform worldUIContainer;

        public Transform EnemiesContainer => enemiesContainer;
        public Transform PlayerContainer => playerContainer;
        public Transform WorldUIContainer => worldUIContainer;

        private Sprite[] MapObjects => Game.Library.SpriteLib.MapObjects;
        private MapLib MapLib => Game.Library.MapLib;

        public async Task GenerateMap(uint mapID)
        {
            var loadedData = await LoadData(mapID);
            var tileSettings = TileSettingsExtractor.Extract(loadedData.tiles, loadedData.sprite);
            var generator = new GeneratorWFC(tileSettings, 6, 16);
            var task = Task.Run(() => generator.Generate());

            try
            {
                await task;
                //mapGrid.Draw(task.Result);
                mapGrid.DrawP(task.Result).Then(() => 
                {
                    AddObstacles(task.Result);
                    AddObjects(task.Result);
                    BakeNavMesh();
                });
                
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        private async Task<(TileData[] tiles, Sprite[] sprite)> LoadData(uint mapID)
        {
            var data = MapLib.GetData(mapID);
            var sprites = AdressableLoader.LoadAssetAsyncTask<Sprite[]>(Strings.MapTile + data.spriteSheetId);
            await sprites;

            return (data.data, sprites.Result);
        }
        private void BakeNavMesh()
        {
            meshSurface.BuildNavMesh();
        }
        private void AddObstacles(IMapTile[,] map)
        {
            var size = map.GetLength(0);
            var offset = size / 2;
            
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                {
                    if (!map[x, y].Walkable)
                    {
                        var xcoord = size - x;
                        var ycoord = y;
                        var pos = new Vector3Int(ycoord - offset, xcoord - offset);
                        var obs = Instantiate(obstaclePF, obstacles);
                        obs.transform.position = new Vector3(pos.x + .5f, pos.y + .5f, pos.z);
                    }
                }
        }
        private void AddObjects(MatrixCell[,] map)
        {
            var size = map.GetLength(0);
            var offset = size / 2;

            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                {
                    var objId = map[x, y].Value.ObjectID;
                    if (objId >= 0)
                    {
                        var xcoord = size - x;
                        var ycoord = y;
                        var pos = new Vector3Int(ycoord - offset, xcoord - offset);
                        var obj = Instantiate(Game.Prefabs.MapObject, objects);
                        obj.transform.position = new Vector3(pos.x + .5f, pos.y + .5f, pos.z);

                        if (objId < MapObjects.Length)
                        {
                            obj.Set(MapObjects[objId]);
                            obj.SetScale(map[x, y].Value.ObjectScale);
                        }
                        
                    }
                }
        }
    }
}