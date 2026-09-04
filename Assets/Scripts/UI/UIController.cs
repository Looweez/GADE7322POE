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

    [SerializeField] private string prefix = "Coins: "; // made prefix we can change it to sugarcubes or candies orsomething

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (CoinManager.Instance != null && coinText != null)
        {
            coinText.text = $"{prefix}{CoinManager.Instance.coins}";
        }
    }

    private void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    public void UpdateTowerHealthText()
    {
        if (towerHealthText != null)
            towerHealthText.text = $"Tower Health: {Mathf.Max(0, towerHealth.currentHealth)} / {towerHealth.maxHealth}";
    }
    
    public void GameOver()
    {
        if (gameOverPanel != null) 
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f; // freeze!! ure under arrest!!
    }
    
    
}
