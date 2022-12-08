using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingStone : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] private bool Go;
    [SerializeField] private Vector3 lastPosition;
    [SerializeField] private float waitTimer;

    private Animator animator;
    private bool playerIsTriggered;
    private Vector3 target;
    private Vector3 startPosition;
    private float currentWaitTimer;

    void Start()
    {
        currentWaitTimer = waitTimer;
        animator = GetComponentInChildren<Animator>();
        startPosition = transform.position;
        target = lastPosition;
    }

    private void OnTriggerEnter(Collider playerCollider)
    {
        if (playerCollider.tag == "Player")
        {
            Go = true;
            animator.SetBool("Player Is Triggered?", true);
        }
    }

    private void Update()
    {
        StoneGo();
    }

    private void StoneGo()
    {
        if (Go)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * Speed);
        }

        if (transform.position == lastPosition)
        {
            Go = false;
            waitTimer -= Time.deltaTime;

            if (waitTimer < 0)
            {
                target = startPosition;
                waitTimer = currentWaitTimer;
                Go = true;
            }
        }

        if (transform.position == startPosition)
        {
            if (Go)
            {
                Go = false;
                target = lastPosition;
            }
        }
    }
}
