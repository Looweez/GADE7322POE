using UnityEngine;

public class TowerDamageFlash : MonoBehaviour
{
    public static TowerDamageFlash Instance { get; private set; }

    [Header("References")]
    public Material damageFlashMaterial;

    [Header("Settings")]
    public float flashDuration = 0.3f;
    public float maxIntensity = 0.5f;

    private float currentIntensity = 0f;
    private float flashTimer = 0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        //ensures intensity is at 0 when game starts
        if (damageFlashMaterial != null)
        {
            damageFlashMaterial.SetFloat("_FlashIntensity", 0f);
        }
    }

    void Update()
    {
        if (flashTimer > 0)
        {
            flashTimer -= Time.deltaTime;
            currentIntensity = Mathf.Lerp(0f, maxIntensity, flashTimer / flashDuration);
            
            if (damageFlashMaterial != null)
            {
                damageFlashMaterial.SetFloat("_FlashIntensity", currentIntensity);
            }
        }
    }
    
    public static void TriggerFlash()
    {
        if (Instance != null)
        {
            Instance.flashTimer = Instance.flashDuration;
        }
    }
}