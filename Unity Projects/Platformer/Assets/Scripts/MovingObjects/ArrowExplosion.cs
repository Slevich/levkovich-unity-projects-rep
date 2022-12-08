using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowExplosion : MonoBehaviour
{
    [Header("GameObject with the explosion effect")]
    [SerializeField] private GameObject explosion;

    [Header("GameObject - chain on the wooden bridge")]
    [SerializeField] private GameObject chain;

    [Header("Place in the scene where spowned explosion")]
    [SerializeField] private Vector3 explosionPlace;

    [Header("Timer of destroy GameObjects")]
    [SerializeField] private float explosionTimer;

    //Переменная bool для отслеживания состояния заспаунен ли взрыв.
    private bool isExplosionActivated;

    //Переменная bool для отслеживания состояния долетела за стрела до грибов.
    private bool isArrowOnPosition;

    //Игровой объект, который спаунится в месте соприкосновения стрелы и грибов.
    private GameObject spawnedObject;

    private void SpawnExplosion()
    {
        Instantiate(explosion, explosionPlace, Quaternion.identity);
        spawnedObject = GameObject.Find("Explosion(Clone)");
    }

    private void Update()
    {
        if (isExplosionActivated)
        {
            explosionTimer -= Time.deltaTime;

            if (explosionTimer < 0)
            {
                Destroy(gameObject);
                Destroy(chain);
                Destroy(spawnedObject);
            }
        }
        else if (isArrowOnPosition)
        {
            SpawnExplosion();
            isExplosionActivated = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D mushroomCollision)
    {
        if (mushroomCollision.gameObject.tag == "MovingObjects")
        {
            isArrowOnPosition = true;
        }
    }

}
