using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTrigger : MonoBehaviour
{
    //Триггер, который уничтожает входящие в него движущиеся объекты.
    private void OnTriggerEnter2D(Collider2D objectCollision)
    {
        if (objectCollision.tag == "MovingObjects")
        {
            Destroy(objectCollision.gameObject);
        }
    }
}
