using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneSpawner : MonoBehaviour
{
    [Header("GameObject which spawned")]
    [SerializeField] private GameObject stonePrefab;

    [Header("Timer every iteration of which spawned objects")]
    [SerializeField] private float spawnTimer;

    //Таймер для обнуления.
    private float currentSpawnTimer;

    public void SpawnObject()
    {
        Instantiate(stonePrefab, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
    }

    private void Start()
    {
        currentSpawnTimer = spawnTimer;
        SpawnObject();
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer < 0)
        {
            SpawnObject();
            spawnTimer = currentSpawnTimer;
        }
    }
}
