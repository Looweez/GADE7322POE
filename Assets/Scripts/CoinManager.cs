using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public int coins;

    public void addCoin(int amount)
    {
        coins = coins + amount;
    }
}
