using UnityEngine;
using UnityEngine.UI;

public class WorldHeathBarManager : MonoBehaviour
{
    public Slider slider;
    public Vector3 offset = new Vector3(0, 2f, 0);
    
    private Camera mainCamera;
    private Transform targetUnit;

    public void Setup(Transform unitTransform, float maxHealth)
    {
        targetUnit = unitTransform;
        mainCamera = Camera.main;

        if (slider != null)
        {
            slider.maxValue = maxHealth;
            slider.value = maxHealth;
        }
    }

    public void UpdateHealth(float currentHealth)
    {
        if (slider != null)
        {
            slider.value = currentHealth;
        }
    }

    private void LateUpdate() //billboarding
    {
        if (targetUnit != null)
        {
            Destroy(gameObject);
            return;
        }
        
        transform.position = targetUnit.position + offset;

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        
        if (mainCamera != null)
        {
            transform.rotation = mainCamera.transform.rotation;
        }
    }
}
