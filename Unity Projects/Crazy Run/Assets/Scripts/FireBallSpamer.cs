using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallSpamer : MonoBehaviour
{
    [Header("GameObject which spawned")]
    [SerializeField] private GameObject fireBallPrefab;

    [Header("Timer every iteration of which spawned objects")]
    [SerializeField] private float spawnTimer;

    [Header("Direction of FireBall movement")]
    [SerializeField] private Vector3 fireBallTriggerDirection;
    [Header("Speed of FireBall movement")]
    [SerializeField] private float fireBallSpeed;

    //Таймер спавна для обнуления.
    private float currentSpawnTimer;

    //Метод по спавну огненных шаров внутри триггера.
    public void SpawnObject()
    {
        Instantiate(fireBallPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
    }

    //На старте присваеваем таймеру для обнуления значение таймера спавна огненных шаров.
    private void Start()
    {
        currentSpawnTimer = spawnTimer;
    }

    //Запускаем таймер, когда он заканчивается, спавним огненный шар и обнуляем таймер.
    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer < 0)
        {
            SpawnObject();
            spawnTimer = currentSpawnTimer;
        }
    }

    //Передаем значения направления движения и скорости огненному шару.
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("FireBall"))
        {
            collider.GetComponent<FireBall>().fireBallDirection = fireBallTriggerDirection;
            collider.GetComponent<FireBall>().speed = fireBallSpeed;
        }
    }
}
