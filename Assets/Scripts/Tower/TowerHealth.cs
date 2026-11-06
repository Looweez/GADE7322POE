using UnityEngine;

public class TowerHealth : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;
    
    public Material jamSplatterMaterial;

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
    
        // Calculate intensity based on missing health (e.g., 0 to 1)
        float healthPercent = currentHealth / maxHealth;
        float splatterIntensity = 1f - healthPercent; 

        // Send the value to your JamSplatter shader material property
        if (jamSplatterMaterial != null)
        {
            jamSplatterMaterial.SetFloat("_HealthIntensity", splatterIntensity); // Match your property name
        }

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