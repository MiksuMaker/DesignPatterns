using System;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    #region Properties
    List<Achievement> achievements = new List<Achievement>();

    #endregion

    #region Setup
    private void Start()
    {
        SetupAchievements();
    }
    private void SetupAchievements()
    {
        //achievements.Add(new A_CoinCollector(this, Coin.OnCoinCollected));
        achievements.Add(new A_CoinCollector(FindObjectOfType<ScoreManager>()));
    }
    #endregion

    #region Achievement Handling

    #endregion
}

