using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class EnemyBase : MonoBehaviour
{
    //CoinManager coinManager;
    
    public float speed = 3f;
    public float EnemyMaxHealth = 50f;
    public float EnemyCurrentHealth;
    
    
    public Transform[] waypoints; //waypoints for enemies to follow. idk how to set this up for random procedurally generated paths lol
    private int wavepointIndex = 0;

    public virtual void Initialize()
    {
        EnemyCurrentHealth = EnemyMaxHealth;

       
        if (waypoints != null && waypoints.Length > 0)
        {
            transform.position = waypoints[0].position;
        }
    }

    protected void MoveTowardsWaypoint()
    {
        if (waypoints == null || wavepointIndex >= waypoints.Length) return;

        // move  towards the current target waypoint
        transform.position = Vector3.MoveTowards(
            transform.position, 
            waypoints[wavepointIndex].position, 
            speed * Time.deltaTime
        );

        // check if the enemy is close enough to the waypoint to switch targets
        if (Vector3.Distance(transform.position, waypoints[wavepointIndex].position) <= 0.1f)
        {
            GetNextWaypoint();
        }
    }

    protected void GetNextWaypoint()
    {
        // if the enemy reaches the final waypoint(the tower), the player loses lives
        if (wavepointIndex >= waypoints.Length - 1)
        {
            EndPath();
            return;
        }

        wavepointIndex++;
    }

    protected void EndPath()
    {
        // losing tower health here
        Destroy(gameObject);
    }

    public virtual void TakeDamage(float amount)
    {
        EnemyCurrentHealth -= amount;

        if (EnemyCurrentHealth <= 0)
        {
            Die();
        }
    }

    protected void Die()
    {
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.addCoin(10);
            Debug.Log("added coins");
        }
        else
        {
            Debug.LogWarning("no coinmanager");
        }

        Destroy(gameObject);
    }

    protected virtual void DoDamage(int amount)
    {
        
    }
}
