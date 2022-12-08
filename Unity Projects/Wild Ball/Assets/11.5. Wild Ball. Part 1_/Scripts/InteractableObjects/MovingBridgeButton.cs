using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBridgeButton : MonoBehaviour
{
    [SerializeField] private GameObject[] planes = new GameObject[6];
    [SerializeField] private Vector3[] planesTargets = new Vector3[6];
    [SerializeField] private float planeSpeed;
    [SerializeField] private GameObject interactHUD;
    [SerializeField] private bool planeGo;

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
                planeGo = true;
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
        planeMovement();
    }

    private void planeMovement()
    {
        if (planeGo)
        {
            for (int n = 0; n <= 5; n++)
            {
                planes[n].transform.position = Vector3.MoveTowards(planes[n].transform.position, planesTargets[n], Time.deltaTime * planeSpeed);
            }
        }
    }
}
