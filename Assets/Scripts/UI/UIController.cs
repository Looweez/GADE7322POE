using System;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public TowerHealth towerHealth;
    public static UIController Instance;
    public TextMeshProUGUI towerHealthText;

    public GameObject gameOverPanel;

    [SerializeField] private TMP_Text coinText;
    [SerializeField] private string prefix = "Coins: "; 
    
    //wave number ui
    //public UnityEngine.UI.Text waveText; 
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

        // BULLETPROOF FIX: Auto-find the TowerHealth script if it wasn't dragged in the inspector
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
        if (towerHealthText != null && towerHealth != null)
        {
            towerHealthText.text = $"Tower Health: {Mathf.Max(0, towerHealth.currentHealth)} / {towerHealth.maxHealth}";
        }
        else
        {
            Debug.LogWarning("UIController is missing a reference to towerHealthText or towerHealth!");
        }
    }
    
    public void GameOver()
    {
        if (gameOverPanel != null) 
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f; 
    }
}
