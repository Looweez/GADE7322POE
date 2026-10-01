using UnityEngine;

public class DefenderJellyTot : DefenderBase
{
    [Header("Jelly Tot Settings (Rapid Fire)")]
    public float detectionRadius = 8f;
    public float attackDamage = 5f;        // Low damage per shot
    public float attackInterval = 0.4f;    // Very fast firing rate
    
    private float attackTimer;

    private void Update()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackInterval)
        {
            Transform closestEnemy = GetClosestEnemy();
            if (closestEnemy != null)
            {
                AttackTarget(closestEnemy);
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

    private void AttackTarget(Transform target)
    {
        if (target.TryGetComponent<EnemyBase>(out EnemyBase enemy))
        {
            enemy.TakeDamage(attackDamage);
            Debug.Log("Jelly Tot fired a rapid sugary shot!");
        }
    }
}