using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Stats")]
    public float speed = 3f;
    public float EnemyMaxHealth = 50f;
    public float EnemyCurrentHealth;
    
    [Header("Combat")]
    //public int damageToTower = 10;
    public float attackDamage = 10;
    public float attackRange = 1.5f;
    public float attackInterval = 1.5f;
    private float attackTimer;
    
    [Header("References")]
    public EnemyPathFollower pathFollower;

    public virtual void Initialize()
    {
        EnemyCurrentHealth = EnemyMaxHealth;
        if (pathFollower == null)
            pathFollower = GetComponent<EnemyPathFollower>();
    }

    protected virtual void Update()
    {
        attackTimer += Time.deltaTime;

        if (TryAttackTargets())
        {
            if (pathFollower != null) pathFollower.isPaused = true;
        }
        else
        {
            if (pathFollower != null) pathFollower.isPaused = false;
        }
    }

    private bool TryAttackTargets()
    {
        //checking for defenders
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("Defender") && hit.TryGetComponent<DefenderBase>(out DefenderBase defender))
            {
                if (attackTimer >= attackInterval)
                {
                    attackTimer = 0f;
                    PerformAttack(defender);
                }

                return true; //target found
            }
        }

        //looking for tower
        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("Tower") && hit.TryGetComponent<TowerHealth>(out TowerHealth towerHealth))
            {
                if (attackTimer >= attackInterval)
                {
                    attackTimer = 0f;
                    PerformAttack(towerHealth);
                }

                return true; //target found
            }
        }
        
        return false; //no targets in range
    }

    protected virtual void PerformAttack(DefenderBase defender)
    {
        defender.TakeDamage(attackDamage);
    }
    
    protected virtual void PerformAttack(TowerHealth towerHealth)
    {
        towerHealth.TakeDamage(attackDamage);
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
            Destroy(gameObject);
            Debug.Log("Added coins for defeating enemy!");
        }
        else
        {
            Debug.LogWarning("No CoinManager found in scene.");
        }

        Destroy(gameObject);
    }
    
    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}