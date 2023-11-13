using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        HandleTriggerEnter(other);
    }

    protected virtual void HandleTriggerEnter(Collider other)
    {

    }
}
