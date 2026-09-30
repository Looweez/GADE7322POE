using System.Collections.Generic;
using UnityEngine;

public class EnemyPathFollower : MonoBehaviour
{
    //public float speed = 3f;
    private List<Vector3> pathWaypoints;
    private int currentWaypointIndex = 0;

    /*public int damageToTower = 10;
    public int attackDamage = 10;
    public float attackRange = 1.2f;
    public float attackInterval = 1.5f;
    private float attackTimer;

    private DefenderBase currentDefenderTarget;*/
    
    [HideInInspector] public bool isPaused = false;
    private EnemyBase enemyBase;

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
        if (isPaused || pathWaypoints == null || currentWaypointIndex >= pathWaypoints.Count) return;

        float moveSpeed = (enemyBase != null) ? enemyBase.speed : 3f;

        Vector3 target = pathWaypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        Vector3 dir = (target - transform.position).normalized;
        if (dir != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10f);
        }

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            currentWaypointIndex++;
        }
    }

    /*private void FindDefenderTarget()
    {
 
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
            //checkif defender moved out of range or was destroyed
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
    }*/
}