using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EndLevelTrigger : MonoBehaviour
{
    #region Переменные
    [Header("Timeline, which plays, when player comming to trigger.")]
    [SerializeField] private PlayableDirector timeline;
    #endregion

    #region Методы
    /// <summary>
    /// При вхождении в триггер игрока,
    /// проигрывается таймлайн.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            timeline.Play();
        }
    }
    #endregion
}
