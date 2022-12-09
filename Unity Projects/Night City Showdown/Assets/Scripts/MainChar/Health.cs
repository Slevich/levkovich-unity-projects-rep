using UnityEngine;

public class Health : MonoBehaviour
{
    #region Переменные
    [Header("Current health's amount of character.")]
    [SerializeField] private float currentHealth;
    [Header("Maximal health's amount of character.")]
    [SerializeField] private float maxHealth;

    public bool IsAlive => currentHealth > 0;

    #endregion

    #region Методы
    /// <summary>
    /// На старте, значение, что первонаж жив - истинно.
    /// Текущее здоровье становится равным максимальному.
    /// </summary>
    private void Start()
    {
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
    /// Метод прибавляет к текущему здоровью определенное значение.
    /// Если при прибавлении текущее здоровье больше максимального - прибавляется их разница.
    /// Если нет, прибавляется указанное в параметре значение.
    /// </summary>
    /// <param name="healAmount"></param>
    public void ToHeal(float healAmount)
    {
        if ((currentHealth + healAmount) > maxHealth) currentHealth += (maxHealth - currentHealth);
        else currentHealth += healAmount;
    }

    /// <summary>
    /// Метод вычисляет текущий процент здоровья.
    /// </summary>
    /// <returns></returns>
    public float GetCurrentHealthProcent()
    {
        float healthProcent = currentHealth / maxHealth;
        return healthProcent;
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
    #endregion
}
