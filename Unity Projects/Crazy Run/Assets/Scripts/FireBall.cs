using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [Header("VFX Prefab of FireBall Explosion")]
    [SerializeField] private GameObject explosionPrefab;
    [Header("Speed of FireBall movement")]
    public float speed;
    [Header("Direction of FireBall movement")]
    public Vector3 fireBallDirection;

    //Переменная, обозначающая движение огненного шара.
    private bool Go = true;
    //Ригидбоди огненного шара, для приложения к нему силы.
    private Rigidbody fireBallRB;

    //На старте получаем компонент ригидбоди.
    private void Start()
    {
        fireBallRB = GetComponent<Rigidbody>();
    }
    
    //Применяем силу к ригидбади огненного шара в определенном направлении при вылете из триггера.
    private void Update()
    {
        if (Go)
        {
            fireBallRB.AddForce(fireBallDirection * speed, ForceMode.Acceleration);
        }
    }

    //При столкновении с игроком, шар спавнит VFX взрыва, вызывает метод проигрыша у игрока, а затем самоуничтожается. При столкновении с границей то же самое без проигрыша игрока.
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            Instantiate(explosionPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            collider.GetComponent<LoseWin>().YouLose();
            Destroy(gameObject);
        }
        else if (collider.CompareTag("Border"))
        {
            Instantiate(explosionPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
