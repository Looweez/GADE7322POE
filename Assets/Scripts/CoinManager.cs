using UnityEngine;

public class CoinManager : MonoBehaviour
{
   //singleton
    public static CoinManager Instance { get; private set; }

    public int coins;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void addCoin(int amount)
    {
        coins += amount;
    }

    // make sure player can afford defedner
    public bool CanAfford(int amount)
    {
        return coins >= amount; //coins greater than cost amount
    }

   //buy defender if they have enough
    public bool Purchase(int amount)
    {
        if (CanAfford(amount))
        {
            coins -= amount;
            Debug.Log($"Spent {amount} coins. current coins: {coins}");
            return true;
        }

        Debug.LogWarning("Not enough coins!");
        return false;
    }
}
