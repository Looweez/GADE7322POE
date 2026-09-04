using Unity.VisualScripting;
using UnityEngine;

public class EnemyAnt : EnemyBase
{
    
    //  public Transform[] waypoints; //waypoints for enemies to follow. idk how to set this up for random procedurally generated paths lol
    // private int wavepointIndex = 0;
   
    private void Start()
    {
        Initialize(); // Ensures health is set when spawned
    }

    private void Update()
    {
        MoveTowardsWaypoint();
    }

    public override void Initialize()
    {
        EnemyCurrentHealth = EnemyMaxHealth;

       
        if (waypoints != null && waypoints.Length > 0)
        {
            transform.position = waypoints[0].position;
        }
    }
    

    protected override void DoDamage(int damage)
    {
        LayerMask targetLayers = default;
        float attackRadius = 0.5f; 
        
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRadius, targetLayers);
        
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Tower"))
            {
                if (hitCollider.TryGetComponent<TowerHealth>(out TowerHealth tower))
                {
                    tower.TakeDamage(damage);
                }
            }
            else if(hitCollider.CompareTag("Defender"))
            {
                if (hitCollider.TryGetComponent<DefenderBase>(out DefenderBase defender))
                {
                    defender.TakeDamage(damage);
                }
            }
        }
    }


    /*void MoveTowardsWaypoint()
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

    void GetNextWaypoint()
    {
        // if the enemy reaches the final waypoint(the tower), the player loses lives
        if (wavepointIndex >= waypoints.Length - 1)
        {
            EndPath();
            return;
        }

        wavepointIndex++;
    }

    void EndPath()
    {
        // losing tower health here
        Destroy(gameObject);
    }

    public void TakeDamage(float amount)
    {
        EnemyCurrentHealth -= amount;

        if (EnemyCurrentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // loot logic like player gets money when enemies die idk
        Destroy(gameObject);
    }*/
}
