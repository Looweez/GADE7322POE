using UnityEngine;

public class TowerHealth : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;

       
        if (UIController.Instance != null)
        {
            UIController.Instance.towerHealth = this;
            UIController.Instance.UpdateTowerHealthText();
        }
        else
        {
            Debug.LogWarning("TowerHealth spawned, but UIController.Instance was not found!");
        }
    }

    public void TakeDamage(float damageAmount) 
    {
        currentHealth -= damageAmount;
        Debug.Log("Tower health:" + currentHealth);

    
        UIController.Instance?.UpdateTowerHealthText();

        if (currentHealth <= 0)
        {
            DestroyTower();
        }
    }

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