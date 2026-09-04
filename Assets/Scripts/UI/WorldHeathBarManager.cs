using UnityEngine;
using UnityEngine.UI;

public class WorldHeathBarManager : MonoBehaviour
{
    public Slider slider;
    public Vector3 offset = new Vector3(0, 2f, 0);
    
    private Camera mainCamera;
    private Transform targetUnit;

    private EnemyBase enemyRef;
    private DefenderBase defenderRef;

    private void Start()
    {
        mainCamera = Camera.main;

        // Since this script is on the Canvas, the character is our direct parent!
        if (transform.parent != null)
        {
            targetUnit = transform.parent;
            enemyRef = targetUnit.GetComponent<EnemyBase>();
            defenderRef = targetUnit.GetComponent<DefenderBase>();
        }

        // Automatically find the slider child if it's not dragged in
        if (slider == null)
        {
            slider = GetComponentInChildren<Slider>();
        }

        // Initialize health values
        if (slider != null)
        {
            if (enemyRef != null)
            {
                slider.maxValue = enemyRef.EnemyMaxHealth;
                slider.value = enemyRef.EnemyCurrentHealth;
            }
            else if (defenderRef != null)
            {
                slider.maxValue = defenderRef.defenderMaxHealth;
                slider.value = defenderRef.defenderCurrentHealth;
            }
        }
    }

    private void Update() 
    {
        // Continuously sync the slider with current health
        if (slider != null)
        {
            if (enemyRef != null)
            {
                slider.maxValue = enemyRef.EnemyMaxHealth;
                slider.value = enemyRef.EnemyCurrentHealth;
            }
            else if (defenderRef != null)
            {
                slider.maxValue = defenderRef.defenderMaxHealth;
                slider.value = defenderRef.defenderCurrentHealth;
            }
        }
    }

    private void LateUpdate() // billboarding & following
    {
        if (targetUnit == null) return;

        // Follow the character's position with an upward offset
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