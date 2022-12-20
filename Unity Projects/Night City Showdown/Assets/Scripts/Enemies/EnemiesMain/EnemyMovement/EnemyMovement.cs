using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    #region Поля
    [Header("Borders of waiting until moving.")]
    [SerializeField] private float minWaitBorder;
    [SerializeField] private float maxWaitBorder;
    [Header("Speed of movement.")]
    [SerializeField] private float speed;
    [Header("The distance at which the enemy stops when reaching a point.")]
    [SerializeField] private float enemyStopDistance;

    //Точка, к которой направляется враг.
    protected Vector2 targetPoint;
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
    #endregion

    #region Свойства
    public bool IsMoving { get { return isMoving; } set { isMoving = value; } }
    public bool PlayerDetected { get { return playerDetected; } set { playerDetected = value; } }
    public bool WallDetected { get { return wallDetected; } set { wallDetected = value; } }
    protected float Speed { get { return speed; } }
    protected bool IsEnemyReachPoint { get { return enemyReachPoint; } }
    protected bool TimerGenerated { get { return timerGenerated; } set { timerGenerated = value; } }
    protected float MinWaitBorder { get { return minWaitBorder; } }
    protected float MaxWaitBorder { get { return maxWaitBorder; } }
    protected float WaitingTimer { get { return waitingTimer; } set { waitingTimer = value; } }
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

    protected void UpdateEnemySpriteFlip(Rigidbody2D enemyRB, SpriteRenderer enemySR)
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

    protected virtual void GenerateTargetPoint(SpriteRenderer enemySR, CapsuleCollider2D enemyRangeTrigger, CircleCollider2D enemyFindingTrigger)
    {
        if (wallDetected == false)
        {
            float randomNumber = Random.Range(0f, 1f);
            if (randomNumber <= 0.5) targetPoint = new Vector2(Random.Range(transform.position.x + enemyRangeTrigger.size.x, 
                                                                            transform.position.x + enemyFindingTrigger.radius), 
                                                                            transform.position.y);
            else targetPoint = new Vector2(Random.Range(transform.position.x - enemyFindingTrigger.radius, 
                                                        transform.position.x - enemyRangeTrigger.size.x), 
                                                        transform.position.y);
            timerGenerated = false;
            wallDetected = false;
            isMoving = true;
        }
        else
        {
            if (enemySR.flipX)
            {
                targetPoint = new Vector2(Random.Range(transform.position.x + enemyRangeTrigger.size.x, 
                                                       transform.position.x + enemyFindingTrigger.radius), 
                                                       transform.position.y);
                timerGenerated = false;
                wallDetected = false;
                isMoving = true;
            }
            else
            {
                targetPoint = new Vector2(Random.Range(transform.position.x - enemyFindingTrigger.radius, 
                                                       transform.position.x - enemyRangeTrigger.size.x), 
                                                       transform.position.y);
                timerGenerated = false;
                wallDetected = false;
                isMoving = true;
            }
        }
    }

    protected void EnemyGoesToPosition(Vector2 targetPoint, EnemyChecks enemyChecks, Rigidbody2D enemyRB, EnemyAnimation enemyAnim)
    {
        enemyAnim.ChangeEnemyState(2);
        Vector2 enemyTargetPoint = targetPoint;
        Vector2 heading = enemyTargetPoint - new Vector2(transform.position.x, transform.position.y);
        float targetDistance = heading.magnitude;
        Vector2 movingDirection = heading / targetDistance;

        if (enemyChecks.IsFalling) enemyRB.velocity = Vector2.down * speed;
        else enemyRB.velocity = movingDirection * speed;
        
        if (playerDetected == false)
        {
            EnemyReachPoint();

            if (enemyReachPoint || wallDetected) StopEnemy(enemyAnim, enemyRB);
        }
    }

    protected void EnemyReachPoint()
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

    protected void StopEnemy(EnemyAnimation enemyAnim, Rigidbody2D enemyRB)
    {
        if (isMoving)
        {
            enemyAnim.ChangeEnemyState(1);
            enemyRB.velocity = Vector2.zero;
            isMoving = false;
        }
    }

    protected virtual void WaitingTimerBeforeMovement(EnemyAnimation enemyAnim, SpriteRenderer enemySR, CapsuleCollider2D enemyRangeTrigger, CircleCollider2D enemyFindingTrigger)
    {
        enemyAnim.ChangeEnemyState(1);

        if (timerGenerated == false)
        {
            waitingTimer = Random.Range(minWaitBorder, maxWaitBorder);
            timerGenerated = true;
        }

        waitingTimer -= Time.deltaTime;

        if (waitingTimer < 0)
        {
            GenerateTargetPoint(enemySR, enemyRangeTrigger, enemyFindingTrigger);
        }
    }
    #endregion
}
