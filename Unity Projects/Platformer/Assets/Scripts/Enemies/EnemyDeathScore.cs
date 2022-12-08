using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathScore : MonoBehaviour
{
    [Header("Score which given to player for killing this enemy")]
    [SerializeField] private float scoreValue;

    [Header("GameObjest of the Player")]
    [SerializeField] private GameObject player;

    //Компонент со скриптом "Health" для определения жив враг или умер.
    private Health enemiesHealth;

    //Переменная bool для того, чтобы начислить очки один раз (как переключатель), а не каждый фрейм до исчезновения его GameObject'а.
    private bool scoreAssessed;

    private void Start()
    {
        scoreAssessed = false;
        enemiesHealth = GetComponent<Health>();
    }

    private void Update()
    {
        if (enemiesHealth.isAlive == false)
        {
            if (scoreAssessed == false)
            {
                player.GetComponent<ScoreCounter>().AssessEnemiesDeathScore(scoreValue);
                scoreAssessed = true;
            }
        }
    }
}
