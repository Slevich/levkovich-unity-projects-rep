using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTriggers : MonoBehaviour
{
    [Header("Damage which take to player")]
    [SerializeField] private float damage;

    private void OnTriggerEnter2D(Collider2D playerCollision)
    {
        if (playerCollision.CompareTag("Player"))
        {
            playerCollision.GetComponent<Health>().TakeDamage(damage);
        }
    }
}
