using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapNeedle : MonoBehaviour
{
    [SerializeField] private Vector3 lastPosition;
    [SerializeField] private float speed;
    [SerializeField] private bool Go;
    [SerializeField] private float waitTimer;

    private Vector3 startPosition;
    private Vector3 target;
    private float currentWaitTimer;

    private void Start()
    {
        currentWaitTimer = waitTimer;
        startPosition = transform.position;
        target = lastPosition;
    }


    private void Update()
    {
        needleMovement();
    }

    private void needleMovement()
    {
        if (Go)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
        }

        if (transform.position == lastPosition)
        {
            Go = false;
            waitTimer -= Time.deltaTime;

            if (waitTimer < 0)
            {
                Go = true;
                target = startPosition;
                waitTimer = currentWaitTimer;
            }
        }
        else if (transform.position == startPosition)
        {
            Go = false;
            waitTimer -= Time.deltaTime;

            if (waitTimer < 0)
            {
                Go = true;
                target = lastPosition;
                waitTimer = currentWaitTimer;
            }
        }
    }
}
