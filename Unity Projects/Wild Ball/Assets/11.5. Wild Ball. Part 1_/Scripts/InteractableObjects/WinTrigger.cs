using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinTrigger : MonoBehaviour
{
    [SerializeField] private ParticleSystem firework1;
    [SerializeField] private ParticleSystem firework2;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject liveScreen;
    [SerializeField] private Rigidbody playerRigidbody;

    private void Start()
    {
        Time.timeScale = 1;
    }

    private void OnTriggerEnter(Collider player)
    {
        if (player.gameObject.tag == "Player")
        {
            playerRigidbody.isKinematic = true;
            firework1.gameObject.SetActive(true);
            firework2.gameObject.SetActive(true);
            StartCoroutine(winTimer());
        }
    }

    private IEnumerator winTimer()
    {
        yield return new WaitForSeconds(4f);
        liveScreen.SetActive(false);
        winScreen.SetActive(true);
        Time.timeScale = 0;
    }
}
