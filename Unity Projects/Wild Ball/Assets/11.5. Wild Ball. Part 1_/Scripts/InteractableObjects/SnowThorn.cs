using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowThorn : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] private bool Go;
    [SerializeField] private Vector3 lastPosition;

    private Vector3 target;

    void Start()
    {
        target = lastPosition;
    }

    private void OnTriggerEnter(Collider playerCollider)
    {
        if (playerCollider.tag == "Player")
        {
            Go = true;
        }
    }

    private void Update()
    {
        snowThornMovement();
    }

    private void snowThornMovement()
    {
        if (Go)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * Speed);
        }

        if (transform.position == lastPosition)
        {
            Go = false;
        }
    }
}
