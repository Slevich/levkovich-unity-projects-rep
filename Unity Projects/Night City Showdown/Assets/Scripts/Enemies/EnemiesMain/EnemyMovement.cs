using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    #region Переменные
    [Header("Borders of waiting until moving.")]
    [SerializeField] private float minWaitBorder;
    [SerializeField] private float maxWaitBorder;
    [Header("Speed of movement.")]
    [SerializeField] private float speed;
    [Header("The distance at which the enemy stops when reaching a point.")]
    [SerializeField] private float enemyStopDistance;
    [Header("Finding trigger, which attached to children game object.")]
    [SerializeField] private CircleCollider2D enemyFindingTrigger;
    [Header("Transform component of the player game object (Joy).")]
    [SerializeField] private Transform playerTransform;

    //Компонент с проверками врага.
    private EnemyChecks enemyChecks;
    //Rigidbody врага.
    private Rigidbody2D enemyRB;
    //Sprite renderer врага.
    private SpriteRenderer enemySR;
    //Точка, к которой направляется враг.
    private Vector2 targetPoint;
    //Переключатель, обозначающий, движется ли враг.
    private bool isMoving;
    //Переменная, обозначающая достиг ли враг точки.
    private bool enemyReachPoint;
    //Переключатель, обозначающий попала ли в триггер стена.
    private bool wallDetected;
    //Переключатель, обозначающий попал ли игрок в триггер.
    private bool playerDetected;
    //Переключатель, обозначающий сгенерирован ли таймер.
    private bool timerGenerated;
    //Время, которое ждет враг перед продолжением движения.
    private float waitingTimer;
    //Переключатель, обозначающий падает ли враг.

    public bool IsMoving { get { return isMoving; } }
    public bool PlayerDetected { set { playerDetected = value; } }
    public bool WallDetected { set { wallDetected = value; } }
    #endregion

    #region Методы
    private void OnValidate()
    {
        if (speed < 0)
        {
            speed = 0;
        }

        if (minWaitBorder < 0)
        {
            minWaitBorder = 0;
        }
        else if (maxWaitBorder < 0)
        {
            maxWaitBorder = 0;
        }
        else if (maxWaitBorder < minWaitBorder)
        {
            maxWaitBorder = minWaitBorder + 1f;
        }
    }

    private void Start()
    {
        enemyChecks = GetComponent<EnemyChecks>();
        enemyRB = GetComponent<Rigidbody2D>();
        enemySR = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Debug.Log($"Wall detected: {wallDetected}");
        Debug.Log($"Player detected: {playerDetected}");
        Debug.DrawLine(transform.position, targetPoint, Color.blue);

        if (playerDetected)
        {
            if (isMoving)
            {
                UpdateEnemySpriteFlip();
                EnemyGoesToPosition(new Vector2(playerTransform.position.x, transform.position.y));
            }
            else
            {

            }
        }
        else
        {
            if (isMoving)
            {
                UpdateEnemySpriteFlip();
                EnemyGoesToPosition(targetPoint);
            }
            else
            {
                WaitingTimerBeforeMovement();
            }
        }
    }

    private void UpdateEnemySpriteFlip()
    {
        if (enemyRB.velocity.x > 0)
        {
            enemySR.flipX = false;
        }
        else if (enemyRB.velocity.x < 0)
        {
            enemySR.flipX = true;
        }
    }

    private void GenerateTargetPoint()
    {
        if (wallDetected == false)
        {
            float randomNumber = Random.Range(0f, 1f);
            if (randomNumber <= 0.5) targetPoint = new Vector2(Random.Range(transform.position.x, 
                                                                            transform.position.x + enemyFindingTrigger.radius), 
                                                                            transform.position.y);
            else targetPoint = new Vector2(Random.Range(transform.position.x - enemyFindingTrigger.radius, 
                                                        transform.position.x), 
                                                        transform.position.y);
            timerGenerated = false;
            wallDetected = false;
            isMoving = true;
        }
        else if (wallDetected || enemyChecks.IsEnemyForward || enemyChecks.IsEnemyBackward || 
                 enemyChecks.IsGroundForward == false || enemyChecks.IsGroundBackward == false)
        {
            if (enemySR.flipX)
            {
                targetPoint = new Vector2(Random.Range(transform.position.x, 
                                                       transform.position.x + enemyFindingTrigger.radius), 
                                                       transform.position.y);
                timerGenerated = false;
                wallDetected = false;
                isMoving = true;
            }
            else
            {
                targetPoint = new Vector2(Random.Range(transform.position.x - enemyFindingTrigger.radius, 
                                                       transform.position.x), 
                                                       transform.position.y);
                timerGenerated = false;
                wallDetected = false;
                isMoving = true;
            }
        }
    }

    private void EnemyGoesToPosition(Vector2 targetPoint)
    {
        Vector2 enemyTargetPoint = targetPoint;
        Vector2 heading = enemyTargetPoint - new Vector2(transform.position.x, transform.position.y);
        float targetDistance = heading.magnitude;
        Vector2 movingDirection = heading / targetDistance;

        if (enemyChecks.IsFalling) enemyRB.velocity = Vector2.down * speed;
        else enemyRB.velocity = movingDirection * speed;
        
        EnemyReachPoint();

        if (enemyReachPoint || wallDetected || enemyChecks.IsEnemyForward || enemyChecks.IsEnemyBackward || 
            enemyChecks.IsGroundForward == false || enemyChecks.IsGroundBackward == false) StopEnemy();
    }

    private void EnemyReachPoint()
    {
        if (Vector2.Distance(transform.position, targetPoint) <= enemyStopDistance)
        {
            enemyReachPoint = true;
        }
        else
        {
            enemyReachPoint = false;
        }
    }

    private void StopEnemy()
    {
        enemyRB.velocity = Vector2.zero;
        isMoving = false;
    }

    private void WaitingTimerBeforeMovement()
    {
        if (timerGenerated == false)
        {
            waitingTimer = Random.Range(minWaitBorder, maxWaitBorder);
            timerGenerated = true;
        }

        waitingTimer -= Time.deltaTime;

        if (waitingTimer < 0)
        {
            GenerateTargetPoint();
        }
    }
    #endregion
}
