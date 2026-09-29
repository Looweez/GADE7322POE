using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Stats")]
    public float speed = 3f;
    public float EnemyMaxHealth = 50f;
    public float EnemyCurrentHealth;
    
    [Header("Combat")]
    public int damageToTower = 10;
    public int attackDamage = 10;

    public virtual void Initialize()
    {
        EnemyCurrentHealth = EnemyMaxHealth;
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
            Debug.Log("Added coins for defeating enemy!");
        }
        else
        {
            Debug.LogWarning("No CoinManager found in scene.");
        }

        Destroy(gameObject);
    }
}