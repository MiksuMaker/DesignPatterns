using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Achievement
{
    public bool isCompleted = false;
    public string name = "Default Name";
    public string Description = "Default Description";

    protected AchievementManager manager;

    // Add necessary references, pass them in with the Constructor
    public Achievement() { }

    // With Check function, check if the conditions have passed
    public abstract void Check();
}

public class A_CoinCollector : Achievement
{
    int coinsNeeded = 2;
    ScoreManager scoreManager;

    public A_CoinCollector(ScoreManager scM)
    {
        //Debug.Log("")
        scoreManager = scM;
        //manager = m;
        //Coin.OnCoinCollected += Check;
        //scM.OnCoinsCount += Check;

        ScoreManager.OnCoinCount += Check;

        name = "Coin Collector";
    }

    public override void Check()
    {
        if (isCompleted == true) { return; }

        if (scoreManager.Coins >= coinsNeeded)
        {
            Debug.Log("ACHIEVEMENT UNLOCKED: Coin Collector");
            isCompleted = true;
        }

    }
}

public class A_Terminator : Achievement
{
    public A_Terminator(AchievementManager m)
    {
        manager = m;
    }

    public override void Check()
    {
        if (isCompleted == true) { return; }

        //if (scoreManager.Coins >= coinsNeeded)
        //{
        //    Debug.Log("ACHIEVEMENT UNLOCKED: Coin Collector");
        //}
        isCompleted = true;
    }
}

#region JoniHelen_Esimerkki
public class JoniHelenEsimerkki
{
    HienoAchevement hAchievement;

    public void Prime()
    {
        hAchievement = new HienoAchevement();
        //var hAchievement = new HienoAchevement();

        hAchievement.Condition = () => { return true; };
    }

    public void Check()
    {
        // Se koodi mikä tarkistaa onko ehdot täyttyneet

        if (hAchievement.IsCompleted)
        {
            Debug.Log("JEE");
        }
    }
}
public class HienoAchevement
{
    public Func<bool> Condition;
    public bool IsCompleted => Condition();
}
#endregion