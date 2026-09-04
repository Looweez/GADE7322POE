using System.Collections.Generic;
using UnityEngine;

public class EnemyPathFollower : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 3f;
    private List<Vector3> pathWaypoints;
    private int currentWaypointIndex = 0;
    
    [Header("Combat")]
    public int damageToTower = 10;
    public int attackDamage = 10;
    public float attackRange = 1.2f;
    public float attackInterval = 1.5f;
    private float attackTimer;

    private DefenderBase currentDefenderTarget;

    public void SetupPath(List<Vector3> newPath)
    {
        pathWaypoints = newPath;
        if (pathWaypoints.Count > 0)
        {
            transform.position = pathWaypoints[0];
        }
    }

    private void Update()
    {
        if (pathWaypoints == null || currentWaypointIndex >= pathWaypoints.Count) return;

        // 1. Check if there is a defender nearby to attack first
        FindDefenderTarget();

        if (currentDefenderTarget != null)
        {
            // Attack the defender instead of moving
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackInterval)
            {
                attackTimer = 0f;
                currentDefenderTarget.TakeDamage(attackDamage);
                Debug.Log("Enemy attacked defender!");
            }
            return; // Pause movement while fighting
        }

        // 2. Normal Path Movement
        Vector3 target = pathWaypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        
        Vector3 dir = (target - transform.position).normalized;
        if (dir != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10f);
        }

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            currentWaypointIndex++;
            
            if (currentWaypointIndex >= pathWaypoints.Count)
            {
                DamageTowerAndDie();
            }
        }
    }

    private void FindDefenderTarget()
    {
        // If current target died, clear it
        if (currentDefenderTarget == null)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
            foreach (Collider hit in hitColliders)
            {
                if (hit.CompareTag("Defender"))
                {
                    if (hit.TryGetComponent<DefenderBase>(out DefenderBase defender))
                    {
                        currentDefenderTarget = defender;
                        break;
                    }
                }
            }
        }
        else
        {
            // Check if defender moved out of range or was destroyed
            float distance = Vector3.Distance(transform.position, currentDefenderTarget.transform.position);
            if (distance > attackRange || currentDefenderTarget == null)
            {
                currentDefenderTarget = null;
            }
        }
    }

    private void DamageTowerAndDie()
    {
        GameObject towerObj = GameObject.FindGameObjectWithTag("Tower");
        if (towerObj != null)
        {
            if (towerObj.TryGetComponent<TowerHealth>(out TowerHealth towerHealth))
            {
                towerHealth.TakeDamage(damageToTower);
            }
        }

        Destroy(gameObject);
    }
}