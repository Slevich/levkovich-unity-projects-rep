using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dog : MonoBehaviour
{
    #region Переменные
    [Header("Borders of waiting until moving.")]
    [SerializeField] private float minWaitBorder;
    [SerializeField] private float maxWaitBorder;
    [Header("Speed of movement.")]
    [SerializeField] private float speed;
    [Header("Damage which apply to player's health.")]
    [SerializeField] private float attackDamage;
    [Header("The interval of time after which the enemy attacks.")]
    [SerializeField] private float attackInterval;
    [Header("Length of different rays.")]
    [SerializeField] private float enemyStopDistance;
    [SerializeField] private float sidesRayDistance;
    [SerializeField] private float voidRayDistance;
    [Header("Points, which player earned for killing enemy.")]
    [SerializeField] private int pointsCost;
    [Header("Layers masks.")]
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask enemyMask;
    [Header("Game object prefab, which contains animation of death VFX.")]
    [SerializeField] private GameObject enemyDeathVFXPrefab;
    [Header("Attack range trigger, which attached to children game object.")]
    [SerializeField] private CapsuleCollider2D enemyAttackRangeTrigger;
    [Header("Finding trigger, which attached to children game object.")]
    [SerializeField] private BoxCollider2D enemyFindingTrigger;
    [Header("Image, which represents current character's health level.")]
    [SerializeField] private Image HPBar;
    [Header("Transform component of the player game object (Joy).")]
    [SerializeField] private Transform playerTransform;
    [Header("Variables, which reflect some conditions.")]
    public bool playerInRange;
    public bool wallDetected;
    public bool playerDetected;


    //Аниматор врага.
    private Animator enemyAnim;
    //Rigidbody врага.
    private Rigidbody2D enemyRB;
    //Компонент здоровья врага.
    private Health enemyHealth;
    //Sprite renderer врага.
    private SpriteRenderer enemySR;
    //Точка, к которой направляется враг.
    private Vector2 targetPoint;
    //Размер триггера врага.
    private Vector2 triggerExtents;
    //Границы триггера врага.
    private Bounds triggerBounds;
    //Текущий номер стейта врага.
    private float enemyStateNumber;
    //Время, которое ждет враг перед движением.
    private float waitingTimer;
    //Процент здоровья врага.
    private float enemyHP;
    //Таймер для обнуления интервала атаки врага.
    private float currentAttackInterval;
    //Постоянные переменные, обозначающие номера стейтов врага.
    private const float PATROL_STATE = 0;
    private const float IDLE_STATE = 1;
    private const float WALK_STATE = 2;
    private const float ATTACK_STATE = 3;
    private const float DEATH_STATE = 4;
    //Переменная обозначающая, сгенерирован ли таймер.
    private bool timerGenerated;
    //Переменная обозначающая, достиг ли враг точки назначения.
    private bool enemyReachPoint;
    //Переменная обозначающая, жив ли враг.
    private bool isAlive;
    //Переменная обозначающая, атакует ли враг.
    private bool isAttack;
    //Переменная обозначающая, окончена ли атака врагаю
    private bool attackEnded;
    //Переменная обозначающая, замечен ли другой враг спереди.
    private bool enemyForward;
    //Переменная обозначающая, замечен ли другой враг сзади.
    private bool enemyBackward;
    //Переменная обозначающая, находится ли земля спереди.
    private bool isGroundForward;
    //Переменная обозначающая, находится ли земля сзади.
    private bool isGroundBackward;
    #endregion

    #region Методы
    /// <summary>
    /// На старте получаем компоненты.
    /// Переводим состояния "жив" в true.
    /// Дефолтный стейт - ожидание (0).
    /// Присваиваем таймеру для обнуления значение.
    /// Рассчитываем крайние точки триггера.
    /// </summary>
    void Start()
    {
        enemyAnim = GetComponent<Animator>();
        enemyRB = GetComponent<Rigidbody2D>();
        enemySR = GetComponent<SpriteRenderer>();
        enemyHealth = GetComponent<Health>();
        isAlive = true;
        enemyStateNumber = 0;
        currentAttackInterval = attackInterval;
        CalculateTriggerBoundsAndExtents();
    }

    /// <summary>
    /// В Update вызываем необходимые методы
    /// в зависимости от того, жив или мертв враг,
    /// замечен ли игрок, находится ли игрок в радиусе атаки.
    /// </summary>
    void Update()
    {
        //DebugStuff();
        EnemyStates();
        UpdateEnemySpriteFlip();
        UpdateHP();
        CheckVoid();
        EnemyCheck();

        if (isAlive)
        {
            if (playerDetected)
            {
                if (playerInRange)
                {
                    ChangeEnemyState(3);
                    EnemyAttack();
                    AttackToIdle();
                }
                else
                {
                    isAttack = false;
                    enemyAnim.SetBool("IsAttack", false);
                    targetPoint = new Vector2(playerTransform.position.x, transform.position.y);
                    enemyStateNumber = 2;
                    EnemyDetectEnemy();
                    EnemyGoesToPosition();
                }
            }
            else
            {
                EnemyReachPoint();
                EnemyDetectEnemy();
                DetectVoid();
            }
        }
        else
        {
            playerTransform.GetComponent<MainCharUICounts>().enemyKilled++;
            playerTransform.GetComponent<MainCharUICounts>().pointsEarned += pointsCost;
            EnemyDeath();
        }
    }

    /// <summary>
    /// Метод обновляет стейты врага
    /// в зависимости от их номера.
    /// </summary>
    private void EnemyStates()
    {
        switch (enemyStateNumber)
        {
            case PATROL_STATE:
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

            case IDLE_STATE:
                enemyAnim.SetFloat("Speed", 0f);
                enemyRB.velocity = Vector2.zero;
                break;

            case WALK_STATE:
                enemyAnim.SetFloat("Speed", 1f);
                EnemyGoesToPosition();
                break;

            case ATTACK_STATE:
                enemyAnim.SetBool("IsAttack", true);
                enemyRB.velocity = Vector2.zero;
                break;

            case DEATH_STATE:
                enemyRB.velocity = Vector2.zero;
                enemyAnim.SetBool("IsDead", true);
                break;
        }
    }

    /// <summary>
    /// Метод генерирует рандомную точку в пространстве.
    /// Если враг не уперся в стену, точка генерируется
    /// в расстоянии от крайних точек внешнего триггера, 
    /// до крайних точек внутреннего, в зависимости от рандомного числа.
    /// Если враг уперся в стену, генерируется рандомная точка
    /// в противоположном от столковения расстоянии.
    /// </summary>
    private void GenerateRandomPoint()
    {
        if (playerDetected == false)
        {
            if (wallDetected == false)
            {
                float randomNumber = Random.Range(0f, 1f);

                if (randomNumber < 0.5) targetPoint = new Vector2(Random.Range(transform.position.x, transform.position.x + triggerExtents.x), transform.position.y);
                else targetPoint = new Vector2(Random.Range(transform.position.x - triggerExtents.x, transform.position.x), transform.position.y);

                enemyStateNumber = 2;
                timerGenerated = false;
                wallDetected = false;
            }
            else
            {
                if (enemySR.flipX)
                {
                    targetPoint = new Vector2(Random.Range(transform.position.x, transform.position.x + triggerExtents.x), transform.position.y);
                    enemyStateNumber = 2;
                    timerGenerated = false;
                    wallDetected = false;
                }
                else
                {
                    targetPoint = new Vector2(Random.Range(transform.position.x - triggerExtents.x, transform.position.x), transform.position.y);
                    enemyStateNumber = 2;
                    timerGenerated = false; 
                    wallDetected = false;
                }
            }
        }
    }

    /// <summary>
    /// Метод рассчитывает направление движения врага,
    /// передает его в велосити.
    /// Велосити меняется в зависимости от того, падает ли сейчас враг или нет, замечен враг или нет.
    /// </summary>
    private void EnemyGoesToPosition()
    {
        Vector3 enemyTargetPoint = new Vector3(targetPoint.x, targetPoint.y, 0);
        Vector3 heading = enemyTargetPoint - transform.position;
        float targetDistance = heading.magnitude;
        Vector3 movingDirection = heading / targetDistance;

        if (playerDetected)
        {
            enemyRB.velocity = movingDirection * speed;
        }
        else
        {
            enemyRB.velocity = movingDirection * speed;
            EnemyReachPoint();

            if (enemyReachPoint || wallDetected)
            {
                ChangeEnemyState(0);
            }
        }
    }

    /// <summary>
    /// Метод меняет текущий номер стейта врага.
    /// </summary>
    /// <param name="enemyState"></param>
    private void ChangeEnemyState(int enemyState)
    {
        enemyStateNumber = enemyState;
    }

    /// <summary>
    /// Метод переключает текущий стейт на атаку (3).
    /// </summary>
    private void EnemyAttack()
    {
        if (isAttack)
        {
            enemyStateNumber = 3;
        }
    }

    /// <summary>
    /// Метод переключает состояние врага из атаки,
    /// на состояние покоя. Когда таймер интервала
    /// атаки проходит, он атакует снова.
    /// </summary>
    private void AttackToIdle()
    {
        if (attackEnded)
        {
            isAttack = false;
            enemyAnim.SetBool("IsAttack", false);
            enemyStateNumber = 1;
            attackInterval -= Time.deltaTime;

            if (attackInterval <= 0)
            {
                attackInterval = currentAttackInterval;
                isAttack = true;
                enemyStateNumber = 3;
                attackEnded = false;
            }
        }
    }

    /// <summary>
    /// Метод переключает состояние, окончена
    /// ли атака на истинное. Данный метод
    /// используется в анимации врага.
    /// </summary>
    private void AttackEnded()
    {
        attackEnded = true;
    }

    /// <summary>
    /// Метод проверяет, находится ли игрок в радиусе атаки.
    /// Если да, вызывает метод наносящий урон.
    /// </summary>
    private void Damage()
    {
        if (playerInRange)
        {
            playerTransform.GetComponent<Health>().ToDamage(attackDamage);
        }
    }

    /// <summary>
    /// Метод рассчитывает текущий процент здоровья врага.
    /// Передает его в Image c HP Bar'ом.
    /// </summary>
    private void UpdateHP()
    {
        enemyHP = enemyHealth.GetCurrentHealthProcent();
        HPBar.fillAmount = enemyHP;
        isAlive = enemyHealth.IsAlive;
    }

    /// <summary>
    /// Метод переключает стейт врага на смерть (4),
    /// после чего спавнит на нем префаб с анимацией
    /// эффекта смерти. После чего враг самоуничтожается.
    /// </summary>
    private void EnemyDeath()
    {
        enemyStateNumber = 4;
        GameObject enemyDeathVFX = Instantiate(enemyDeathVFXPrefab, enemyAttackRangeTrigger.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    /// <summary>
    /// Метод проверяет расстояние от врага до точки назначения,
    /// переключая тем самым переменные, обозначающие
    /// дошел ли он или нет.
    /// </summary>
    private void EnemyReachPoint()
    {
        if (Vector2.Distance(transform.position, targetPoint) < enemyStopDistance)
        {
            enemyReachPoint = true;
        }
        else
        {
            enemyReachPoint = false;
        }
    }

    /// <summary>
    /// Метод поворачивает спрайт врага,
    /// в зависимости от направления его движения.
    /// </summary>
    private void UpdateEnemySpriteFlip()
    {
        if (enemyStateNumber == 2 && enemyRB.velocity.x > 0)
        {
            enemySR.flipX = false;
        }
        else if (enemyStateNumber == 2 && enemyRB.velocity.x < 0)
        {
            enemySR.flipX = true;
        }
    }

    /// <summary>
    /// Метод рассчитывает крайние точки триггера.
    /// </summary>
    private void CalculateTriggerBoundsAndExtents()
    {
        triggerBounds = enemyFindingTrigger.bounds;
        triggerExtents = triggerBounds.extents;
    }

    /// <summary>
    /// Метод кидает два луча под углом в землю,
    /// проверяя тем самым находится ли пустота
    /// спереди или сзади.
    /// </summary>
    private void CheckVoid()
    {
        isGroundForward = Physics2D.Raycast(enemyAttackRangeTrigger.transform.position, new Vector2(0.4f, -0.65f), voidRayDistance, groundMask);
        isGroundBackward = Physics2D.Raycast(enemyAttackRangeTrigger.transform.position, new Vector2(-0.4f, -0.65f), voidRayDistance, groundMask);
    }

    /// <summary>
    /// Метод проверяя если пустота спереди или сзади,
    /// повернут ли спрайт, меняет направление движения
    /// врага от пустота в противоположную сторону.
    /// </summary>
    private void DetectVoid()
    {
        if (isGroundForward == false && enemySR.flipX == false)
        {
            targetPoint = new Vector2((transform.position.x - enemyAttackRangeTrigger.size.x), transform.position.y);
        }
        else if (isGroundBackward == false && enemySR.flipX)
        {
            targetPoint = new Vector2((transform.position.x + enemyAttackRangeTrigger.size.x), transform.position.y);
        }
    }

    /// <summary>
    /// Метод кидает два луча - вперед и назад,
    /// проверяя тем самым, попал ли он во врага.
    /// </summary>
    private void EnemyCheck()
    {
        enemyForward = Physics2D.Raycast(new Vector2(enemyAttackRangeTrigger.transform.position.x + (enemyAttackRangeTrigger.size.x / 2), 
                                                     enemyAttackRangeTrigger.transform.position.y), 
                                                     Vector2.right, sidesRayDistance, enemyMask);
        enemyBackward = Physics2D.Raycast(new Vector2(enemyAttackRangeTrigger.transform.position.x - (enemyAttackRangeTrigger.size.x / 2), 
                                                      enemyAttackRangeTrigger.transform.position.y), 
                                                      Vector2.left, sidesRayDistance, enemyMask);
    }

    /// <summary>
    /// Метод переключает состояния врага, в зависимости от того,
    /// замечен игрок или нет.
    /// При этом, в зависимости от того, с какой стороны враг, 
    /// меняется направление движения врага.
    /// </summary>
    private void EnemyDetectEnemy()
    {
        if (playerDetected)
        {
            if (enemySR.flipX == false && enemyForward)
            {
                enemyStateNumber = 1;
            }
            else if (enemySR.flipX && enemyBackward)
            {
                enemyStateNumber = 1;
            }
            else
            {
                targetPoint = new Vector2(playerTransform.position.x, transform.position.y);
                enemyStateNumber = 2;
            }
        }
        else
        {
            if (enemySR.flipX == false && enemyForward)
            {
                enemyStateNumber = 2;
                targetPoint = new Vector2(-1 * speed, transform.position.y);
            }
            else if (enemySR.flipX && enemyBackward)
            {
                enemyStateNumber = 2;
                targetPoint = new Vector2(1 * speed, transform.position.y);
            }
        }
    }

    /// <summary>
    /// Метод, содержащий разного рода данные для дебага.
    /// </summary>
    private void DebugStuff()
    {
        Debug.Log(enemyStateNumber);
        //Debug.Log(goToPosition);
        Debug.DrawLine(transform.position, targetPoint, Color.green);
        //Debug.Log($"Ground detected: {groundDetected}");
        Debug.DrawRay(enemyAttackRangeTrigger.transform.position, new Vector2(0.4f, -0.65f) * voidRayDistance, Color.cyan);
        Debug.DrawRay(enemyAttackRangeTrigger.transform.position, new Vector2(-0.4f, -0.65f) * voidRayDistance, Color.cyan);
        Debug.DrawRay(new Vector2(enemyAttackRangeTrigger.transform.position.x + (enemyAttackRangeTrigger.size.x / 2), 
                                  enemyAttackRangeTrigger.transform.position.y), 
                                  Vector2.right * sidesRayDistance, Color.red);
        Debug.DrawRay(new Vector2(enemyAttackRangeTrigger.transform.position.x - (enemyAttackRangeTrigger.size.x / 2), 
                                  enemyAttackRangeTrigger.transform.position.y), 
                                  Vector2.left * sidesRayDistance, Color.red);
        Debug.DrawLine(transform.position, targetPoint, Color.blue);
        //Debug.Log($"Player in range: {playerInRange}");
        Debug.Log($"Wall Detected: {wallDetected}");
    }
    #endregion
}
