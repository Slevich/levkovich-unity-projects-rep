using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTimeline : MonoBehaviour
{
    #region Методы
    /// <summary>
    /// Метод дезактивирует конкретный игровой объект,
    /// переданный в метод в качестве параметра.
    /// </summary>
    /// <param Объект для дезактивации="disactivatedGameObject"></param>
    public void DisactivateGameObject(GameObject disactivatedGameObject)
    {
        disactivatedGameObject.SetActive(false);
    }
    #endregion
}
