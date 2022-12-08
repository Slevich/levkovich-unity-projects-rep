using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnawBalls : MonoBehaviour
{
    private Rigidbody snowBallRigidbody;

    private void Start()
    {
        snowBallRigidbody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider playerCollider)
    {
        if (playerCollider.tag == "Player")
        {
            snowBallRigidbody.useGravity = true;
        }
    }
}
