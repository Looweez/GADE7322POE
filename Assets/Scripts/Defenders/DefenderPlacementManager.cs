using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DefenderPlacementManager : MonoBehaviour
{
    public Camera mainCamera;
    public LayerMask terrainLayer;
    
    [Header("Path Blocking Settings")]
    public DynamicPathGenerator pathGenerator;
    public float pathBlockedRadius = 1.5f; // How close a defender can be to the path

    [Header("Defender Settings")]
    public GameObject defenderPrefab;
    public Material defenderOutlineMaterial; // for player to see outline of defender before they place it
    public int defenderCost = 25;
    
    private GameObject currentOutline;
    private bool isPlacing = false;

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        // Auto-find path generator if not assigned in inspector
        if (pathGenerator == null)
        {
            pathGenerator = FindObjectOfType<DynamicPathGenerator>();
        }
    }
    
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
        CancelPlacement();

        isPlacing = true;
        defenderPrefab = prefabToPlace;
        defenderCost = cost;
        
        currentOutline = Instantiate(defenderPrefab);
        
        // disable colliders so doesn't block enemies or projectiles
        foreach (var collider in currentOutline.GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
        }

        // disable scripts on the preview so it doesn't attack or be attacked
        foreach (var script in currentOutline.GetComponentsInChildren<MonoBehaviour>())
        {
            if (script != this)
            {
                script.enabled = false;
            }
        }

        if (defenderOutlineMaterial != null)
        {
            foreach (var renderer in currentOutline.GetComponentsInChildren<Renderer>())
            {
                renderer.material = defenderOutlineMaterial;
            }
        }
    }
    
    private void UpdateOutlinePosition()
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
            // CHECK 1: Is the drop location too close to an enemy path?
            if (IsTooCloseToPath(hit.point))
            {
                Debug.LogWarning("Cannot place defender on or near the enemy path!");
                return;
            }

            // CHECK 2: Does the player have enough gold?
            if (HasEnoughGold(defenderCost)) 
            {
                SpendGold(defenderCost);
                
                // Spawn actual defender instead of the preview
                Instantiate(defenderPrefab, hit.point, Quaternion.identity);
                
                CancelPlacement();
            }
            else
            {
                Debug.LogWarning("Not enough gold");
            }
        }
    }

    private bool IsTooCloseToPath(Vector3 position)
    {
        if (pathGenerator == null || pathGenerator.enemyPaths == null) return false;

        foreach (var path in pathGenerator.enemyPaths)
        {
            foreach (var pathPoint in path)
            {
                // Check horizontal distance (X and Z) so terrain height differences don't mess it up
                float distance = Vector2.Distance(new Vector2(position.x, position.z), new Vector2(pathPoint.x, pathPoint.z));
                if (distance < pathBlockedRadius)
                {
                    return true; // Too close to the road!
                }
            }
        }
        return false;
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

    private void SpendGold(int amount)
    {
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.Purchase(amount);
        }
    }
}