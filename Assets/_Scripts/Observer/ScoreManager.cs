using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static event Action OnCoinCount;
    public static event Action OnEnemySlain;

    int coinsCollected = 0;
    public int CoinsCollected { get { return coinsCollected; } }

    int enemiesSlain = 0;
    public int EnemiesSlain { get { return enemiesSlain; } }

    private void Start()
    {
        // This needs to be a different event because the order can't be guaranteed
        Coin.OnCoinCollected += CoinWasCollected;
    }

    public void CoinWasCollected()
    {
        coinsCollected++;

        OnCoinCount?.Invoke();
    }

    public void EnemyWasSlain()
    {
        enemiesSlain++;

        OnEnemySlain?.Invoke();
    }
}

