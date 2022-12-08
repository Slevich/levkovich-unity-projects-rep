using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("Maximum of health, which every character have")]
    [SerializeField] private float maxHealth;

    [Header("Image of the health bar progress which corresponds current level of health")]
    [SerializeField] private Image healthBar;

    //Текущее здоровье персонажа.
    public float currentHealth;

    //Переменная, которая содержит информацию о том, жив ли персонаж или мертв.
    public bool isAlive;

    private void Awake()
    {
        Time.timeScale = 1;
        currentHealth = maxHealth;
        isAlive = true;
    }

    private void Update()
    {
        healthBar.fillAmount = currentHealth / maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        CheckIsAlive();
    }

    public void CheckIsAlive()
    {
        if (currentHealth > 0)
        {
            isAlive = true;
        }
        else
        {
            isAlive = false;
        }
    }
}
