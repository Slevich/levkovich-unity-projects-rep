using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractButtonOpen : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private float doorSpeed;
    [SerializeField] private GameObject interactHUD;
    [SerializeField] private bool doorGo;
    [SerializeField] private Vector3 doorTarget;

    private void OnTriggerEnter(Collider playerCollider)
    {
        if (playerCollider.tag == "Player")
        {
            interactHUD.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider playerCollider)
    {
        if (playerCollider.tag == "Player")
        {
            if (Input.GetKey(KeyCode.E))
            {
                doorGo = true;
            }
        }
    }

    private void OnTriggerExit(Collider playerCollider)
    {
        if (playerCollider.tag == "Player")
        {
            interactHUD.SetActive(false);
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

        if (door.transform.position == doorTarget)
        {
            doorGo = false;
        }
    }
}
