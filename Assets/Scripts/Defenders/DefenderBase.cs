using UnityEngine;

public class DefenderBase : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float speed;
    public float defenderMaxHealth;
    public float defenderCurrentHealth;
    
    public virtual void Initialize()
    {
        defenderCurrentHealth = defenderMaxHealth;
        
        /*if (waypoints != null && waypoints.Length > 0)
        {
            transform.position = waypoints[0].position;
        }*/
    }

    public void TakeDamage(int damage)
    {
        defenderCurrentHealth -= damage;
        if (defenderCurrentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
