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
    
    public void SelectDefenderToPlace()
    {

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
            currentOutline.transform.position = hit.point;
        }
    }
    
    private void TryPlaceDefender()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
        {
            if (HasEnoughGold(defenderCost))
            {
                DeductGold(defenderCost);
                
                // spawn defender
                Instantiate(defenderPrefab, hit.point, Quaternion.identity);

            }
            else
            {
                Debug.LogWarning("not enough gold");
            }
        }
    }
    
    public void CancelPlacement()
    {
        isPlacing = false;
        if (currentOutline != null)
        {
            Destroy(currentOutline);
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

    private void DeductGold(int amount)
    {
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.Purchase(amount);
        }
    }
}
