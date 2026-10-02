using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject[] enemyPrefabs;
    public DynamicPathGenerator pathGenerator;
    public TowerHealth towerHealth;

    [Header("Wave Settings")]
    public int currentWave = 1;
    public int startingPointPool = 12;
    public float spawnInterval = 1.5f;

    [Header("Skill Adaptation Modifiers")]
    public int poolModifier = 0;      //adjusts based on player

    private int pathIndexTracker = 0;
    private bool isSpawning = false;

    // begin the wave using the point pool
    public void StartSpawningWave()
    {
        // Autogenerate paths if they havent been created yet
        if (pathGenerator.enemyPaths == null || pathGenerator.enemyPaths.Count == 0)
        {
            Debug.Log("Paths not found, generating now...");
            pathGenerator.GeneratePathways();
        }

        if (pathGenerator.enemyPaths == null || pathGenerator.enemyPaths.Count == 0)
        {
            Debug.LogWarning("No paths available to spawn enemies!");
            return;
        }

        if (!isSpawning)
        {
            StartCoroutine(SpawnWaveRoutine());
        }
    }

    private IEnumerator SpawnWaveRoutine()
    {
        isSpawning = true;

        //calculate point pool for current wave: starting pool + (wave * 10) + skill modifier
        int currentPointPool = startingPointPool + (currentWave * 10) + poolModifier;
        Debug.Log($"Starting Wave {currentWave} with Point Pool: {currentPointPool}");

        //loop until pool is 0
        while (currentPointPool > 0)
        {
            // find which enemies can still be afforded with points
            List<GameObject> affordableEnemies = GetAffordableEnemies(currentPointPool);

            if (affordableEnemies.Count == 0)
            {
                break; //dont have enough points for more enemies
            }

            //randomly pick an affordable enemy
            GameObject chosenPrefab = affordableEnemies[Random.Range(0, affordableEnemies.Count)];
            int enemyCost = GetEnemyCost(chosenPrefab);

            //spawn enemy and - the points
            currentPointPool -= enemyCost;
            SpawnSingleEnemy(chosenPrefab);

            yield return new WaitForSeconds(spawnInterval);
        }

        isSpawning = false;
        Debug.Log($"Wave {currentWave} spawn phase complete.");
    }

    private List<GameObject> GetAffordableEnemies(int remainingPoints)
    {
        List<GameObject> affordable = new List<GameObject>();
        foreach (var prefab in enemyPrefabs)
        {
            if (GetEnemyCost(prefab) <= remainingPoints)
            {
                affordable.Add(prefab);
            }
        }
        return affordable;
    }

    private int GetEnemyCost(GameObject prefab)
    {
        if (prefab.GetComponent<EnemyFly>() != null) return 1;   // fly1 points
        if (prefab.GetComponent<EnemyAnt>() != null) return 3;   // ant 3 points
        if (prefab.GetComponent<EnemyRat>() != null) return 6;   // rat 6 points
        return 3;
    }

    private void SpawnSingleEnemy(GameObject prefabToSpawn)
    {
        List<List<Vector3>> paths = pathGenerator.enemyPaths;
        
        // path selection: Spawner 1 -> Spawner 2 -> Spawner 3 -> Spawner 1
        List<Vector3> selectedPath = paths[pathIndexTracker];
        pathIndexTracker = (pathIndexTracker + 1) % paths.Count;

        if (selectedPath == null || selectedPath.Count == 0) return;

        // pace enemy at the start of the path
        Vector3 spawnPosition = selectedPath[0];
        GameObject newEnemy = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

        if (newEnemy.TryGetComponent<EnemyPathFollower>(out EnemyPathFollower follower))
        {
            follower.SetupPath(selectedPath);
        }
    }

    // evaluate player performance
    public void EvaluateWavePerformance()
    {
       
        if (towerHealth == null)
        {
            GameObject tower = GameObject.FindGameObjectWithTag("Tower");
            if (tower != null)
            {
                towerHealth = tower.GetComponent<TowerHealth>();
            }
        }

        if (towerHealth != null)
        {
            float healthPercentage = towerHealth.GetCurrentHealthPercentage(); 

            // If player finishes with high health increase next waves difficulty
            if (healthPercentage >= 0.8f)
            {
                poolModifier += 5; 
                Debug.Log("Player dominating! Increasing next wave difficulty pool.");
            }
            //If player finishes with low health lower difficulty
            else if (healthPercentage <= 0.3f)
            {
                poolModifier -= 5;
                Debug.Log("Player struggling. Granting mercy buffer for next wave.");
            }
        }

        currentWave++;
        Debug.Log("Advanced to Wave: " + currentWave);
    }
}