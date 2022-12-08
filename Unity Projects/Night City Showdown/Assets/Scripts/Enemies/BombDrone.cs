using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombDrone : MonoBehaviour
{
    #region Переменные
    [Header("Borders of waiting until moving.")]
    [SerializeField] private float minWaitBorder;
    [SerializeField] private float maxWaitBorder;
    [Header("Speed of movement.")]
    [SerializeField] private float speed;
    [Header("Damage which apply to player's health.")]
    [SerializeField] private float explosionDamage;
    [Header("Points, which player earned for killing character.")]
    [SerializeField] private int pointsCost;
    [Header("Transform component of the player game object (Joy).")]
    [SerializeField] private Transform playerTransform;
    [Header("Game object prefab, which spawned on character's death.")]
    [SerializeField] private GameObject explosionVFXPrefab;
    [Header("Colliders, which attached to childrens game objects.")]
    [SerializeField] private CircleCollider2D enemyFindingTrigger;
    [SerializeField] private CircleCollider2D enemyCheckGroundTrigger;
    [Header("Image, which represents current character's health level.")]
    [SerializeField] private Image HPBar;
    [Header("Variables, which reflect some conditions.")]
    public bool playerDetected;
    public bool groundDetected;
    public bool enemyReachPlayer;

    //Аниматор персонажа.
    private Animator enemyAnim;
    //Rigidbody персонажа.
    private Rigidbody2D enemyRB;
    //Компонент здоровья персонажа.
    private Health enemyHealth;
    //Sprite rendere персонажа.
    private SpriteRenderer enemySR;
    //Точка, к которой направляется персонаж при движении.
    private Vector2 targetPoint;
    //Переменная обозначающая текущее состояние персонажа.
    private int enemyStateNumber;
    //Таймер, в течении которого персонаж ждет.
    private float waitingTimer;
    //Текущее ХП персонажа.
    private float enemyHP;
    //Постоянные переменные персонажа, обозначающие его состояния.
    private const int IDLE_STATE = 0;
    private const int WALK_STATE = 1;
    //Переменная, нужная для генерации таймера.
    private bool timerGenerated;
    //Переменная, обозначающая достиг ли персонаж точки назначения.
    private bool enemyReachPoint;
    //Переменная, обозначающая жив ли персонаж.
    private bool isAlive;
    #endregion

    #region Методы
    /// <summary>
    /// На старте получаем необходимые компоненты.
    /// Дефолтный стейт - айдл (0).
    /// </summary>
    void Start()
    {
        enemyAnim = GetComponent<Animator>();
        enemyRB = GetComponent<Rigidbody2D>();
        enemySR = GetComponent<SpriteRenderer>();
        enemyHealth = GetComponent<Health>();
        enemyStateNumber = 0;
    }

    /// <summary>
    /// В Update постоянно обновляем стейты игрока.
    /// Если игрок замечен - стейт передвижения (1),
    /// когда персонаж достиг игрока - взрыв.
    /// Если игрок не замечен, выбирается рандомная точка,
    /// персонаж летит к ней. Достигает - остановки и ожидание.
    /// Также постоянно обновляется здоровье персонажа.
    /// При смерти - начисляется в статистику (очки и плюс к количеству убитых),
    /// после чего взрыв.
    /// </summary>
    void Update()
    {
        EnemyStates();

        if (playerDetected)
        {
            targetPoint = playerTransform.position;
            enemyStateNumber = 1;
            UpdateEnemySpriteFlip();

            if (enemyReachPlayer)
            {
                Explosion();
            }
        }
        else
        {
            UpdateEnemySpriteFlip();
            EnemyReachPoint();

            if (enemyReachPoint)
            {
                StopDrone();
            }
        }

        UpdateEnemiesHealth();

        if (isAlive == false && enemyReachPlayer == false)
        {
            playerTransform.GetComponent<MainCharUICounts>().enemyKilled++;
            playerTransform.GetComponent<MainCharUICounts>().pointsEarned += pointsCost;
            Explosion();
        }
    }

    /// <summary>
    /// Метод обновляет состояния персонажа.
    /// Он либо стоит и ждем, после чего генерируется точка.
    /// Либо направляется к точке.
    /// </summary>
    private void EnemyStates()
    {
        switch (enemyStateNumber)
        {
            case IDLE_STATE:
                enemyAnim.SetFloat("Speed", 0f);
                
                if (timerGenerated == false)
                {
                    waitingTimer = Random.Range(minWaitBorder, maxWaitBorder);
                    timerGenerated = true;
                }

                waitingTimer -= Time.deltaTime;

                if (waitingTimer < 0)
                {
                    GenerateRandomPoint();
                }
                break;

            case WALK_STATE:
                enemyAnim.SetFloat("Speed", 1f);
                EnemyGoesToPosition();
                break;
        }
    }

    /// <summary>
    /// Метод генерирует рандомную точку в пространстве, внутри триггера.
    /// После чего меняет стейт на перемещение.
    /// </summary>
    private void GenerateRandomPoint()
    {
        if (playerDetected == false)
        {
            targetPoint = new Vector2(Random.Range(transform.position.x - enemyFindingTrigger.radius, 
                                                   transform.position.x + enemyFindingTrigger.radius), 
                                      Random.Range(transform.position.y - enemyFindingTrigger.radius, 
                                                   transform.position.y + enemyFindingTrigger.radius));
            enemyStateNumber = 1;
            timerGenerated = false;
        }
    }

    /// <summary>
    /// Метод рассчитывает направление движения персонажа.
    /// Затем передает его в велосити, домножая на скорость.
    /// При этом идет проверка, достиг ли персонаж точки.
    /// </summary>
    private void EnemyGoesToPosition()
    {
        Vector3 enemyTargetPoint = new Vector3(targetPoint.x, targetPoint.y, 0);
        Vector3 heading = enemyTargetPoint - transform.position;
        float targetDistance = heading.magnitude;
        Vector3 movingDirection = heading / targetDistance;

        enemyRB.velocity = movingDirection * speed;
        EnemyReachPoint();
    }

    /// <summary>
    /// Метод спавнит префаб с анимацией взрыва.
    /// Если персонаж достиг игрока, то наносит ему урон.
    /// После чего самоуничтожается.
    /// </summary>
    private void Explosion()
    {
        GameObject explosionVFX = Instantiate(explosionVFXPrefab, transform.position, Quaternion.identity);
        gameObject.GetComponent<SpawnObjectOnDeath>().SpawnObject();

        if (enemyReachPlayer)
        {
            playerTransform.GetComponent<Health>().ToDamage(explosionDamage);
        }

        Destroy(gameObject);
    }

    /// <summary>
    /// Метод останавливает персонажа, приводя его велосити к нулю.
    /// Текущий стейт переключается в положение - ожидание.
    /// </summary>
    private void StopDrone()
    {
        enemyRB.velocity = Vector2.zero;
        enemyStateNumber = 0;
    }

    /// <summary>
    /// Метод проверяет дистанцию между персонажем и целью.
    /// Если она меньше радиуса внутреннего триггера - он дистиг цель.
    /// </summary>
    private void EnemyReachPoint()
    {
        if (Vector2.Distance(transform.position, targetPoint) < enemyCheckGroundTrigger.radius)
        {
            enemyReachPoint = true;
        }
        else
        {
            enemyReachPoint = false;
        }
    }

    /// <summary>
    /// Метод обновляет поворот спрайта, 
    /// в зависимости от направления движения персонажа.
    /// </summary>
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

    /// <summary>
    /// Метод рассчитывает процент текущего здоровья персонажа,
    /// после чего передает его в Image с HP Bar.
    /// Также проверяет, жив ли персонаж.
    /// </summary>
    private void UpdateEnemiesHealth()
    {
        enemyHP = enemyHealth.currentHealth / enemyHealth.maxHealth;
        HPBar.fillAmount = enemyHP;
        isAlive = enemyHealth.isAlive;
    }
    #endregion
}
