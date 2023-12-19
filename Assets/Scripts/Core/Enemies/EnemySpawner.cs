using Assets.Scripts.Core.Data;
using Assets.Scripts.Core.LevelManagment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.Core.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        public event Action AllEnemiesKilled;

        private List<Enemy> enemyList = new List<Enemy>();
        private Stack<Enemy> enemyListDead = new Stack<Enemy>();
        private float minSpawnDist = 12f;
        private float maxSpawnDist = 20f;
        private Transform player, world;
        private Level level;
        private List<SpawnReserve> spawnReserve = new List<SpawnReserve>();

        private IDisposable timerDis;
        private class SpawnReserve
        {
            public EnemyData data;
            public uint count;
        }
        private void Awake()
        {
            Game.Events.actorDeath += OnKill;
        }
        public void Init(Level level, Transform playerTr, Transform worldTr)
        {
            this.level = level;
            InitSpawnReserve();
            player = playerTr;
            world = worldTr;
            timerDis = Observable.Interval(TimeSpan.FromSeconds(1f), Scheduler.MainThreadEndOfFrame).Subscribe(x => Timer()).AddTo(this);
        }
        private void InitSpawnReserve()
        {
            foreach (var data in level.EnemyData)
            {
                spawnReserve.Add(new SpawnReserve { data = Game.Library.EnemyLib.GetData(data.id), count = data.count });
            }
        }
        private void Timer()
        {
            Spawn();
            MoveEnemiesFromBehind();
        }
        private void Spawn()
        {
            var count = level.SpawnPool - enemyList.Count;
            for (uint i = 0; i < count; i++)
            {
                if (TryGetEnemy(out EnemyData data))
                {
                    SpawnEnemy(data);
                }
                else
                {
                    if (enemyList.Count <= 0 && spawnReserve.Count == 0)
                    {
                        AllEnemiesKilled?.Invoke();
                        timerDis.Dispose();
                        Debug.Log("end level");
                        return;
                    }
                }
            }
        }
        private bool TryGetEnemy(out EnemyData data)
        {
            if (spawnReserve.Count > 0)
            {
                var current = spawnReserve.First();
                data = current.data;
                current.count--;
                if (current.count == 0)
                    spawnReserve.RemoveAt(0);
                return true;
            }
            data = null;
            return false;
        }
        private void SpawnEnemy(EnemyData data)
        {
            Enemy enemy;
            if (enemyListDead.Count > 0)
            {
                enemy = enemyListDead.Pop();
                enemy.transform.position = GetRandomPos();
                enemy.SetVisble(true);
            }
            else
            {
                enemy = Instantiate(Game.Prefabs.Enemy, world);
                enemy.transform.position = GetRandomPos();
            }
            enemy.Init(data);
            enemyList.Add(enemy);
        }
        private Vector2 GetRandomPos()
        {
            var dist = UnityEngine.Random.Range(minSpawnDist, maxSpawnDist);
            Vector3 point;
            if (Game.GameManager.LevelInputListener.AxisInput == Vector3.zero)
            {
                var vec = Vector2.right * dist;
                point = vec.GetInacuracyVector(360);
            }
            else
            {
                Vector2 dir = Game.GameManager.LevelInputListener.AxisInput.normalized * dist;
                point = dir.GetInacuracyVector(90);
            }
            return point + player.position;
        }

        private void MoveEnemiesFromBehind()
        {
            foreach (Enemy enemy in enemyList)
            {
                if (enemy.DistanceToPlayer > maxSpawnDist + 1f)
                {
                    enemy.Teleport(GetRandomPos());
                }
            }
        }
        private void OnKill(Enemy enemy)
        {
            enemyList.Remove(enemy);
            enemyListDead.Push(enemy);
            enemy.SetVisble(false);
        }
        private void OnDestroy()
        {
            Game.Events.actorDeath -= OnKill;
        }
    }
}