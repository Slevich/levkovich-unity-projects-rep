using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttackEnemyCheck : MonoBehaviour
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
    /// что коллайдер земли попал в триггер.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            if (parentName.Contains("Cyborg"))
            {
                GetComponentInParent<Cyborg>().wallDetected = true;
            }
            else if (parentName.Contains("Raider"))
            {
                GetComponentInParent<Raider>().wallDetected = true;
            }
        }
    }

    /// <summary>
    /// При вхождении в триггер в родительском компоненте меняется состояние,
    /// что коллайдер земли покинул триггер.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            if (parentName.Contains("Cyborg"))
            {
                GetComponentInParent<Cyborg>().wallDetected = false;
            }
            else if (parentName.Contains("Raider"))
            {
                GetComponentInParent<Raider>().wallDetected = false;
            }
        }
    }
    #endregion
}
