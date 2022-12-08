using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [Header("Damage which take at melee range")]
    [SerializeField] private float meleeDamage;

    [Header("Boolean variable which define enemy in melee trigger of Player")]
    [SerializeField] private bool isEnemyInRange;
    
    //GameObject врага.
    private GameObject enemy;

    //Аниматор игрока, для смены анимаций.
    private Animator playerAnim;

    private void Awake()
    {
        playerAnim = GetComponent<Animator>();
    }

    public void PlayAttackAnimation()
    {
        playerAnim.SetBool("IsAttacking", true);
    }

    public void StopAttackAnimation()
    {
        playerAnim.SetBool("IsAttacking", false);
    }

    private void OnTriggerEnter2D(Collider2D enemyCollision)
    {
        if (enemyCollision.tag == "Damageable")
        {
            isEnemyInRange = true;
            enemy = enemyCollision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D enemyCollision)
    {
        if (enemyCollision.tag == "Damageable")
        {
            isEnemyInRange = false;
        }
    }

    public void Hit()
    {
        if (isEnemyInRange)
        {
            enemy.GetComponent<Health>().TakeDamage(meleeDamage); 
        }
    }
}
