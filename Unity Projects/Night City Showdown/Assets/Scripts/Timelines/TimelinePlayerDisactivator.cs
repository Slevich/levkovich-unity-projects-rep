﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Inputs
{
    public class TimelinePlayerDisactivator : MonoBehaviour
    {
        #region Переменные
        [Header("Main character Game Object ('Joy').")]
        [SerializeField] private GameObject player;
        #endregion
    
        #region Методы
        /// <summary>
    /// Метод переключает переменные из компонентов игрока,
    /// чтобы лишить его возможности принимать инпут и исопользовать оружие.
    /// При этом он переводится в айдловое положение.
    /// </summary>
        public void DisablePlayerParams()
        {
            player.GetComponent<MainCharInput>().inputIsActive = false;
            player.GetComponent<MainCharInput>().horizontalDirection = 0;
            player.GetComponent<MainCharWeapons>().canAim = false;
            player.GetComponent<Animator>().SetBool("IsJumping", false);
            player.GetComponent<Animator>().SetBool("IsDoubleJumping", false);
            player.GetComponent<Animator>().SetFloat("Speed", 0f);
        }
    
        /// <summary>
    /// Метод переключает переменные из компонентов игрока,
    /// чтобы вернуть ему возможность принимать инпут и пользоваться оружием.
    /// </summary>
        public void EnablePlayerParams()
        {
            player.GetComponent<MainCharInput>().inputIsActive = true;
            player.GetComponent<MainCharWeapons>().canAim = true;
        }
        #endregion
    }
    
}
