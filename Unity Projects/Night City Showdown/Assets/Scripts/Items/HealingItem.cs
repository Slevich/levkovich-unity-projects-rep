using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingItem : MonoBehaviour
{
    #region Переменные
    [Header("Number of health points, on which players's health increase.")]
    [SerializeField] private float HPplus;
    #endregion

    #region Методы
    /// <summary>
    /// При вхождении в триггер, к текущему здоровью игрока
    /// прибавляется значение его роста за подбор.
    /// Если здоровье на маскимуме - не прибавляется ничего.
    /// Если здоровье не на масимуме - прибавляется количество,
    /// в зависимости от того, при росте здоровья, больше ли оно максимального.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            float _playerMaxHealth = collision.GetComponent<Health>().maxHealth;
            float _playerCurrentHealth = collision.GetComponent<Health>().currentHealth;

            if (_playerCurrentHealth < _playerMaxHealth)
            {
                if ((_playerCurrentHealth + HPplus) > _playerMaxHealth) collision.GetComponent<Health>().currentHealth += (_playerMaxHealth - _playerCurrentHealth);
                else collision.GetComponent<Health>().currentHealth += HPplus;
            }
            else collision.GetComponent<Health>().currentHealth += 0;

            collision.GetComponent<MainCharSounds>().PlaySyringePickingUpSounds();
            Destroy(gameObject);
        }
    }
    #endregion
}
