using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WAITING, CONTINUING }

    [Header("SpawnStuff")]
    [SerializeField] private Vector2 SpawnAreaSize;
    [SerializeField] private GameObject[] Enemy;
    [Space(5)]
    [SerializeField] private float TimeBetweenWaves = 5.0f;
    [Space(5)]
    [SerializeField] private float TimeTillMutationOccurs = 30f;

    private float mutationTimer;

    private float timeBetweenWavesTimer;

    private float searchTime = 1.0f;
    private float searchTimer;

    private int waveCount;
    private int enemyCount = 3;
    private float spawnRate = 1.0f;

    private SpawnState spawnState = SpawnState.CONTINUING;

    private void Start()
    {
        timeBetweenWavesTimer = TimeBetweenWaves;
        mutationTimer = TimeTillMutationOccurs;
    }

    private void Update()
    {
        if (GameManager.Instance.State == GameState.Death)
            return;

        if (spawnState == SpawnState.WAITING)
        {
            WaveCompleted();
        }

        //if (EnemyAlive()) 
        //{
            mutationTimer -= Time.deltaTime;

            if (mutationTimer < 0)
            {
                AudioManager.Instance.Play("EnemyTalking");
                GameManager.Instance.StartMutation();
                mutationTimer = TimeTillMutationOccurs;
            }
        //}

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
            // Spawn the enemies
            for (int i = 0; i < enemyCount; i++) 
            {
                // Calc Pos
                Vector2 spawnPos = new Vector2(Random.Range(-SpawnAreaSize.x / 2, SpawnAreaSize.x / 2), Random.Range(-SpawnAreaSize.y / 2, SpawnAreaSize.y / 2));

                SpawnEnemy((Vector2)transform.position + spawnPos);

                yield return new WaitForSeconds(spawnRate);
            }

            // Change state to WAITING after all enemies are spawned
            spawnState = SpawnState.WAITING;

            yield break;
        }
    }

    private void SpawnEnemy(Vector2 pos)
    {
        Instantiate(Enemy[Random.Range(0, Enemy.Length)], pos, Quaternion.identity);
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

        // Move to the CONTINUING state, reset timers
        spawnState = SpawnState.CONTINUING;
        timeBetweenWavesTimer = TimeBetweenWaves;
        mutationTimer = TimeTillMutationOccurs;

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
