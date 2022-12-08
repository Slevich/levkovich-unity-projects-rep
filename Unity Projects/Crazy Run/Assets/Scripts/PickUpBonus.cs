using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpBonus : MonoBehaviour
{
    [Header("String name of bonus: Usual or Extra")]
    [SerializeField] private string typeOfBonus;

    //При вхождении в триггер, в зависимости от типа бонуса, он в статистике приплюсовывается к тому или иному показателю по бонусам. Также проигрывается звук поднятия.
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (typeOfBonus == "Usual")
            {
                collider.GetComponent<PlayerStatistic>().numberOfBonuses += 1;
                collider.GetComponentInChildren<AudioSource>().Play();
            }
            else if (typeOfBonus == "Extra")
            {
                collider.GetComponent<PlayerStatistic>().numberOfExtraBonuses += 1;
                collider.GetComponentInChildren<AudioSource>().Play();
            }
            Destroy(gameObject);
        }
    }
}
