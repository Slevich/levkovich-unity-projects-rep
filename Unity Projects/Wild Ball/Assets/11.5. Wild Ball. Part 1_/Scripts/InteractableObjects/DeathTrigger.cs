using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    [SerializeField] private GameObject liveScreen;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject playerParticleSystem;
    [SerializeField] private GameObject visiblePlayerBody;
    [SerializeField] private AudioSource levelMusic;
    [SerializeField] private Rigidbody playerRigidbody;

    private void OnCollisionEnter(Collision playerCollision)
    {
        if (playerCollision.gameObject.tag == "Player")
        {
            levelMusic.Stop();
            playerRigidbody.isKinematic = true;
            playerParticleSystem.SetActive(true);
            visiblePlayerBody.SetActive(false);

            StartCoroutine(deathTimer());
        }
    }

    private IEnumerator deathTimer()
    {
        yield return new WaitForSeconds(3.2f);

        Time.timeScale = 0;
        liveScreen.SetActive(false);
        deathScreen.SetActive(true);
    }
}
