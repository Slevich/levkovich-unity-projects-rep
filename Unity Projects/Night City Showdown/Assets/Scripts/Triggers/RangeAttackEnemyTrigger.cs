using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttackEnemyTrigger : MonoBehaviour
{
    #region Переменные
    //Переменная, содержащая имя родительского объекта.
    private string parentName;
    #endregion

    #region Методы
    /// <summary>
    /// На старте получаем имя родительского объекта.
    /// </summary>
    private void Start()
    {
        parentName = GetComponentInParent<Rigidbody2D>().gameObject.name;
    }

    /// <summary>
    /// При вхождении в триггер в родительском компоненте меняется состояние,
    /// что игрок попал в зону атаки врага.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (parentName.Contains("Cyborg"))
            {
                GetComponentInParent<Cyborg>().playerInRange = true;
            }
            else if (parentName.Contains("Raider"))
            {
                GetComponentInParent<Raider>().playerInRange = true;
            }
        }
    }

    /// <summary>
    /// При вхождении в триггер в родительском компоненте меняется состояние,
    /// что игрок вышел из зоны атаки врага.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (parentName.Contains("Cyborg"))
            {
                GetComponentInParent<Cyborg>().playerInRange = false;
            }
            else if (parentName.Contains("Raider"))
            {
                GetComponentInParent<Raider>().playerInRange = false;
            }
        }
    }
    #endregion
}
