using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("--- Misc Settings ---")]
    [SerializeField] public GameObject[] enemyPrefabs;
    [SerializeField] public Transform[] spawnPoints;
    [SerializeField] public WaypointController waypointController;
    [SerializeField] public Transform playerTransform;

    [SerializeField][HideInInspector] private List<GameObject> activeEnemies = new List<GameObject>();

    [Header("--- Wave Settings ---")]
    [SerializeField] public int enemiesPerWave = 0;
    [SerializeField] public float timeBetweenSpawns = 0F;
    [SerializeField] public float timeBetweenWaves = 0F;
    [SerializeField] public int totalWaves = 10;
    [SerializeField] public bool isWaveActive = true;
    [SerializeField] public int currentWave = 0;

    [SerializeField][HideInInspector] private float waveTimer = 0F;
    [SerializeField][HideInInspector] private int enemiesSpawned = 0;

    void Update()
    {
        if (isWaveActive)
        {
            waveTimer += Time.deltaTime;

            if (waveTimer >= timeBetweenSpawns && enemiesSpawned < enemiesPerWave)
            {
                SpawnEnemy();
                waveTimer = 0F;
                enemiesSpawned++;
            }

            if (enemiesSpawned >= enemiesPerWave && AllEnemiesDefeated())
            {
                isWaveActive = false;
                enemiesSpawned = 0;
                currentWave++;

                if (currentWave < totalWaves)
                    Invoke("StartNextWave", timeBetweenWaves);
                else
                    Debug.Log("Complete!");
            }
        }
    }

    void StartNextWave()
    {
        isWaveActive = true;
        waveTimer = 0F;
        enemiesPerWave += Mathf.FloorToInt(currentWave * 3F);
        Debug.Log("Wave: " + (currentWave + 1));
    }

    void SpawnEnemy()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        int enemyIndex = Random.Range(0, enemyPrefabs.Length);

        List<Transform> waypoints = waypointController.GetWaypoints();
            GameObject enemy = Instantiate(enemyPrefabs[enemyIndex], spawnPoints[randomIndex].position, Quaternion.identity);
            activeEnemies.Add(enemy);

            EnemyAI ai = enemy.GetComponent<EnemyAI>();

        if (ai != null)
        {
            if (Random.value < 0.5F && waypoints.Count > 0)
            {
                ai.SetTarget(waypoints[Random.Range(0, waypoints.Count)]);
            }
            else
                ai.SetTarget(playerTransform);
        }
    }

    bool AllEnemiesDefeated()
    {
        activeEnemies.RemoveAll(enemy => enemy == null);
        return activeEnemies.Count == 0;
    }
}