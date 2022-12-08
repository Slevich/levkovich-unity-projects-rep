using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoScript : MonoBehaviour
{
    [SerializeField] private Vector3 lastPosition;
    [SerializeField] private float Speed;
    [SerializeField] private float waitTimer;

    private float currentWaitTimer;
    private bool Go;
    private Animator NPCAnimator;
    private Vector3 target;
    private Vector3 startPosition;

    private void Start()
    {
        currentWaitTimer = waitTimer;
        NPCAnimator = GetComponent<Animator>();
        target = lastPosition;
        startPosition = transform.position;
    }

    private void Update()
    {
        NPCmovement();
    }

    private void NPCmovement()
    {
        transform.LookAt(target);

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
                Go = true;
                waitTimer = currentWaitTimer;
                target = startPosition;
            }
        }
        else if (transform.position == startPosition)
        {
            waitTimer -= Time.deltaTime;

            if (waitTimer < 0)
            {
                target = lastPosition;
                Go = true;
                waitTimer = currentWaitTimer;
            }
        }

        if (transform.position != startPosition && transform.position != lastPosition)
        {
            NPCAnimator.SetBool("Go", true);
        }
        else
        {
            NPCAnimator.SetBool("Go", false);
        }
    }
}
