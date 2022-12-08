using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
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
    /// При вхождении игрока в триггер, в компоненте родительского объекта,
    /// в зависимости от его имени переключается состояние - замечен игрок.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (parentName.Contains("Biker"))
            {
                GetComponentInParent<Biker>().playerDetected = true;
            }
            else if (parentName.Contains("Dog"))
            {
                GetComponentInParent<Dog>().playerDetected = true;
            }
            else if (parentName.Contains("BombDrone"))
            {
                GetComponentInParent<BombDrone>().playerDetected = true;
            }
            else if (parentName.Contains("Cyborg"))
            {
                GetComponentInParent<Cyborg>().playerDetected = true;
            }
            else if (parentName.Contains("Raider"))
            {
                GetComponentInParent<Raider>().playerDetected = true;
            }
        }
    }

    /// <summary>
    /// При выходе игрока из триггера, в компоненте родительского объекта,
    /// в зависимости от его имени переключается состояние - игрок вышел из зоны.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (parentName.Contains("Biker"))
            {
                GetComponentInParent<Biker>().playerDetected = false;
            }
            else if (parentName.Contains("Dog"))
            {
                GetComponentInParent<Dog>().playerDetected = false;
            }
            else if (parentName.Contains("BombDrone"))
            {
                GetComponentInParent<BombDrone>().playerDetected = false;
            }
            else if (parentName.Contains("Cyborg"))
            {
                GetComponentInParent<Cyborg>().playerDetected = false;
            }
            else if (parentName.Contains("Raider"))
            {
                GetComponentInParent<Raider>().playerDetected = false;
            }
        }
    }
    #endregion
}
