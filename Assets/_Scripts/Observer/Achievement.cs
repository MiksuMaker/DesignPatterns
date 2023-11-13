using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Achievement
{
    public abstract void Check();
}

public class Achievement_1
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
        
        if (hAchievement.IsCompleted) {
            Debug.Log("JEE");
        }
    }
}

public class HienoAchevement
{
    public Func<bool> Condition;
    public bool IsCompleted => Condition();
}