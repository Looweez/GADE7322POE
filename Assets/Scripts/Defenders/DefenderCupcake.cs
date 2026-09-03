using Unity.VisualScripting;
using UnityEngine;

public class DefenderCupcake : DefenderBase
{
    [Header("Targeting Settings")] 
    public float detectionRadius = 10f;
    public float attackDamage = 15f;
    public float attackInterval = 1.5f;
    
    private Transform towerTransform;
    private float attackTimer;
    
    private void Start()
    {
        Initialize(); // Must be called so towerTransform gets assigned!
    }
    
    public override void Initialize()
    {
        base.Initialize();
        
        //for finding the tower to tafret the closest enemy to it
        GameObject towerObj = GameObject.FindGameObjectWithTag("Tower");
        if (towerObj != null)
        {
            towerTransform = towerObj.transform;
        }
        
    }

    private void Update()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackInterval)
        {
            Transform closestEnemy = GetClosestEnemyToTower();
            if (closestEnemy != null)
            {
                AttackTarget(closestEnemy);
                attackTimer = 0f;
            }
        }
    }

    private Transform GetClosestEnemyToTower()
    {
        if (towerTransform == null)
        {
            return null;
        }
        //finds every collider in the defender's range
        Collider[] hitColliders = Physics.OverlapSphere(towerTransform.position, detectionRadius);

        Transform closestEnemy = null;
        float shortestDistanceToTower = Mathf.Infinity;

        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("Enemy"))
            {
                float distanceToTower = Vector3.Distance(hit.transform.position, towerTransform.position);

                if (distanceToTower < shortestDistanceToTower)
                {
                    shortestDistanceToTower = distanceToTower;
                    closestEnemy = hit.transform;
                }
            }
        }
        
        return closestEnemy;
    }

    private void AttackTarget(Transform target)
    {
        if (target.TryGetComponent<EnemyBase>(out EnemyBase enemyHealth))
        {
            enemyHealth.TakeDamage(attackDamage);
            Debug.Log("Attacked enemy");
        }
    }
}
