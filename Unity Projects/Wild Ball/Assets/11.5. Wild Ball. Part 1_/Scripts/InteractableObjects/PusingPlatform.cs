using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PusingPlatform : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private float doorSpeed;
    [SerializeField] private bool doorGo;
    [SerializeField] private Vector3 doorTarget;

    private Vector3 doorStartPosition;

    private void Start()
    {
        doorStartPosition = door.transform.position;
    }

    private void OnTriggerEnter(Collider playerCollider)
    {
        if (playerCollider.tag == "Player")
        {
            doorGo = true;
        }
    }

    private void OnTriggerExit(Collider playerCollider)
    {
        if (playerCollider.tag == "Player")
        {
            doorGo = false;
        }
    }

    private void Update()
    {
        doorMovement();
    }

    private void doorMovement()
    {
        if (doorGo)
        {
            door.transform.position = Vector3.MoveTowards(door.transform.position, doorTarget, Time.deltaTime * doorSpeed);
        }
        else
        {
            door.transform.position = Vector3.MoveTowards(door.transform.position, doorStartPosition, Time.deltaTime * doorSpeed);
        }
    }
}
