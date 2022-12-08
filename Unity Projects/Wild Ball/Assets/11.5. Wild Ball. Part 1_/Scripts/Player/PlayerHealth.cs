using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private GameObject liveScreen;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject playerParticleSystem;
    [SerializeField] private GameObject visiblePlayerBody;
    [SerializeField] private AudioSource levelMusic;
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private AudioSource hitSound;
    [SerializeField] private AudioSource pickUpSound;

    private float health = 1;

    private void OnCollisionEnter(Collision enemyCollision)
    {
        if (enemyCollision.gameObject.tag == "Enemy")
        {
            health -= 0.25f;
            hitSound.Play();
            healthBar.fillAmount = health;
        }
        else if (enemyCollision.gameObject.tag == "Death")
        {
            death();
        }
    }

    private void OnTriggerEnter(Collider objectCollider)
    {
        if (objectCollider.tag == "Health")
        {
            if (health < 1)
            {
                health += 0.25f;
            }

            healthBar.fillAmount = health;
            pickUpSound.Play();
        }
    }

    private void Update()
    {
        if (health <= 0)
        {
            death();
        }
    }

    private IEnumerator deathTimer()
    {
        yield return new WaitForSeconds(3.2f);

        Time.timeScale = 0;
        liveScreen.SetActive(false);
        deathScreen.SetActive(true);
    }

    private void death()
    {
        levelMusic.Stop();
        playerRigidbody.isKinematic = true;
        visiblePlayerBody.SetActive(false);
        healthBar.fillAmount = 0;

        if (playerParticleSystem)
        {
            playerParticleSystem.SetActive(true);
        }
       
        StartCoroutine(deathTimer());
    }
}
