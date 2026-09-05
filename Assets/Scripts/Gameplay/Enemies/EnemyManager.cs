using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Data;
using Pools.Interfaces;
using ScriptableObjects;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Gameplay.Enemies
{
    public class EnemyManager : MonoBehaviour
    {
        private const float SPAWN_AREA_WIDTH = 6.0f;
        private const float SPAWN_DEPTH = 100.0f;
        private const float SPAWN_DENSITY = 0.7f;
        private const float SPAWN_OFFSET = 40f;
        private const float DESPAWN_OFFSET = 10f;

        private IPool<Enemy> _pool;
        private Car _car;
        private GameConfigSo _gameConfig;
        private PlayerData _playerData;

        private readonly Dictionary<float, Vector3> _enemiesToSpawn = new Dictionary<float, Vector3>();
        private readonly List<Enemy> _spawnedEnemies = new List<Enemy>();
        private CancellationTokenSource _cts;

        [Inject]
        private void Construct(Car car, IPool<Enemy> pool, GameConfigSo gameConfig, PlayerData playerData)
        {
            _car = car;
            _pool = pool;
            _gameConfig = gameConfig;
            _playerData = playerData;
        }

        private void Start()
        {
            GenerateEnemiesPrepositions();
            _cts = new CancellationTokenSource();
            StartSpawning(_cts.Token).Forget();
        }

        private void GenerateEnemiesPrepositions()
        {
            int currentLevelValue = Mathf.Max(_playerData.CurrentLevel - 1, 0);
            int enemiesCount = _gameConfig.BaseEnemiesAmount + currentLevelValue * _gameConfig.ExtraEnemiesPerLevel;
            int depth = _gameConfig.BaseLevelLength + currentLevelValue * _gameConfig.ExtraLevelLengthPerLevel;
            float areaForOneEnemy = SPAWN_AREA_WIDTH * depth / enemiesCount;
            float rSquared = areaForOneEnemy * SPAWN_DENSITY;
            float r = Mathf.Sqrt(rSquared);
            float maxR = r * 2;
            int k = 30;

            List<Vector3> poissonPoints = new List<Vector3>();
            List<Vector3> finalPoints = new List<Vector3>();

            int halfWidth = Mathf.CeilToInt(SPAWN_AREA_WIDTH * .5f);
            int halfDepth = Mathf.CeilToInt(depth * .5f);

            Vector3 firstPoint = new Vector3(Random.Range(-halfWidth, halfWidth), 0, Random.Range(SPAWN_OFFSET, depth + SPAWN_OFFSET));
            poissonPoints.Add(firstPoint);
            finalPoints.Add(firstPoint);

            while(poissonPoints.Count > 0 && finalPoints.Count < enemiesCount)
            {
                int startPointIndex = Random.Range(0, poissonPoints.Count);
                Vector3 startPoint = poissonPoints[startPointIndex];

                for(int i = 0; i < k; i++)
                {
                    float angleInRadials = Random.Range(0, Mathf.PI * 2);
                    float range = Random.Range(r, maxR);
                    Vector3 nextPoint = startPoint + new Vector3(Mathf.Cos(angleInRadials) * range, 0f, Mathf.Sin(angleInRadials) * range);

                    if(nextPoint.x > halfWidth || nextPoint.x < -halfWidth || nextPoint.z > depth + SPAWN_OFFSET || nextPoint.z < SPAWN_OFFSET)
                    {
                        continue;
                    }

                    bool canAdd = true;

                    foreach(Vector3 checkPoint in finalPoints)
                    {
                        if(checkPoint == startPoint)
                        {
                            continue;
                        }

                        float distance = (nextPoint - checkPoint).sqrMagnitude;

                        if(distance <= rSquared)
                        {
                            canAdd = false;
                            break;
                        }
                    }

                    if(canAdd)
                    {
                        finalPoints.Add(nextPoint);
                        poissonPoints.Add(nextPoint);

                        if(finalPoints.Count == enemiesCount)
                        {
                            break;
                        }
                    }
                }

                poissonPoints.RemoveAt(startPointIndex);
            }

            finalPoints.Sort((a, b) => a.z.CompareTo(b.z));

            foreach(Vector3 point in finalPoints)
            {
                _enemiesToSpawn.Add(point.z, point);
            }

        }

        private async UniTaskVoid StartSpawning(CancellationToken token)
        {
            try
            {
                while(!token.IsCancellationRequested)
                {
                    SpawnEnemies();
                    DespawnEnemiesBehind();
                    await UniTask.WaitForSeconds(0.1f, cancellationToken: token);
                }
            } catch(OperationCanceledException)
            {

            }
        }

        private void SpawnEnemies()
        {
            if(_enemiesToSpawn.Count == 0)
            {
                return;
            }

            float rangeMin = _car.transform.position.z + SPAWN_OFFSET;
            float rangeMax = rangeMin + SPAWN_DEPTH;
            List<float> spawned = new List<float>();

            foreach(KeyValuePair<float, Vector3> data in _enemiesToSpawn)
            {
                if(data.Key < rangeMin || data.Key > rangeMax)
                {
                    continue;
                }

                Enemy enemy = _pool.Get();
                enemy.OnDeath += DespawnEnemy;
                enemy.transform.position = data.Value;
                _spawnedEnemies.Add(enemy);
                spawned.Add(data.Key);
            }

            foreach(float i in spawned)
            {
                _enemiesToSpawn.Remove(i);
            }
        }

        private void DespawnEnemiesBehind()
        {
            Queue<Enemy> enemiesToDespawn = new Queue<Enemy>();

            foreach(Enemy enemy in _spawnedEnemies)
            {
                if(_car.transform.position.z > enemy.transform.position.z && _car.transform.position.z - enemy.transform.position.z > DESPAWN_OFFSET)
                {
                    enemiesToDespawn.Enqueue(enemy);
                }
            }

            for(int i = 0; i < enemiesToDespawn.Count; i++)
            {
                DespawnEnemy(enemiesToDespawn.Dequeue());
            }
        }

        private void DespawnEnemy(Enemy enemy)
        {
            enemy.OnDeath -= DespawnEnemy;
            _spawnedEnemies.Remove(enemy);
            _pool.Return(enemy);
        }
    }
}