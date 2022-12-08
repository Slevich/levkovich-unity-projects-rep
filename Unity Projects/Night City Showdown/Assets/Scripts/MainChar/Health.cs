using UnityEngine;

public class Health : MonoBehaviour
{
    #region Переменные
    [Header("Current health's amount of character.")]
    public float currentHealth;
    [Header("Maximal health's amount of character.")]
    public float maxHealth;
    public bool isAlive = true;
    #endregion

    #region Методы
    /// <summary>
    /// На старте, значение, что первонаж жив - истинно.
    /// Текущее здоровье становится равным максимальному.
    /// </summary>
    private void Start()
    {
        isAlive = true;
        currentHealth = maxHealth;
    }

    /// <summary>
    /// При измении параметров в инспекторе, отслеживаем,
    /// чтобы текущее здоровье не было больше максимального, или меньше нуля.
    /// Также отслеживаем, чтобы максимальное не было меньше текущего или нуля.
    /// </summary>
    private void OnValidate()
    {
        if (currentHealth < 0) currentHealth = 0;
        else if (currentHealth > maxHealth) currentHealth = maxHealth;
        else if (maxHealth < currentHealth) maxHealth = currentHealth;
        else if (maxHealth < 0) maxHealth = currentHealth;
    }

    /// <summary>
    /// В Update проверяем жив ли персонаж.
    /// </summary>
    private void Update()
    {
        CheckIsAlive();
    }

    /// <summary>
    /// Метод наносит текущему здоровью урон
    /// в установленном размере.
    /// </summary>
    /// <param name="damage"></param>
    public void ToDamage(float damage)
    {
        currentHealth -= damage;
    }

    /// <summary>
    /// Метод проверяет, исходя из того
    /// меньше ли или равно ли нулю текущее здоровье.
    /// Если да - персонаж мертв.
    /// </summary>
    private void CheckIsAlive()
    {
        if (currentHealth <= 0)
        {
            isAlive = false;
        }
        else
        {
            isAlive = true;
        }
    }
    #endregion
}
