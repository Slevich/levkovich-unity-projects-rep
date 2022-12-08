using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Vector3 lastPosition;
    [SerializeField] private float Speed;

    private bool Go;
    private Vector3 target;
    private Vector3 startPosition;

    private void Start()
    {
        target = lastPosition;
        startPosition = transform.position;
    }

    private void OnTriggerEnter(Collider playerCollider)
    {
        if (playerCollider.tag == "Player")
        {
            Go = true;
        }
    }

    private void OnTriggerExit(Collider playerCollider)
    {
        if (playerCollider.tag == "Player")
        {
            Go = false;
        }
    }

    private void Update()
    {
        platformMovement();
    }

    private void platformMovement()
    {
        if (Go)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * Speed);
        }

        if (transform.position == lastPosition)
        {
            target = startPosition;
        }
    }
}
