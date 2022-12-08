using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Player Transform")]
    [SerializeField] private Transform playerTransform;

    [Header("Smooth of the camera action")]
    [SerializeField] private float smooth;

    [Header("Camera distance of Y-axis")]
    [SerializeField] private float yOffset;

    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(playerTransform.position.x, playerTransform.position.y + yOffset, transform.position.z), Time.deltaTime * smooth);
    }
}