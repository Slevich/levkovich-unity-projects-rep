using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelAnimation : MonoBehaviour
{
    #region Переменные
    [Header("String name of bool variable in Animator")]
    [SerializeField] private string animBoolName;
    
    //Аниматор объекта.
    private Animator objectAnim;
    #endregion

    #region Методы
    /// <summary>
    /// На старте получаем аниматор.
    /// </summary>
    private void Start()
    {
        objectAnim = GetComponent<Animator>();
    }

    /// <summary>
    /// Метод передает значение в аниматоре.
    /// </summary>
    public void ToCancelAnimation()
    {
        objectAnim.SetBool(animBoolName, false);
    }
    #endregion
}
