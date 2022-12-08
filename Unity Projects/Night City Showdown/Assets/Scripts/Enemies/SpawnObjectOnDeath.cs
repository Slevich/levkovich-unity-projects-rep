using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectOnDeath : MonoBehaviour
{
    #region Переменные
    [Header("Variable switch, which means enemy spawn something on his death.")]
    [SerializeField] private bool isSpawner;
    [Header("Prefab of game object, which spawned on enemies death.")]
    [SerializeField] private GameObject objectPrefab;
    #endregion

    #region Методы
    /// <summary>
    /// Метод спавнит префаб объекта.
    /// </summary>
    public void SpawnObject()
    {
        if (isSpawner && objectPrefab != null)
        {
            GameObject spawnedObject = Instantiate(objectPrefab, transform.position, Quaternion.identity);
        }
    }
    #endregion
}
