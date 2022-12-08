using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCutsceneController : MonoBehaviour
{
    [Header("Speed of enemy movement at cutscene")]
    [SerializeField] private float speed;

    [Header("Boolean variable for two conditions of enemy movement")]
    public bool goForward;
    public bool goBackward;

    //Rigidbody2D противника для изменения его перемещения (velocity).
    private Rigidbody2D enemyRB;
    
    //SpriteRenderer противника для отзеркаливания его спрайта при движении назад.
    private SpriteRenderer enemySR;

    private void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        enemySR = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (goForward)
        {
            enemyRB.velocity = Vector2.left * speed;
        }
        else if (goBackward)
        {
            enemySR.flipX = false;
            enemyRB.velocity = Vector2.right * speed;
        }
    }
}
      
