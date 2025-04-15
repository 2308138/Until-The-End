using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    [Header("--- Misc Settings ---")]
    [SerializeField] public GameObject[] enemyPrefabs;
    [SerializeField] public Transform[] spawnPoints;
    [SerializeField] public WaypointController waypointController;
    [SerializeField] public Transform playerTransform;

    [SerializeField][HideInInspector] private List<GameObject> activeEnemies = new List<GameObject>();
    [SerializeField][HideInInspector] private UIManager uiManager;

    [Header("--- Wave Settings ---")]
    [SerializeField] public int enemiesPerWave = 0;
    [SerializeField] public float timeBetweenSpawns = 0F;
    [SerializeField] public float timeBetweenWaves = 0F;
    [SerializeField] public int totalWaves = 10;
    [SerializeField] public int currentWave = 0;

    [SerializeField][HideInInspector] public bool isWaveActive = true;
    [SerializeField][HideInInspector] private float waveTimer = 0F;
    [SerializeField][HideInInspector] private int enemiesSpawned = 0;

    [Header("--- Tutorial Settings ---")]
    [SerializeField] public Image moveImage;
    [SerializeField] public Image dashImage;
    [SerializeField] public Image attackImage;
    [SerializeField] public GameObject tutorialPanel;

    [SerializeField][HideInInspector] private bool moved = false;
    [SerializeField][HideInInspector] private bool attacked = false;
    [SerializeField][HideInInspector] private bool dashed = false;
    [SerializeField][HideInInspector] private bool tutorialComplete = false;
    [UnityEngine.Range(0F, 1F)][HideInInspector] private float inactiveAlpha = 0.5F;
    [UnityEngine.Range(0F, 1F)][HideInInspector] private float activeAlpha = 1F;

    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();

        if (currentWave == 0)
        {
            UpdateTutorialVisuals();
        }
        else
            StartNextWave();
    }

    void Update()
    {
        if (!tutorialComplete && currentWave == 0)
        {
            HandleTutorial();
            return;
        }

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

                if (currentWave < totalWaves)
                    Invoke("StartNextWave", timeBetweenWaves);
                else
                    Debug.Log("Complete!");
            }
        }
    }

    void HandleTutorial()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            moved = true;

        if (Input.GetKeyDown(KeyCode.Z))
            attacked = true;

        if (Input.GetKeyDown(KeyCode.Space))
            dashed = true;

        UpdateTutorialVisuals();

        if (moved && attacked && dashed)
            CompleteTutorial();
    }

    void UpdateTutorialVisuals()
    {
        SetAlpha(moveImage, moved ? activeAlpha : inactiveAlpha);
        SetAlpha(dashImage, dashed ? activeAlpha : inactiveAlpha);
        SetAlpha(attackImage, attacked ? activeAlpha : inactiveAlpha);
    }

    void SetAlpha(Image img, float alpha)
    {
        Color c = img.color;
        c.a = alpha;
        img.color = c;
    }

    void CompleteTutorial()
    {
        tutorialComplete = true;
        tutorialPanel.SetActive(false);
        StartNextWave();
    }

    void StartNextWave()
    {
        currentWave++;
        isWaveActive = true;
        waveTimer = 0F;
        enemiesPerWave += Mathf.FloorToInt(currentWave * 3F);
        uiManager.UpdateWaveCounter();
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