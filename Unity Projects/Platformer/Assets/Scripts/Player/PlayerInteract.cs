using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Power of pushing object")]
    [SerializeField] private float pushingPower;

    private void OnCollisionEnter2D(Collision2D objectCollision)
    {
        if (objectCollision.gameObject.tag == "PushingObjects")
        {
            objectCollision.rigidbody.AddForce(Vector2.right * pushingPower, ForceMode2D.Force);
        }
    }
}
