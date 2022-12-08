using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    //Если игрок попадает к триггер ловушки - вызывается метод его проигрыша.
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            collider.GetComponent<LoseWin>().YouLose();
        }
    }
}
