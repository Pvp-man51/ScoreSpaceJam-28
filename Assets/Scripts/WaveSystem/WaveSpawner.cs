using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    // Continuig this Tomorrow :)
    public enum SpawnState { SPAWNING, WAITING, CONTINUING }

    [Header("SpawnArea")]
    [SerializeField] private Vector2 SpawnAreaSize;

    //IEnumerator SpawnWaves(SpawnState spawnState)
    //{
    //   while (spawnState == SpawnState.SPAWNING)
    //   {
    //        
    //   }
    //}

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.1f);
        Gizmos.DrawCube(transform.position, SpawnAreaSize);
    }
}
