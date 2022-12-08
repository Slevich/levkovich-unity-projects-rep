using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("GameObject with player's Health Bar")]
    [SerializeField] private GameObject playersHealhBar;

    [Header("Death image on LevelHUD")]
    [SerializeField] private GameObject deathImage;

    [Header("Music playing at the level")]
    [SerializeField] private AudioSource levelMusic;

    //Компонент игрока со скриптом "Health" для опредления живой он или нет.
    private Health health;

    //Аниматор игрока для смены анимаций.
    private Animator playerAnim;

    private void Awake()
    {
        health = GetComponent<Health>();
        playerAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (health.isAlive == false)
        {
            playerAnim.SetBool("IsDead", true);
        }
    }

    private void PlayersDeath()
    {
        playersHealhBar.SetActive(false);
        deathImage.SetActive(true);
        levelMusic.Stop();
        Time.timeScale = 0;
    }
}
