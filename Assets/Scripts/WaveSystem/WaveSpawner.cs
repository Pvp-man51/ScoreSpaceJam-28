using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WAITING, CONTINUING }

    [Header("SpawnStuff")]
    [SerializeField] private Vector2 SpawnAreaSize;
    [SerializeField] private GameObject Enemy;
    [Space(5)]
    [SerializeField] private float TimeBetweenWaves = 5.0f;

    private float timeBetweenWavesTimer;

    private float searchTime = 1.0f;
    private float searchTimer;

    private int waveCount;
    private int enemyCount = 1;
    private float spawnRate = 1.0f;

    private SpawnState spawnState = SpawnState.CONTINUING;

    private void Start()
    {
        timeBetweenWavesTimer = TimeBetweenWaves;
    }

    private void Update()
    {   
        if (spawnState == SpawnState.WAITING)
        {
            if (!EnemyAlive())
            {
                // Move to next wave
                WaveCompleted();
            }
            else
            {
                return;
            }
        }

        if (timeBetweenWavesTimer <= 0)
        {
            if (spawnState != SpawnState.SPAWNING)
                StartCoroutine(SpawnWaves());
        }
        else
            timeBetweenWavesTimer -= Time.deltaTime;
    }

    IEnumerator SpawnWaves()
    {
        print("Spawning Wave: " + waveCount);

        spawnState = SpawnState.SPAWNING;

        while (spawnState == SpawnState.SPAWNING)
        {
            // Calc Pos
            Vector2 spawnPos = new Vector2(Random.Range(-SpawnAreaSize.x, SpawnAreaSize.x), Random.Range(-SpawnAreaSize.y, SpawnAreaSize.y));
            
            // Spawn the enemies
            for (int i = 0; i < enemyCount; i++) 
            {
                SpawnEnemy(spawnPos);
                yield return new WaitForSeconds(spawnRate);
            }

            // Change state to WAITING after all enemies are spawned
            spawnState = SpawnState.WAITING;

            yield break;
        }
    }

    private void SpawnEnemy(Vector2 pos)
    {
        Instantiate(Enemy, pos, Quaternion.identity);
    }

    private bool EnemyAlive()
    {
        searchTimer -= Time.deltaTime;

        if (searchTimer < 0)
        {
            searchTimer = searchTime;

            if (GameObject.FindGameObjectWithTag("Enemy") == null)
                return false;
        }

        return true;
    }

    public void EndEnemyWaves()
    {
        spawnState = SpawnState.CONTINUING;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            GameObject.Destroy(enemy);
        }
    }

    private void WaveCompleted()
    {
        print("Wave Completed");

        // Move to the CONTINUING state, reset timer
        spawnState = SpawnState.CONTINUING;
        timeBetweenWavesTimer = TimeBetweenWaves;

        // Update wave parameters
        waveCount++;
        spawnRate -= 0.1f;
        enemyCount++;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.1f);
        Gizmos.DrawCube(transform.position, SpawnAreaSize);
    }
}
