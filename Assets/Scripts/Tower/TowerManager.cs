using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [Header("References")]
    public GameObject towerPrefab;
    public MeshGenerator meshGenerator;
    public DynamicPathGenerator dynamicPathGenerator;
    public EnemySpawner enemySpawner;
    
    [Header("Map Grid Dimensions")]
    public int mapSizex = 20;
    public int mapSizez = 20;
    
    [Header("Placement Adjustments")]
    public float yOffset = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        meshGenerator.OnMeshGenerated += HandleMeshReady;
    }

    private void OnDisable()
    {
        meshGenerator.OnMeshGenerated -= HandleMeshReady;
    }

    private void HandleMeshReady()
    {
        Debug.Log("1. Mesh Ready! Spawning Center Tower...");
        SpawnCenterTower();

        if (dynamicPathGenerator != null)
        {
            Debug.Log("2. Generating Paths...");
            dynamicPathGenerator.GeneratePathways();
        }
        else
        {
            Debug.LogError("Path Generator reference is MISSING in TowerManager inspector!");
        }

        if (enemySpawner != null)
        {
            Debug.Log("3. Calling StartSpawningWave...");
            enemySpawner.StartSpawningWave();
        }
        else
        {
            Debug.LogError("Enemy Spawner reference is MISSING in TowerManager inspector!");
        }
    }

    private void SpawnCenterTower()
    {
        float centerX = meshGenerator.xSize / 2f;
        float centerZ = meshGenerator.zSize / 2f;
        float surfaceY = meshGenerator.GetTerrainHeightAt(centerX, centerZ);

        Vector3 worldPosition = meshGenerator.transform.TransformPoint(new Vector3(centerX, surfaceY, centerZ));

        if (towerPrefab.TryGetComponent<Collider>(out Collider col))
        {
            worldPosition.y += col.bounds.extents.y;
        }

        Instantiate(towerPrefab, worldPosition, Quaternion.identity);
    }
}
