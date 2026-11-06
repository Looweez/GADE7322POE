using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    [Header("Settings")]
    public GameObject boidPrefab;
    public int numberOfBoids = 100;
    public float spawnRadius = 12f;
    public float orbitRadius = 12f;

    [Header("Color Gradient")]
    public Color baseColor = Color.cyan;
    public Color heightColor = Color.magenta;

    [Header("Flocking Parameters")]
    [Range(0.1f, 10f)] public float perception = 3f;
    [Range(0f, 10f)] public float cohesionWeight = 1f;
    [Range(0f, 10f)] public float separationWeight = 1.5f;
    [Range(0f, 10f)] public float alignmentWeight = 1f;
    
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float maxSteeringForce = 3f;

    [HideInInspector] public Transform towerTarget;
    private List<Boid> boids = new List<Boid>();
    private bool isInitialized = false;

    void Start()
    {
        StartCoroutine(WaitForTowerAndSpawn());
    }

    IEnumerator WaitForTowerAndSpawn()
    {
        Debug.Log("BoidManager: Waiting for runtime tower to spawn...");
        GameObject towerObj = null;

        while (towerObj == null)
        {
            towerObj = GameObject.FindWithTag("Tower");
            yield return null;
        }

        towerTarget = towerObj.transform;
        Debug.Log($"BoidManager: Tower found at {towerTarget.position}. Spawning boids...");

        SpawnBoids();
        isInitialized = true;
    }

    void Update()
    {
        if (!isInitialized) return;

        for (int i = 0; i < boids.Count; i++)
        {
            if (boids[i] != null)
            {
                boids[i].UpdateBoid(boids);
            }
        }
    }

    void SpawnBoids()
    {
        Vector3 center = towerTarget.position;

        for (int i = 0; i < numberOfBoids; i++)
        {
            //spawns in an outer ring of the tower
            Vector3 randomDir = Random.onUnitSphere;
            randomDir.y = 0;
            randomDir.Normalize();

            float distance = Random.Range(6f, spawnRadius);
            Vector3 spawnPos = center + (randomDir * distance) + Vector3.up * Random.Range(4f, 10f);

            GameObject obj = Instantiate(boidPrefab, spawnPos, Quaternion.identity);
            Boid boid = obj.GetComponent<Boid>();
            if (boid != null)
            {
                boid.Initialize(this, spawnPos);
                boids.Add(boid);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (towerTarget != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(towerTarget.position, orbitRadius);
        }
    }
}