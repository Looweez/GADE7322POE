using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public DefenderPlacementManager placementManager;
    

    public GameObject cupcakePrefab;
    public GameObject snowballPrefab;
    public GameObject jellytotPrefab;
    
    public int cupcakePrice = 10;
    public int snowballPrice = 20; //aadded later
    public int jellytotPrice = 5;
    
    public void BuyCupcake()
    {
        if (CoinManager.Instance != null && CoinManager.Instance.CanAfford(cupcakePrice))
        {
            placementManager.SelectDefenderToPlace(cupcakePrefab, cupcakePrice); //if the player can afford a cupcake, they go into placement mode
        }
        else
        {
            Debug.LogWarning("not enough coins");
        }
    }
    
    public void BuyJellytot()
    {
        if (CoinManager.Instance != null && CoinManager.Instance.CanAfford(jellytotPrice))
        {
            placementManager.SelectDefenderToPlace(jellytotPrefab, jellytotPrice);
        }
        else
        {
            Debug.LogWarning("not enough coins");
        }
    }
    
    public void BuySnowball()
    {
        if (CoinManager.Instance != null && CoinManager.Instance.CanAfford(snowballPrice))
        {
            placementManager.SelectDefenderToPlace(snowballPrefab, snowballPrice);
        }
        else
        {
            Debug.LogWarning("not enough coins");
        }
    }
}