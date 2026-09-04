using UnityEngine;

public class TowerHealth : MonoBehaviour
{
    
    //handles tower (the players base) health, losing health if enemies reach it, and destroying tower/ending game if health reaches 0
    public int maxHealth = 100;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        UIController.Instance?.UpdateTowerHealthText();
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
        UIController.Instance?.GameOver();
        Destroy(gameObject);
    }
}
