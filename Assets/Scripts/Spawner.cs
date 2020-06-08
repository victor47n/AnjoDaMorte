using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Wave[] waves;
    public List<EnemyController> enemy = new List<EnemyController>();
    private int noiseValues;

    public LayerMask EnemyLayer;
    [Header("Distance spawn")]
    public float distanceGeneration = 9;
    private float distancePlayerForGeneration = 20;
    private GameObject player;

    Wave currentWave;
    int currentWaveNumber;

    int enemiesRemainingToSpawn;
    int enemiesRemainingAlive;
    float nextSpawnTime;

    EnemyCounter enemyCounter;

    void Start()
    {
        enemyCounter = GameObject.FindWithTag("EnemyCounter").GetComponent(typeof(EnemyCounter)) as EnemyCounter;
        NextWave();
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {

        if (player != null)
        {
            if (Vector3.Distance(transform.position, player.transform.position) > distancePlayerForGeneration)
            {
                if (enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime)
                {
                    StartCoroutine(GenerateNewEnemy());
                }
            }
        }
    }

    int RandomEnemy()
    {
        return UnityEngine.Random.Range(0, 3);
    }

    Vector3 RandomPositionEnemy()
    {
        Vector3 position = UnityEngine.Random.insideUnitSphere * distanceGeneration;
        position += transform.position;
        position.y = 0;

        return position;
    }

    IEnumerator GenerateNewEnemy()
    {
        noiseValues = RandomEnemy();

        Vector3 positionGenerate = RandomPositionEnemy();
        Collider[] colisions = Physics.OverlapSphere(positionGenerate, 1, EnemyLayer);

        while (colisions.Length > 0)
        {
            positionGenerate = RandomPositionEnemy();
            colisions = Physics.OverlapSphere(positionGenerate, 1, EnemyLayer);
            yield return null;
        }

        enemiesRemainingToSpawn--;
        nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

        EnemyController spawnedEnemy = Instantiate(enemy[noiseValues], positionGenerate, Quaternion.identity) as EnemyController;
        spawnedEnemy.transform.parent = GameObject.Find("Scene").transform;
        spawnedEnemy.OnDeath += OnEnemyDeath;
    }

    void OnEnemyDeath()
    {
        enemiesRemainingAlive--;
        enemyCounter.DecreaseEnemyLives();
        if (enemiesRemainingAlive == 0)
        {
            NextWave();
        }
    }

    void NextWave()
    {
        currentWaveNumber++;
        if (currentWaveNumber - 1 < waves.Length)
        {
            currentWave = waves[currentWaveNumber - 1];

            enemiesRemainingToSpawn = currentWave.enemyCount;
            enemiesRemainingAlive = enemiesRemainingToSpawn;
            enemyCounter.AddToAliveEnemies(enemiesRemainingAlive);
        }
    }

    [Serializable]
    public class Wave
    {
        public int enemyCount;
        public float timeBetweenSpawns;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanceGeneration);
    }
}
