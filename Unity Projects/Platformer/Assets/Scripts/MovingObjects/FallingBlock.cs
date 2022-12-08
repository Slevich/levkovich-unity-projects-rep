using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBlock : MonoBehaviour
{
    [Header("Damage which will be take to player at collision")]
    [SerializeField] private float damage;

    [Header("GameObject with animation of destruction of the block")]
    [SerializeField] private GameObject destroyEffect;

    //Rigidbody2D блока для манипуляции с гравитацией.
    private Rigidbody2D blockRB;

    //Аниматор блока для проигрывания анимаций.
    public Animator blockAnim;

    //Переменная bool для отслеживания, падает ли блок или нет.
    public bool fall;

    private void Awake()
    {
        blockRB = GetComponent<Rigidbody2D>();
        blockAnim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (fall)
        {
            blockRB.gravityScale = 0.5f;
        }
    }

    private void OnCollisionEnter2D(Collision2D characterCollision)
    {
        if (characterCollision.gameObject.tag == "Player")
        {
            characterCollision.gameObject.GetComponent<Health>().TakeDamage(damage);
            destroyEffect.SetActive(true);
        }
        else if (characterCollision.gameObject.tag == "Ground")
        {
            destroyEffect.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D characterCollision)
    {
        if (characterCollision.tag == "Player")
        {
            blockAnim.SetBool("Go", true);
        }
    }
}
