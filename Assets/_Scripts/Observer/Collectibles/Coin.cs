using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Collectible
{
    public static event Action OnCoinCollected;


    protected override void HandleTriggerEnter(Collider other)
    {
        // Handle Collision
        Destroy(this.gameObject);
    }

    private void OnDisable()
    {
        OnCoinCollected?.Invoke();
    }
}
