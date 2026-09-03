using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [SerializeField] private TMP_Text coinText;

    [SerializeField] private string prefix = "Coins: "; // made prefix we can change it to sugarcubes or candies orsomething
    private void Update()
    {
        if (CoinManager.Instance != null && coinText != null)
        {
            coinText.text = $"{prefix}{CoinManager.Instance.coins}";
        }
    }
}
