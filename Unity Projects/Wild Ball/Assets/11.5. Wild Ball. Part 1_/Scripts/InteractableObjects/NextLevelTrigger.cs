using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelTrigger : MonoBehaviour
{
    [SerializeField] private ParticleSystem firework1;
    [SerializeField] private ParticleSystem firework2;

    private void OnTriggerEnter(Collider player)
    {
        if (player.gameObject.tag == "Player")
        {
            if (SceneManager.GetActiveScene().buildIndex < 6)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else if (SceneManager.GetActiveScene().buildIndex == 6)
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
