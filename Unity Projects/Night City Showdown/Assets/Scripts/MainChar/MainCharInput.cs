using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Inputs
{
    public class MainCharInput : MonoBehaviour
    {
        #region Переменные
        //Это публичные переменные, обозначающие
        //нажата ли та или иная клавиша.
        //Скрыты, чтобы не перегружать инспектор.
        [HideInInspector]
        public float horizontalDirection;
        [HideInInspector]
        public bool inputIsActive;
        [HideInInspector]
        public bool jumpButtonPressed;
        [HideInInspector]
        public bool number1Pressed;
        [HideInInspector]
        public bool number2Pressed;
        [HideInInspector]
        public bool number3Pressed;
        [HideInInspector]
        public bool fireButtonPressed;
        [HideInInspector]
        public bool punchButtonPressed;
        [HideInInspector]
        public bool reloadButtonPressed;
        [HideInInspector]
        public bool escapeButtonPressed;
        
        //Переменная, хранящая референс на компонент,
        //управляющий оружием игрока.
        private MainCharWeapons playerWeapons;
        #endregion

        #region Методы
        /// <summary>
        /// На старте получаем компонент.
        /// </summary>
        private void Start()
        {
            playerWeapons = GetComponent<MainCharWeapons>();
            inputIsActive = true;
        }

        /// <summary>
        /// В Update вызываем методы по отслеживанию нажатий.
        /// </summary>
        private void Update()
        {
            Debug.Log(inputIsActive);

            if (inputIsActive)
            {
                horizontalDirection = Input.GetAxis(GlobalStringVars.HORIZONTAL_AXIS);
                JumpButtonPressed();
                NumberButtonsPressed();
                LeftMouseButtonPressed();
                RightMouseButtonPressed();
                ReloadButtonPressed();
                EscapeButtonPressed();
            }
        }

        /// <summary>
        /// Метод отслеживает и передает в переменную, нажата ли клавиша прыжка (пробел).
        /// </summary>
        private void JumpButtonPressed()
        {
            if (Input.GetButtonDown(GlobalStringVars.JUMP_BUTTON))
            {
                jumpButtonPressed = true;
            }
        }

        /// <summary>
        /// Метод отслеживает и передает в переменную, нажати ли клавиши 1,2,3.
        /// </summary>
        private void NumberButtonsPressed()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                number1Pressed = true;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                number2Pressed = true;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                number3Pressed = true;
            }
        }

        /// <summary>
        /// Метод отслеживает и передает в переменную, нажата или отжата левая кнопка мыши.
        /// </summary>
        private void LeftMouseButtonPressed()
        {
            if (Input.GetButtonDown(GlobalStringVars.FIRE_BUTTON))
            {
                fireButtonPressed = true;
            }
            else if (Input.GetButtonUp(GlobalStringVars.FIRE_BUTTON))
            {
                fireButtonPressed = false;
            }
        }

        /// <summary>
        /// Метод отслеживает и передает в переменную, нажата или отжата правая кнопка мыши.
        /// </summary>
        private void RightMouseButtonPressed()
        {
            if (Input.GetButtonDown(GlobalStringVars.PUNCH_BUTTON))
            {
                punchButtonPressed = true;
            }
            else if (Input.GetButtonUp(GlobalStringVars.PUNCH_BUTTON))
            {
                punchButtonPressed = false;
            }
        }

        /// <summary>
        /// Метод отслеживает и передает в переменную, 
        /// нажата ли клавиша перезарядки (R) и говорит, 
        /// что перезарядка начата.
        /// </summary>
        private void ReloadButtonPressed()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                playerWeapons.isReloading = true;
                reloadButtonPressed = true;
            }
        }

        /// <summary>
        /// Метод отслеживает и передает в переменную, нажата ли клавиша Escape.
        /// </summary>
        private void EscapeButtonPressed()
        {
            if (Input.GetButtonDown(GlobalStringVars.ESCAPE_BUTTON))
            {
                escapeButtonPressed = true;
            }
            else if (Input.GetButtonUp(GlobalStringVars.ESCAPE_BUTTON))
            {
                escapeButtonPressed = false;
            }
        }
        #endregion
    }
}
