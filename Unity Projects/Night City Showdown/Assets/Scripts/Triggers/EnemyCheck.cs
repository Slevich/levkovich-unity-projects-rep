using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCheck : MonoBehaviour
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
    /// При вхождении в триггер в родительском компоненте меняется состояние.
    /// В зависимости от тега объекта - это означает или то, что объект
    /// контактирует с коллайдером земли, или игрок попал в зону атаки.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            GetComponentInParent<EnemyMovement>().WallDetected = true;
        }
    }

    /// <summary>
    /// При выходе из триггера в родительском компоненте меняется состояние.
    /// В зависимости от тега объекта - это означает или то, что объект
    /// перестал контактировать с коллайдером земли, или игрок вышел из зоны атаки.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            GetComponentInParent<EnemyMovement>().WallDetected = false;
        }
    }
    #endregion
}
