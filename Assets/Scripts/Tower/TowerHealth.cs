using UnityEngine;

public class TowerHealth : MonoBehaviour
{
    //handles tower (the players base) health, losing health if enemies reach it, and destroying tower/ending game if health reaches 0
    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount) //for when enemy gets through defenders and reaches the tower
    {
        currentHealth -= damageAmount;
        Debug.Log("Tower health:" + currentHealth);

        if (currentHealth <= 0)
        {
            DestroyTower();
        }
    }

    void DestroyTower()
    {
        Debug.Log("Tower destroyed");
        //need to add gameover screen / restart here
        Destroy(gameObject);
    }
}
