using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextLevelDoor : MonoBehaviour
{
    #region Переменные
    [Header("Game Object prefab, which contains destroy VFX.")]
    [SerializeField] private GameObject destroyVFX;
    [Header("Image which show current HP level.")]
    [SerializeField] private Image HPBar;

    //Переменная, содержащая референс на компонент здоровья.
    private Health doorsHealth;
    #endregion

    #region Методы
    /// <summary>
    /// На старте получаем компонент здоровья.
    /// </summary>
    private void Start()
    {
        doorsHealth = GetComponent<Health>();
    }

    /// <summary>
    /// В Update обновляем уровень здоровья двери.
    /// Если дверь "умерла", то спавним префаб с эффектом уничтожения.
    /// После чего уничтожаем дверь.
    /// </summary>
    private void Update()
    {
        UpdateDoorHPLevel();

        if (doorsHealth.IsAlive == false)
        {
            GameObject explosion = Instantiate(destroyVFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Расчитываем уровень здоровья двери.
    /// Передаем его в Image c уровнем здоровья.
    /// </summary>
    private void UpdateDoorHPLevel()
    {
        float doorHP = doorsHealth.GetCurrentHealthProcent();
        HPBar.fillAmount = doorHP;
    }
    #endregion
}
