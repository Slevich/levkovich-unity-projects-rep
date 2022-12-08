using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Speed of enemy's movement and time when he is reverting")]
    [SerializeField] private float Speed, TimeToRevert;

    [Header("Animator of the enemy")]
    [SerializeField] private Animator enemyAnim;

    [Header("Sprite Renderer of the enemy")]
    [SerializeField] private SpriteRenderer enemySR;

    //Rigidbody2D противника для изменения его местоположения (velocity)/
    private Rigidbody2D enemyRB;

    //Компонент со скриптом "Health" для определения живой или нет
    private Health enemyHealth;

    //Постоянная переменная, содержащая информацию о состоянии "Покой".
    private const float IDLE_STATE = 0;

    //Постоянная переменная, содержащая информацию о состоянии "Ходьба".
    private const float WALK_STATE = 1;

    //Постоянная переменная, содержащая информацию о состоянии "Разворот".
    private const float REVERT_STATE = 2;

    //Постоянная переменная, содержащая информацию о состоянии "Смерть".
    private const float DEATH_STATE = 3;

    //Переменные нужные для обнуления текущего состояния и времени до разворота.
    private float currentState, currentTimeToRevert;

    private void Awake()
    {
        currentState = WALK_STATE;
        currentTimeToRevert = 0;
        enemyRB = GetComponent<Rigidbody2D>();
        enemyHealth = GetComponent<Health>();
    }

    private void Update()
    {
        if (currentTimeToRevert >= TimeToRevert)
        {
            currentTimeToRevert = 0;
            currentState = REVERT_STATE;
        }

        if (enemyHealth.isAlive == false)
        {
            currentState = DEATH_STATE;
        }

        switch(currentState)
        {
            case IDLE_STATE:
                currentTimeToRevert += Time.deltaTime;
                break;
            case WALK_STATE:
                enemyRB.velocity = Vector2.right * Speed;
                break;
            case REVERT_STATE:
                enemySR.flipX = !enemySR.flipX;
                Speed *= -1;
                currentState = WALK_STATE;
                break;
            case DEATH_STATE:
                enemyRB.velocity = Vector2.zero;
                break;
        }

        enemyAnim.SetFloat("Velocity", enemyRB.velocity.magnitude);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyStopper"))
        {
            currentState = IDLE_STATE;
        }
    }
}
