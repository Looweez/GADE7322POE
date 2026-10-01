using UnityEngine;

public class DefenderSnowball : DefenderBase
{
    [Header("Snowball Settings (Heavy AoE Splash)")]
    public float detectionRadius = 10f;
    public float attackDamage = 35f;       // Heavy damage
    public float attackInterval = 2.5f;    // Slow firing rate
    public float splashRadius = 3.5f;      // Area of effect explosion radius
    
    private float attackTimer;

    private void Update()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackInterval)
        {
            Transform target = GetClosestEnemy();
            if (target != null)
            {
                ExplodeAtTarget(target.position);
                attackTimer = 0f;
            }
        }
    }

    private Transform GetClosestEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        Transform closest = null;
        float minDis = Mathf.Infinity;

        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("Enemy"))
            {
                float dis = Vector3.Distance(transform.position, hit.transform.position);
                if (dis < minDis)
                {
                    minDis = dis;
                    closest = hit.transform;
                }
            }
        }
        return closest;
    }

    private void ExplodeAtTarget(Vector3 explodePosition)
    {
        // Hit all enemies within the splash radius
        Collider[] hitColliders = Physics.OverlapSphere(explodePosition, splashRadius);
        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("Enemy"))
            {
                if (hit.TryGetComponent<EnemyBase>(out EnemyBase enemy))
                {
                    enemy.TakeDamage(attackDamage);
                }
            }
        }
        Debug.Log("Snowball dealt coconut-y splash damage to nearby enemies!");
    }
}
