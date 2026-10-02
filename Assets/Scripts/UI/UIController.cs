using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class UIController : MonoBehaviour
{
    public TowerHealth towerHealth;
    public static UIController Instance;
    public TextMeshProUGUI towerHealthText;

    public GameObject gameOverPanel;

    [SerializeField] private TMP_Text coinText;
    [SerializeField] private string prefix = "Coins: "; 
    
    //wave number ui
    [SerializeField] private TMP_Text waveText;

    public void UpdateWaveText(int waveNumber)
    {
        if (waveText != null)
        {
            waveText.text = "Wave: " + waveNumber;
            Debug.Log("UI updated: wave " + waveNumber);
        }
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

       
        if (towerHealth == null)
        {
            GameObject tower = GameObject.FindGameObjectWithTag("Tower");
            if (tower != null)
            {
                towerHealth = tower.GetComponent<TowerHealth>();
            }
        }

        UpdateTowerHealthText();
    }

    private void Update()
    {
        if (CoinManager.Instance != null && coinText != null)
        {
            coinText.text = $"{prefix}{CoinManager.Instance.coins}";
        }
    }

    public void UpdateTowerHealthText()
    {
     
        if (towerHealth == null)
        {
            GameObject tower = GameObject.FindGameObjectWithTag("Tower");
            if (tower != null)
            {
                towerHealth = tower.GetComponent<TowerHealth>();
            }
        }

        if (towerHealthText == null)
        {
            towerHealthText = GetComponentInChildren<TextMeshProUGUI>();
        }

        if (towerHealthText != null && towerHealth != null)
        {
            towerHealthText.text = $"Tower Health: {Mathf.Max(0, towerHealth.currentHealth)} / {towerHealth.maxHealth}";
        }
        else
        {
            Debug.LogWarning("UIController still cannot find towerHealthText or towerHealth! Check your Tower tag.");
        }
    }
    
    [Header("Shop References")]
    public GameObject shopPanel;

    public void GameOver()
    {
        if (gameOverPanel != null) 
            gameOverPanel.SetActive(true);

      
        if (shopPanel != null)
            shopPanel.SetActive(false);
        
        DefenderPlacementManager placementManager = FindObjectOfType<DefenderPlacementManager>();
        if (placementManager != null)
        {
            placementManager.CancelPlacement();
        }

        Time.timeScale = 0f;
    }

   
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }
}