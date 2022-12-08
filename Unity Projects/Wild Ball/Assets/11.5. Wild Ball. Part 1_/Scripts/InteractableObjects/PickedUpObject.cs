using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickedUpObject : MonoBehaviour
{
    [SerializeField] private GameObject objectParticleSystem;
    [SerializeField] private GameObject animatedObject;
    [SerializeField] private GameObject objectBody;
    [SerializeField] private Collider objectCollider;

    private bool objectParticleSystemAlive = true;

    private void OnTriggerEnter(Collider playerCollider)
    {
        if (playerCollider.tag == "Player")
        {
            objectCollider.enabled = false;
            objectParticleSystem.SetActive(true);
            objectParticleSystemAlive = false;
            transform.localScale = new Vector3(0, 0, 0);
            StartCoroutine(destroyTimer());
        }
    }

    private void Update()
    {
        effectMovement();
    }

    private IEnumerator destroyTimer()
    {
        yield return new WaitForSeconds(1);

        objectBody.SetActive(false);
    }

    private void effectMovement()
    {
        if (objectParticleSystemAlive)
        {
            objectParticleSystem.transform.position = animatedObject.transform.position;
        }
    }
}
