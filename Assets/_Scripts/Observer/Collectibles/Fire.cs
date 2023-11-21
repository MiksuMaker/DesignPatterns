using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Collectible
{

    protected override void HandleTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            // Set them on fire

            other.gameObject.GetComponent<PlayerMover>().StartBurning();
        }
    }

    private void OnDisable()
    {

    }
}
