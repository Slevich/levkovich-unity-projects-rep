using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Inputs
{

    public class Cart : MonoBehaviour
    {
        [Header("Rigidbody2D of the Player")]
        [SerializeField] private Rigidbody2D playerRB;

        [Header("Transform of the Player")]
        [SerializeField] private Transform playerTransform;

        //PointEffector2D которым притягивается игрок к телеге.
        private PointEffector2D cartPE;

        //Переменная bool для отслеживания в телеге ли игрок.
        private bool isPlayerInCart;

        //Переменная bool для отслеживания вышел ли игрок из телеги.
        private bool isPlayerOutCart;

        //Вращение по Z-оси телеги.
        private float cartZRotation;

        //Изменение вращения телеги.
        private float cartRotationDifference;

        //Изменение вращения игрока.
        private float playerRotationDifference;

        private void Start()
        {
            cartPE = GetComponent<PointEffector2D>();
        }

        private void Update()
        {
            cartZRotation = gameObject.transform.rotation.z;
            cartRotationDifference = playerTransform.rotation.z - cartZRotation;

            if (cartRotationDifference > 0)
            {
                cartRotationDifference *= -1;
            }

            playerRotationDifference = 0 - playerTransform.rotation.z;

            if (playerRotationDifference < 0)
            {
                playerRotationDifference *= -1;
            }

            if (isPlayerInCart)
            {
                playerTransform.Rotate(0, 0, cartRotationDifference);
            }
            else if (isPlayerOutCart)
            {
                playerTransform.Rotate(0, 0, playerRotationDifference);
            }
        }

        private void OnTriggerEnter2D(Collider2D playerCollision)
        {
            if (playerCollision.tag == "Player")
            {
                playerCollision.GetComponent<PlayerMovement>().jumpForce = 15;
                cartPE.forceMagnitude = -100;
                isPlayerInCart = true;
            }
        }

        private void OnTriggerExit2D(Collider2D playerCollision)
        {
            if (playerCollision.tag == "Player")
            {
                playerCollision.GetComponent<PlayerMovement>().jumpForce = 7;
                isPlayerInCart = false;
                isPlayerOutCart = true;
            }
        }

        private void OnTriggerStay2D(Collider2D playerCollision)
        {
            if (playerCollision.tag == "Player")
            {
                if (Input.GetButton(GlobalStringVars.JUMP_BUTTON))
                {
                    cartPE.forceMagnitude = 0;
                }
            }
        }
    }
}
