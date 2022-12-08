using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    [Header("Damage which take to player")]
    [SerializeField] private float damage;

    [Header("GameObject with animation of the destruction of stone")]
    [SerializeField] private GameObject destroyEffect;

    public void SpawnDestroyEffect()
    {
        Instantiate(destroyEffect, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
    }

    private void OnCollisionEnter2D(Collision2D playerCollision)
    {
        if (playerCollision.gameObject.tag == "Player")
        {
            playerCollision.gameObject.GetComponent<Health>().TakeDamage(damage);
            Destroy(gameObject);
            SpawnDestroyEffect();
        }
    }
}
