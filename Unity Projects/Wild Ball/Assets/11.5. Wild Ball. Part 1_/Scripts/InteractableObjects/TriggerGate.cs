using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGate : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] private bool Go;
    [SerializeField] private Vector3 lastPosition;
    [SerializeField] private Transform gate;

    private Vector3 startPosition;
    private Vector3 target;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        gateMovement();
    }

    private void OnTriggerEnter(Collider playerCollider)
    {
        if (playerCollider.tag == "Player")
        {
            Go = true;
            target = lastPosition;
        }
    }

    private void OnTriggerExit(Collider playerCollider)
    {
        if (playerCollider.tag == "Player")
        {
            Go = true;
            target = startPosition;
        }
    }

    private void gateMovement()
    {
        if (Go)
        {
            gate.position = Vector3.MoveTowards(gate.position, target, Time.deltaTime * Speed);
        }

        if (gate.position == lastPosition)
        {
            Go = false;
        }
    }

}
