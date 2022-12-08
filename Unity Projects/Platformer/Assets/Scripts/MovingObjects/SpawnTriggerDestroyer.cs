using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTriggerDestroyer : MonoBehaviour
{
    [Header("Trigger where spawn objects")]
    [SerializeField] private GameObject spawnTrigger;

    private void OnTriggerEnter2D(Collider2D objectCollision)
    {
        if (objectCollision.tag == "Player")
        {
            Destroy(spawnTrigger);
        }
    }
}
