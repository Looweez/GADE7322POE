using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TowerAttack : MonoBehaviour
{
    // handles towers' attacks toward enemies that get too close to it
    public float range = 10f;
    public float fireRate = 1f;
    private float fireCountdown = 0f;

    public string enemyTag = "Enemy"; //enemies will have a tag called "enemy" so the tower knows what to fire at
    public GameObject projectilePrefab; //projectiles that the tower shoots
    public Transform firePoint; //place on the tower where projecticles fire from

    private Transform target;

    void Start()
    {
        InvokeRepeating(nameof(UpdateTarget), 0f, 0.2f); //invokes repeated scanning for targets (enemies) to save performance instead of scanning every frame
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag); //only targets objects with the enemy tag
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance) //if the enemy is in range of the tower
            {
                
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    private void Update()
    {
        if (target == null) return;
        
        
        //firing logic
        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }
        
        fireCountdown -= Time.deltaTime;
    }

    void Shoot()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        TowerProjectile projectile = projectileObject.GetComponent<TowerProjectile>();

        if (projectile != null)
        {
            projectile.Seek(target);
        }
    }

    void OnDrawGizmos() // to show radius tower will attack in (ill comment tjis out later)
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
