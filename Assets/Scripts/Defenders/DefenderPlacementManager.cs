using UnityEngine;
using UnityEngine.InputSystem;

public class DefenderPlacementManager : MonoBehaviour
{
    public Camera mainCamera;
    public LayerMask terrainLayer;
    
    public GameObject defenderPrefab;
    public Material defenderOutlineMaterial; // for player to see outline of defender before they place it
    public int defenderCost = 25;
    
    private GameObject currentOutline;
    private bool isPlacing = false;
    
    private void Update()
    {
        if (!isPlacing) return;

        UpdateOutlinePosition();
        
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            TryPlaceDefender();
        }
        
    }
    
    public void SelectDefenderToPlace(GameObject prefabToPlace, int cost)
    {
        CancelPlacement(); //removes outline/preview thing after they place down defender

        isPlacing = true;
        
        defenderPrefab = prefabToPlace;
        defenderCost = cost;
        isPlacing = true;
        
        currentOutline = Instantiate(defenderPrefab);
        
        // disable colliders on the preview so it doesnt block projectiles and enemies
        foreach (var collider in currentOutline.GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
        }
        if (defenderOutlineMaterial != null)
        {
            foreach (var renderer in currentOutline.GetComponentsInChildren<Renderer>())
            {
                renderer.material = defenderOutlineMaterial;
            }
        }
    }
    
    private void UpdateOutlinePosition() //to see where they gonna place the defender
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
        {
            currentOutline.transform.position = hit.point; //display preview if they can place defender there
            
        }
    }
    
    private void TryPlaceDefender()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()); //raycast to ground

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f)) //if there is ground for defender to be placed on
        {
            if (HasEnoughGold(defenderCost)) // and if player has enough gold
            {
                SpendGold(defenderCost); //spend amt of gold = to cost of defender
                
                // spawn defender
                Instantiate(defenderPrefab, hit.point, Quaternion.identity);
                
                CancelPlacement();

            }
            else
            {
                Debug.LogWarning("not enough gold");
            }
        }
    }
    
    public void CancelPlacement() //messing it up idk
    {
        isPlacing = false;
        if (currentOutline != null)
        {
            Destroy(currentOutline); //cancel the placement
        }
    }
    
    private bool HasEnoughGold(int amount)
    {
        if (CoinManager.Instance != null)
        {
            return CoinManager.Instance.CanAfford(amount);
        }
        return false; 
    }

    private void SpendGold(int amount)
    {
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.Purchase(amount);
        }
    }
}
