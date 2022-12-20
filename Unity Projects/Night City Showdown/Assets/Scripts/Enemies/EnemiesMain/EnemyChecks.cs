using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChecks : MonoBehaviour
{
    #region Переменные
    [SerializeField] private float groundRayDistance;
    [SerializeField] private float sidesRayDistance;
    [SerializeField] private float voidRayDistance;
    [Header("Layers masks.")]
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask enemyMask;

    private CapsuleCollider2D enemyAttackRangeTrigger;
    private EnemyMovement enemyMovement;
    //Переключатель, обозначающий падает ли враг.
    private bool isFalling;
    //Переключатель, обозначающий замечен ли другой враг спереди.
    private bool isEnemyForward;
    //Переключатель, обозначающий замечен ли другой враг сзади.
    private bool isEnemyBackward;
    //Переключатель, обозначающий находится ли земля спереди.
    private bool isGroundForward;
    //Переключатель, обозначающий находится ли земля сзади.
    private bool isGroundBackward;

    public bool IsFalling { get { return isFalling; } }
    public bool IsEnemyForward { get { return isEnemyForward; } }
    public bool IsEnemyBackward { get { return isEnemyBackward; } }
    public bool IsGroundForward { get { return isGroundForward; } }
    public bool IsGroundBackward { get { return isGroundBackward; } }

    #endregion

    #region Методы
    private void Start()
    {
        enemyAttackRangeTrigger = GetComponentInChildren<CapsuleCollider2D>();
        enemyMovement = GetComponent<EnemyMovement>();
    }

    private void Update()
    {
        GroundCheck();

        if (enemyMovement.IsMoving)
        {
            CheckVoid();
            AnotherEnemyCheck();
        }
    }

    public void GroundCheck()
    {
        isFalling = !Physics2D.Raycast(transform.position, 
                                      Vector2.down, 
                                      groundRayDistance, 
                                      groundMask);
    }

    public void AnotherEnemyCheck()
    {
        isEnemyForward = Physics2D.Raycast(new Vector2(enemyAttackRangeTrigger.transform.position.x + 
                                                      (enemyAttackRangeTrigger.size.x / 2),
                                                       enemyAttackRangeTrigger.transform.position.y),
                                                       Vector2.right, sidesRayDistance, enemyMask);
        isEnemyBackward = Physics2D.Raycast(new Vector2(enemyAttackRangeTrigger.transform.position.x -
                                                       (enemyAttackRangeTrigger.size.x / 2),
                                                       enemyAttackRangeTrigger.transform.position.y),
                                                       Vector2.left, sidesRayDistance, enemyMask);
    }

    public void CheckVoid()
    {
        isGroundForward = Physics2D.Raycast(enemyAttackRangeTrigger.transform.position, 
                                            new Vector2(0.4f, -0.65f), 
                                            voidRayDistance, 
                                            groundMask);
        isGroundBackward = Physics2D.Raycast(enemyAttackRangeTrigger.transform.position, 
                                             new Vector2(-0.4f, -0.65f), 
                                             voidRayDistance, 
                                             groundMask);
    }
    #endregion
}
