using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float smooth;
    [SerializeField] private float yOffset;

    void FixedUpdate()
    {
       transform.position = Vector3.Lerp(transform.position, new Vector3(playerTransform.position.x, playerTransform.position.y + yOffset, transform.position.z), Time.deltaTime * smooth);
    }
}
