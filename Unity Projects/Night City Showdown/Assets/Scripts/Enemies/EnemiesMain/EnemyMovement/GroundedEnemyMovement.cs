using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedEnemyMovement : EnemyMovement
{
    [Header("Transform component of the player game object (Joy).")]
    [SerializeField] private Transform playerTransform;

    private CircleCollider2D enemyFindingTrigger;
    private CapsuleCollider2D enemyRangeTrigger;
    private Rigidbody2D enemyRB;
    private SpriteRenderer enemySR;
    private EnemyChecks enemyChecks;
    private EnemyAnimation enemyAnim;

    private void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        enemySR = GetComponent<SpriteRenderer>();
        enemyChecks = GetComponent<EnemyChecks>();
        enemyAnim = GetComponent<EnemyAnimation>();
        enemyFindingTrigger = GetComponentInChildren<CircleCollider2D>();
        enemyRangeTrigger = GetComponentInChildren<CapsuleCollider2D>();
    }

    private void Update()
    {
        Debug.DrawLine(transform.position, targetPoint, Color.blue);
        UpdateEnemySpriteFlip(enemyRB, enemySR);

        if (PlayerDetected)
        {
            if (IsMoving)
            {
                EnemyGoesToPosition(new Vector2(playerTransform.position.x, transform.position.y), enemyChecks, enemyRB, enemyAnim);
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
                EnemyGoesToPosition(targetPoint, enemyChecks, enemyRB, enemyAnim);
            }
            else
            {
                WaitingTimerBeforeMovement(enemyAnim, enemySR, enemyRangeTrigger, enemyFindingTrigger);
            }
        }
    }
}
