using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject enemyPrefab;
    public DynamicPathGenerator pathGenerator;

    [Header("Spawn Settings")]
    public float spawnInterval = 2f;
    public int totalEnemiesToSpawn = 15;

    private int spawnedCount = 0;
    private bool isSpawning = false;

    // Call this to begin the wave (or connect it to a UI Start Wave button)
    public void StartSpawningWave()
    {
        if (pathGenerator.enemyPaths.Count == 0)
        {
            Debug.LogWarning("No paths available yet!");
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
        
        // Pick one of the 3 paths at random
        int randomPathIndex = Random.Range(0, paths.Count);
        List<Vector3> selectedPath = paths[randomPathIndex];

        // Ensure the selected path has points
        if (selectedPath == null || selectedPath.Count == 0) return;

        // Instantiate enemy at the start of the path (index 0)
        Vector3 spawnPosition = selectedPath[0];
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // Assign the path to the enemy follower script
        if (newEnemy.TryGetComponent<EnemyPathFollower>(out EnemyPathFollower follower))
        {
            follower.SetupPath(selectedPath);
        }
    }
}