using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WildBall.Inputs
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField, Range(0, 10)] private float speed = 2.0f;
        [SerializeField, Range(0, 10)] private float jumpForce = 1.0f;
        [SerializeField] private AudioSource jumpSound;

        private Animator playerAnim;
        private bool jumping;
        private Rigidbody playerRigidbody;

        private void Awake()
        {
            Time.timeScale = 1;
            playerRigidbody = GetComponent<Rigidbody>();
            playerAnim = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            float horizontal = Input.GetAxis(GlobalStringVars.HORIZONTAL_AXIS);

            if (horizontal > 0)
            {
                playerAnim.SetInteger("Velocity", 1);
            }
            else if (horizontal < 0)
            {
                playerAnim.SetInteger("Velocity", -1);
            }
            else if (horizontal == 0)
            {
                playerAnim.SetInteger("Velocity", 0);
            }
        }

        public void MoveCharacter(Vector2 movement)
        {
            playerRigidbody.AddForce(movement * speed);
        }

        public void Jump (Vector2 movement)
        {
            if (Input.GetButton(GlobalStringVars.JUMP_BUTTON))
            {
                if (jumping)
                {
                    playerAnim.SetBool("Jump?", true);
                    playerRigidbody.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
                    jumpSound.Play();
                }
            }
        }

        private void OnCollisionEnter(Collision groundCollision)
        {
            if(groundCollision.gameObject.tag == "Ground" || groundCollision.gameObject.tag == "Enemy")
            {
                jumping = true;
            }
        }

        private void OnCollisionExit(Collision groundCollision)
        {
            if(groundCollision.gameObject.tag == "Ground")
            {
                jumping = false;
                playerAnim.SetBool("Jump?", false);
            }
        }

        private void OnCollisionStay(Collision groundCollision)
        {
            if (groundCollision.gameObject.tag == "Ground")
            {
                jumping = true;
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Reset Values")]
        public void ResetValues()
        {
            speed = 2;
        }
#endif
    }
}
