using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesAnimations : MonoBehaviour
{
    [Header("Gameobject with animation of enemy's destroy")]
    [SerializeField] private GameObject destroyFX;

    [Header("Health Bar of the enemy")]
    [SerializeField] private GameObject enemyHealthBar;

    [Header("Audio Source which play Audio Clip of enemy's death")]
    [SerializeField] private AudioSource enemyDeathSound;

    //Компонент врага со скриптом "Health".
    private Health enemyHealth;

    //Sprite Renderer врага для его отключения при смерти.
    private SpriteRenderer enemySR;

    //Animator врага для проигрывания анимции смерти.
    private Animator enemyAnim;

    private void Awake()
    {
        enemyAnim = GetComponent<Animator>();
        enemySR = GetComponent<SpriteRenderer>();
        enemyHealth = GetComponentInParent<Health>();
    }

    private void Update()
    {
        if (enemyHealth.isAlive == false)
        {
            enemyAnim.SetBool("IsDead", true);
        }
    }

    private void EnemyDeath()
    {
        destroyFX.SetActive(true);
        enemyDeathSound.Play();
        enemySR.enabled = false;
        enemyHealthBar.SetActive(false);
    }
}
