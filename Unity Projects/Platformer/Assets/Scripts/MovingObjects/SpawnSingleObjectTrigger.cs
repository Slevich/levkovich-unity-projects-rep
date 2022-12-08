using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSingleObjectTrigger : MonoBehaviour
{
    [Header("GameObject which spawned")]
    [SerializeField] private GameObject objectPrefab;

    [Header("Place on scene where GameObject is spawned")]
    [SerializeField] private Vector3 spawnPlace;

    //Переменная bool для проверки заспаунен ли объект.
    private bool spawn;

    private void Awake()
    {
        spawn = true;
    }

    public void SpawnObject()
    {
        Instantiate(objectPrefab, spawnPlace, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D playerCollision)
    {
        if (playerCollision.tag == "Player")
        {
            if (spawn)
            {
                SpawnObject();
                spawn = false;
            }
        }
    }
}
