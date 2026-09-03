using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public DefenderPlacementManager placementManager;
    

    public GameObject cupcakePrefab;
    public GameObject snowballPrefab;
    public GameObject jellytotPrefab;
    
    public int cupcakePrice = 25;
    public int snowballPrice = 35; //aadded later
    public int jellytotPrice = 50;
    
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
}