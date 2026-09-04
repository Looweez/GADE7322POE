using UnityEngine;

public class TowerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        UIController.Instance?.UpdateTowerHealthText();
    }

    public void TakeDamage(int damageAmount) 
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