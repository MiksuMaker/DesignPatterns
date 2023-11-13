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

    [ContextMenu("Check unlocked achievements")]
    private void CheckUnlockedAchievements()
    {
        foreach (var a in achievements)
        {
            string output = "Achievement \"";
            output += a.name;
            output += "\" is ";
            output += a.isCompleted ? "UNLOCKED!" : "locked.";
            Debug.Log(output);
        }
    }

}

