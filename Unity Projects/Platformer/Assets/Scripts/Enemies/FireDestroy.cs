using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDestroy : MonoBehaviour
{
    [Header("GameObject which need to destroy")]
    [SerializeField] private GameObject enemy;

    private void DestroyEnemy()
    {
        Destroy(enemy);
    }
}
