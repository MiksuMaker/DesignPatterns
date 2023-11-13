using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static event Action OnCoinCount;

    int coinsCollected = 0;
    public int Coins { get { return coinsCollected; } }

    private void Start()
    {
        // This needs to be a different event because the order can't be guaranteed
        Coin.OnCoinCollected += CountCoins; 
    }

    public void CountCoins()
    {
        coinsCollected++;

        OnCoinCount?.Invoke();
    }
}

