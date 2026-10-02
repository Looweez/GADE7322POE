using UnityEngine;

public class TowerHealth : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        UIController.Instance?.UpdateTowerHealthText();
    }

    public void TakeDamage(float damageAmount) 
    {
        currentHealth -= damageAmount;
        Debug.Log("Tower health:" + currentHealth);

        // Tell the UI controller to update the text right when damage is taken
        UIController.Instance?.UpdateTowerHealthText();

        if (currentHealth <= 0)
        {
            DestroyTower();
        }
    }

    // Added this method to support the EnemySpawner skill adaptation system
    public float GetCurrentHealthPercentage()
    {
        if (maxHealth <= 0f) return 0f;
        return currentHealth / maxHealth;
    }

    void DestroyTower()
    {
        Debug.Log("Tower destroyed");
        UIController.Instance?.GameOver();
        Destroy(gameObject);
    }
}