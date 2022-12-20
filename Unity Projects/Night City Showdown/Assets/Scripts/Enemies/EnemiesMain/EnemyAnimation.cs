using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    EnemyStates enemyStates = new EnemyStates();
    private Animator enemyAnim;

    enum EnemyStates : int
    {
        Idle = 1,
        Move = 2,
        Attack = 3,
        Death = 4
    }

    private void Start()
    {
        enemyAnim = GetComponent<Animator>();
    }

    void Update()
    {
        EnemyAnimStates();
    }

    private void EnemyAnimStates()
    {
        switch (enemyStates)
        {
            case EnemyStates.Idle:
                enemyAnim.SetBool("IsAttack", false);
                enemyAnim.SetFloat("Speed", 0f);
                break;

            case EnemyStates.Move:
                enemyAnim.SetBool("IsAttack", false);
                enemyAnim.SetFloat("Speed", 1f);
                break;

            case EnemyStates.Attack:
                enemyAnim.SetBool("IsAttack", true);
                break;

            case EnemyStates.Death:
                enemyAnim.SetBool("IsDead", true);
                break;
        }
    }

    public void ChangeEnemyState(int enemyState)
    {
        enemyStates = (EnemyStates)enemyState;
    }
}
