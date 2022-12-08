using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Input Component")]
    [SerializeField] private PlayerInput playerInput;

    [Header("Speed of player's movement")]
    [SerializeField] private float movementSpeed;

    [Header("Force of player's jump")]
    [SerializeField] private float jumpForce;

    [Header("Impulse power of collison with border")]
    [SerializeField] private float collisionImpulsePower;

    //Ригидбади игрока.
    private Rigidbody playerRB;

    //Переменные для отслеживания движения игрока в определенную сторону, а также двигается ли он в целом.
    public bool movingRight;
    public bool movingLeft;
    public bool movingUp;
    public bool movingDown;
    public bool moving;

    //Переменная, остановлен ли игрок.
    public bool stopped;
    //Переменная, обозначающая в воздухе ли игрок.
    public bool isInAir;

    //На старте присваиваем переменной - ригидбоди игрока.
    private void Start()
    {
        playerRB = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        //В зависимости от направления движения, считываемого из инпута, применяем силу в соответствующем направлении. При этом переключаем переменные, отвечающие за движение и остановку.
        if (movingRight)
        {
            playerRB.AddForce(Vector3.right * movementSpeed, ForceMode.Force);
            stopped = false;
            moving = true;
        }
        else if (movingLeft)
        {
            playerRB.AddForce(Vector3.left * movementSpeed, ForceMode.Force);
            stopped = false;
            moving = true;
        }
        else if (movingUp)
        {
            playerRB.AddForce(Vector3.forward * movementSpeed, ForceMode.Force);
            stopped = false;
            moving = true;
        }
        else if (movingDown)
        {
            playerRB.AddForce(Vector3.back * movementSpeed, ForceMode.Force);
            stopped = false;
            moving = true;
        }
    }

    //Метод, останавливающий игрока прям на месте. Переключает все переменные, отвечающие за движение.
    public void StopPlayer()
    {
        if (moving && stopped == false)
        {
            playerRB.velocity = Vector3.zero;
            playerRB.angularVelocity = Vector3.zero;
            movingRight = false;
            movingLeft = false;
            movingUp = false;
            movingDown = false;
            moving = false;
            stopped = true;
        }
    }

    //Метод, отвечающий за прыжок игрока, если он не в воздухе.
    public void Jump()
    {
        if (isInAir == false)
        {
            playerRB.velocity = Vector3.up * jumpForce;
        }
    }

    //Если игрок входит в коллизию с землей - он не в воздухе. Если он взаимодействует с границами - игрок останавливается. При этом, игрока слегка отталкивает от границы, чтобы не стоять в коллизии.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isInAir = false;
        }
        else if (collision.gameObject.CompareTag("Border"))
        {
            StopPlayer();
            Vector3 direction = transform.position - collision.transform.position;
            playerRB.AddForce(direction.normalized * collisionImpulsePower * Vector3.Distance(collision.transform.position, transform.position), ForceMode.Impulse);
        }
    }

    //Если игрок стоит в коллизии с землей - он не в воздухе.
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isInAir = false;
        }
    }

    //Если игрок вышел из коллизии с землей - он не в воздухе.
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isInAir = true;
        }
    }
}
