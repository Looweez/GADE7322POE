using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public DynamicPathGenerator pathGenerator;
    
    public float spawnInterval = 2f;
    public int totalEnemiesToSpawn = 15;

    private int spawnedCount = 0;
    private bool isSpawning = false;

    // begin the wave
    public void StartSpawningWave()
    {
        // autogenerate paths if they havent been created yet
        if (pathGenerator.enemyPaths == null || pathGenerator.enemyPaths.Count == 0)
        {
            Debug.Log("Paths not found, generating now...");
            pathGenerator.GeneratePathways();
        }

        if (pathGenerator.enemyPaths.Count == 0)
        {
            Debug.LogWarning("no paths available");
            return;
        }

        if (!isSpawning)
        {
            StartCoroutine(SpawnRoutine());
        }
    }

    private IEnumerator SpawnRoutine()
    {
        isSpawning = true;

        while (spawnedCount < totalEnemiesToSpawn)
        {
            SpawnSingleEnemy();
            spawnedCount++;
            yield return new WaitForSeconds(spawnInterval);
        }

        isSpawning = false;
    }

    private void SpawnSingleEnemy()
    {
        List<List<Vector3>> paths = pathGenerator.enemyPaths;
        
        // pick path
        int randomPathIndex = Random.Range(0, paths.Count);
        List<Vector3> selectedPath = paths[randomPathIndex];

      
        if (selectedPath == null || selectedPath.Count == 0) return;

        // place enemy at start
        Vector3 spawnPosition = selectedPath[0];
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        if (newEnemy.TryGetComponent<EnemyPathFollower>(out EnemyPathFollower follower))
        {
            follower.SetupPath(selectedPath);
        }
    }
}