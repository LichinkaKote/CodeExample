using Assets.Scripts.Core.Enemies;
using Assets.Scripts.Core.Input;
using Assets.Scripts.Core.LevelManagment;
using Assets.Scripts.Core.MeshCreator;
using Assets.Scripts.Core.PlayerSystems;
using Assets.Scripts.UI.Base;
using Assets.Scripts.UI.Windows;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public class GameManager
    {
        public Transform World { get; private set; }
        public Transform PlayerTranform => Player.transform;
        public Player Player { get; private set; }
        public Level CurrentLevel { get; private set; }
        public LevelInputListener LevelInputListener { get; private set; }
        public PauseManager PauseManager { get; private set; }

        private GameWorld gameWorldPF;

        private GameUI UI;
        private EnemySpawner spawner;
        private LevelUIInputListener levelUIInputListener;
        private GameWorld world;

        public GameManager(GameWorld world)
        {
            gameWorldPF = world;
        }

        public async void StartGame(uint levelId)
        {
            if (Game.Library.LevelLib.TryGetLevel(levelId, out Level level))
            {
                CurrentLevel = level;
                var loading = Game.WindowController.Open<LoadingWindow>();
                Debug.Log("StartGame");
                await Game.Library.MaterialLib.PreLoadEnemyMaterialsForLevel(CurrentLevel);
                Debug.Log("PreLoadEnemyMaterialsForLevel");

                Game.SceneLoader.LoadGameScene().Then(async () =>
                {
                    Debug.Log("LoadGameScene");
                    world = Object.Instantiate(gameWorldPF);
                    await world.GenerateMap(CurrentLevel.MapID);
                    Debug.Log("GenerateMap");
                    InitGameScene();
                    Debug.Log("InitGameScene");
                    loading.Close();
                });

            }
            else
            {
                Game.SceneLoader.LoadMainMenu();
            }
        }

        private void InitGameScene()
        {
            //temp add first wep if no wep selected
            if (Game.Player.Inventory.ActionBarItemCount == 0)
                Game.Player.Inventory.TryAddToActionBar(Game.Library.ItemLib.Weapons[0]);
            //---------

            PauseManager = new PauseManager();
            InitInput();
            InitUIInput();
            Player = Object.Instantiate(Game.Prefabs.Player, world.PlayerContainer);
            Camera.main.transform.SetParent(PlayerTranform);
            UI = Object.Instantiate(Game.Prefabs.GameUI);
            UI.Init(Player, world.WorldUIContainer);
            InitEnemySpawner(PlayerTranform);
            InitMeshSpawners();
            InitPalyerLevelUpSubscribe();
        }

        private void InitEnemySpawner(Transform player)
        {
            var spawner = new GameObject(typeof(EnemySpawner).Name, typeof(EnemySpawner));
            this.spawner = spawner.GetComponent<EnemySpawner>();

            this.spawner.Init(CurrentLevel, player, world.EnemiesContainer);
            this.spawner.AllEnemiesKilled += AllEnemiesKilled;
        }

        private void AllEnemiesKilled()
        {
            spawner.AllEnemiesKilled -= AllEnemiesKilled;
            PauseManager.Pause(true);
            var complete = Game.WindowController.Open<LevelCompleteWindow>();
            complete.OnNextClick(() => { StartGame(CurrentLevel.ID + 1); });
            complete.OnMenuClick(() => { Game.SceneLoader.LoadMainMenu(); });
        }

        private void InitMeshSpawners()
        {
            var enemies = Game.Library.EnemyLib.GetDatas(CurrentLevel.EnemyIDs);
            var enemyCounts = CurrentLevel.GetEnemyCount();

            var decalSpawners = new GameObject("DecalSpawners");
            var blood = new GameObject("Blood");
            var body = new GameObject("Body");
            var ragdollBlood = new GameObject("RagdollBlood");
            blood.transform.SetParent(decalSpawners.transform);
            body.transform.SetParent(decalSpawners.transform);
            ragdollBlood.transform.SetParent(decalSpawners.transform);

            foreach (var enemy in enemies)
            {
                var bloodDecalCreator = Util.CreateMeshRenderer<BloodDecalMeshCreator>();
                var bloodmat = Game.Library.MaterialLib.GetMaterial(enemy.BloodTexture);
                bloodDecalCreator.SetEnemyID(enemy.id);
                var bloodMatRows = (byte)bloodmat.GetFloat("_MatRows");
                bloodDecalCreator.Init(bloodmat, bloodMatRows);

                ushort count = enemyCounts[enemy.id] > ushort.MaxValue ? ushort.MaxValue : (ushort)enemyCounts[enemy.id];
                var bodyCreator = Util.CreateMeshRenderer<BodyMeshCreator>();
                var deadmat = Game.Library.MaterialLib.GetMaterial(enemy.DeadTexture);
                bodyCreator.SetEnemyID(enemy.id);
                bodyCreator.Init(deadmat, quadsCount: count);

                var decalCreator = Util.CreateMeshRenderer<RagdollDecalCreator>();
                decalCreator.Init(bloodmat, bloodMatRows);
                bodyCreator.ragdollStep += decalCreator.SpawnDecal;

                bloodDecalCreator.transform.SetParent(blood.transform);
                bodyCreator.transform.SetParent(body.transform);
                decalCreator.transform.SetParent(ragdollBlood.transform);
            }
        }
        private void InitInput()
        {
            var inst = new GameObject(typeof(LevelInputListener).Name);
            LevelInputListener = inst.AddComponent<LevelInputListener>();
        }
        private void InitUIInput()
        {
            var inst = new GameObject(typeof(LevelUIInputListener).Name);
            levelUIInputListener = inst.AddComponent<LevelUIInputListener>();
        }
        private void InitPalyerLevelUpSubscribe()
        {
            Game.Player.Experience.levelUp -= OnPalyerLevelUp;
            Game.Player.Experience.levelUp += OnPalyerLevelUp;
        }

        private void OnPalyerLevelUp(short lvl)
        {
            Game.Player.Attributes.AddLP();
        }
    }
}