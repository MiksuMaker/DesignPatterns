using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Collectible
{
    public static event Action OnCoinCollected;


    protected override void HandleTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            // Handle Collision
            Destroy(this.gameObject);
        }
    }

    private void OnDisable()
    {
        OnCoinCollected?.Invoke();
    }
}
