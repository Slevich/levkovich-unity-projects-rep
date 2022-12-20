using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirEnemyMovement : EnemyMovement
{
    [Header("Transform component of the player game object (Joy).")]
    [SerializeField] private Transform playerTransform;

    private float maxYPosition;
    private CircleCollider2D enemyFindingTrigger;
    private CapsuleCollider2D enemyRangeTrigger;
    private Rigidbody2D enemyRB;
    private SpriteRenderer enemySR;
    private EnemyAnimation enemyAnim;

    private void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        enemySR = GetComponent<SpriteRenderer>();
        enemyAnim = GetComponent<EnemyAnimation>();
        enemyFindingTrigger = GetComponentInChildren<CircleCollider2D>();
        enemyRangeTrigger = GetComponentInChildren<CapsuleCollider2D>();
        maxYPosition = transform.position.y + enemyFindingTrigger.radius;
    }

    private void Update()
    {
        Debug.DrawLine(transform.position, targetPoint, Color.blue);
        UpdateEnemySpriteFlip(enemyRB, enemySR);

        if (PlayerDetected)
        {
            if (IsMoving)
            {
                EnemyGoesToPosition(playerTransform.position);
            }
            else
            {
                enemyRB.velocity = Vector2.zero;
            }
        }
        else
        {
            if (IsMoving)
            {
                EnemyGoesToPosition(targetPoint);
            }
            else
            {
                WaitingTimerBeforeMovement(enemyRangeTrigger, enemyFindingTrigger);
            }
        }
    }

    private void EnemyGoesToPosition(Vector2 targetPoint)
    {
        enemyAnim.ChangeEnemyState(2);
        Vector2 enemyTargetPoint = targetPoint;
        Vector2 heading = enemyTargetPoint - new Vector2(transform.position.x, transform.position.y);
        float targetDistance = heading.magnitude;
        Vector2 movingDirection = heading / targetDistance;

        enemyRB.velocity = movingDirection * Speed;

        if (PlayerDetected == false)
        {
            EnemyReachPoint();

            if (IsEnemyReachPoint || WallDetected) StopEnemy(enemyAnim, enemyRB);
        }
    }

    protected virtual void GenerateTargetPoint(CapsuleCollider2D enemyRangeTrigger, CircleCollider2D enemyFindingTrigger)
    {
        float randomNumber = Random.Range(0f, 1f);
        if (randomNumber <= 0.5) targetPoint = new Vector2(Random.Range(transform.position.x + enemyRangeTrigger.size.x,
                                                                        transform.position.x + enemyFindingTrigger.radius),
                                                           Random.Range(transform.position.y, maxYPosition));
        else targetPoint = new Vector2(Random.Range(transform.position.x - enemyFindingTrigger.radius,
                                                    transform.position.x - enemyRangeTrigger.size.x),
                                       Random.Range(transform.position.y - enemyFindingTrigger.radius, transform.position.y));
        IsMoving = true;
        TimerGenerated = false;
    }

    private void WaitingTimerBeforeMovement(CapsuleCollider2D enemyRangeTrigger, CircleCollider2D enemyFindingTrigger)
    {
        enemyAnim.ChangeEnemyState(1);

        if (TimerGenerated == false)
        {
            WaitingTimer = Random.Range(MinWaitBorder, MaxWaitBorder);
            TimerGenerated = true;
        }

        WaitingTimer -= Time.deltaTime;

        if (WaitingTimer < 0)
        {
            GenerateTargetPoint(enemyRangeTrigger, enemyFindingTrigger);
        }
    }
}
