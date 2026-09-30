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

        // FIX: Tell the UI controller to update the text right when damage is taken!
        UIController.Instance?.UpdateTowerHealthText();

        if (currentHealth <= 0)
        {
            DestroyTower();
        }
    }

    void DestroyTower()
    {
        Debug.Log("Tower destroyed");
        UIController.Instance?.GameOver();
        Destroy(gameObject);
    }
}