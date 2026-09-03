using UnityEngine;

public class ShopManager : MonoBehaviour
{
    CoinManager coinManager;
    DefenderSpawner _defenderSpawner;
    
    private int cupcakePrice = 25;
    private int snowballPrice; //these two will be used laterss
    private int jellytotPrice;

    public void BuyCupcake()
    {
        coinManager.coins -= cupcakePrice;
        _defenderSpawner.SpawnDefender("Cupcake");
    }
    
}
