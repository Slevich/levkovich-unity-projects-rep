using UnityEngine;

namespace Platformer.Inputs
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Vars")]
        [SerializeField, Range(0, 10)] private float speed = 2.0f;
        [SerializeField, Range(0, 10)] public float jumpForce = 1.0f;
        [SerializeField] private bool isGrounded = false;

        [Header("Settings")]
        [SerializeField] private Transform groundColliderTransform;
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private float jumpOffset;
        [SerializeField] private LayerMask groundMask;

        //SpriteRenderer игрока для поворота.
        private SpriteRenderer playerSR;

        //Аниматор игрока для смены анимаций.
        private Animator playerAnim;

        //Rigidbody2D игрока для изменения местоположения. 
        private Rigidbody2D playerRigidbody;

        private void Awake()
        {
            playerRigidbody = GetComponent<Rigidbody2D>();
            playerAnim = GetComponent<Animator>();
            playerSR = GetComponent<SpriteRenderer>();
        }

        private void FixedUpdate()
        {
            Vector3 overlapCirclePosition = groundColliderTransform.position;
            isGrounded = Physics2D.OverlapCircle(overlapCirclePosition, jumpOffset, groundMask);
        }

        public void MoveCharacter(float direction, bool isJumpButtonPressed)
        {
            if (isJumpButtonPressed)
            {
                Jump();
            }

            if (Mathf.Abs(direction) > 0.01f)
            {
                HorizontalMovement(direction);
            }

            if (direction != 0)
            {
                playerAnim.SetInteger("Velocity", 1);

                if (direction < 0)
                {
                    playerSR.flipX = true;
                }
                else if (direction > 0)
                {
                    playerSR.flipX = false;
                }
            }
            else if (direction == 0)
            {
                playerAnim.SetInteger("Velocity", 0);
            }

            if (isGrounded)
            {
                playerAnim.SetBool("IsInAir", false);
                
            }
            else
            {
                playerAnim.SetBool("IsInAir", true);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Damageable") || collision.gameObject.CompareTag("MovingObjects") || collision.gameObject.CompareTag("PushingObjects"))
            {
                playerAnim.SetBool("IsLanding", true);
                playerAnim.SetBool("IsJumping", false);
            }
        }

        public void Jump()
        {
            if (isGrounded)
            {
                playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, jumpForce);
                playerAnim.SetBool("IsJumping", true);
            }
        }

        private void HorizontalMovement(float direction)
        {
            playerRigidbody.velocity = new Vector2(curve.Evaluate(direction) * speed, playerRigidbody.velocity.y);
        }

        private void StopLanding()
        {
            playerAnim.SetBool("IsLanding", false);
        }

        private void StopJumping()
        {
            playerAnim.SetBool("IsJumping", false);
        }

#if UNITY_EDITOR
        [ContextMenu("Reset Values")]
        public void ResetValues()
        {
            speed = 2;
            jumpForce = 5;
        }
#endif
    }
}
