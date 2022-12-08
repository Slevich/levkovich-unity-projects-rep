using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObjects : MonoBehaviour
{

    private void OnTriggerEnter(Collider playerCollider)
    {
        if (playerCollider.tag == "Player")
        {
            FallObject();
        }
    }

    private void FallObject()
    {
        Rigidbody[] boxes = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody b in boxes)
        {
            b.useGravity = true;
        }
    }
}
