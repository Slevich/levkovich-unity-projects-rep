using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCutsceneStopper : MonoBehaviour
{
    //При вхождении противника в катсцене в триггер "Stopper" его состояние меняется и он встает на месте.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Damageable"))
        {
            collision.GetComponent<EnemyCutsceneController>().goForward = false;
            collision.GetComponent<EnemyCutsceneController>().goBackward = false;
        }
    }
}
