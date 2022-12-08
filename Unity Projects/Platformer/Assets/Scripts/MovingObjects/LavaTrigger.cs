using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaTrigger : MonoBehaviour
{
    [Header("Timer every iteration of which damage take to player")]
    [SerializeField] private float damageTimer;

    [Header("Damage which take to player")]
    [SerializeField] private float damage;

    [Header("Player's GameObject")]
    [SerializeField] private GameObject player;

    //Таймер нанесения урона для обнуления.
    private float currentDamageTimer;
    
    //Переменная bool для отслеживания в лаве игрок или нет.
    private bool isInLava;

    //Переменная bool для отслеживания нанесен ли игроку урон в эту итерацию.
    private bool isDamaged;

    private void Awake()
    {
        currentDamageTimer = damageTimer;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Damageable"))
        {
            isInLava = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Damageable"))
        {
            isInLava = false;
            damageTimer = currentDamageTimer;
        }
    }

    private void Update()
    {
        if (isInLava)
        {
            damageTimer -= Time.deltaTime;

            if (damageTimer <= 0)
            {
                isDamaged = false;

                if (isDamaged == false)
                {
                    player.GetComponent<Health>().TakeDamage(damage);
                    damageTimer = currentDamageTimer;
                    isDamaged = true;
                }
            }
        }
    }
}
