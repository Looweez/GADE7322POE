using UnityEngine;

public class UpgradableEntity : MonoBehaviour
{
    [Header("Upgrade Settings")]
    public int currentLevel = 1;
    public int maxLevel = 3;
    public int baseUpgradeCost = 30;
    public int costMultiplierPerLevel = 20;

    [Header("Stat Boosts Per Level")]
    public float damageIncrease = 5f;
    public float healthIncrease = 25f;
    private DefenderCupcake cupcakeScript;
    private DefenderJellyTot jellyTotScript;
    private DefenderSnowball snowballScript;
    private TowerHealth towerHealthScript;

    void Awake()
    {
        cupcakeScript = GetComponent<DefenderCupcake>();
        jellyTotScript = GetComponent<DefenderJellyTot>();
        snowballScript = GetComponent<DefenderSnowball>();
        towerHealthScript = GetComponent<TowerHealth>();
    }

    public int GetUpgradeCost()
    {
        if (currentLevel >= maxLevel) return 0;
        return baseUpgradeCost + ((currentLevel - 1) * costMultiplierPerLevel);
    }

    public bool CanUpgrade()
    {
        return currentLevel < maxLevel && CoinManager.Instance != null && CoinManager.Instance.CanAfford(GetUpgradeCost());
    }

    public void UpgradeUnit()
    {
        if (!CanUpgrade()) return;

        int cost = GetUpgradeCost();
        if (CoinManager.Instance.Purchase(cost))
        {
            currentLevel++;
            ApplyStatUpgrades();
            Debug.Log($"{gameObject.name} upgraded to Level {currentLevel}!");
        }
    }

    private void ApplyStatUpgrades()
    {
        if (cupcakeScript != null)
        {
            cupcakeScript.attackDamage += damageIncrease;
        }
        else if (jellyTotScript != null)
        {
            jellyTotScript.attackDamage += damageIncrease;
        }
        else if (snowballScript != null)
        {
            snowballScript.attackDamage += damageIncrease;
        }
        else if (towerHealthScript != null)
        {
            towerHealthScript.maxHealth += healthIncrease;
            towerHealthScript.currentHealth += healthIncrease;
            UIController.Instance?.UpdateTowerHealthText();
        }
    }

 // ui popup preview for stats
    public float GetNextDamagePreview(out float current)
    {
        current = 0;
        if (cupcakeScript != null) current = cupcakeScript.attackDamage;
        else if (jellyTotScript != null) current = jellyTotScript.attackDamage;
        else if (snowballScript != null) current = snowballScript.attackDamage;
        return damageIncrease;
    }

    public float GetNextHealthPreview(out float current)
    {
        current = 0;
        if (towerHealthScript != null) current = towerHealthScript.maxHealth;
        return healthIncrease;
    }
}
