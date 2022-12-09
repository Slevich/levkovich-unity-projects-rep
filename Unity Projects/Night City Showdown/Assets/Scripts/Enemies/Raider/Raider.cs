using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Raider : MonoBehaviour
{
    #region Переменные
    [Header("Borders of waiting until moving.")]
    [SerializeField] private float minWaitBorder;
    [SerializeField] private float maxWaitBorder;
    [Header("Speed of movement.")]
    [SerializeField] private float speed;
    [Header("The amount of time that passes between attacks.")]
    [SerializeField] private float attackInterval;
    [Header("The speed of the bullet firing.")]
    [SerializeField] private float fireSpeed;
    [Header("The difference in distance along the X-axis within which the enemy has no change in animation position.")]
    [SerializeField] private float distanceDifference;
    [Header("The length of the ray launched into the ground.")]
    [SerializeField] private float groundRayDistance;
    [Header("The length of the ray launched to sides.")]
    [SerializeField] private float sidesRayDistance;
    [Header("The length of the ray launched to down sides.")]
    [SerializeField] private float voidRayDistance;
    [Header("Distance to the point at which the enemy stops.")]
    [SerializeField] private float enemyStopDistance;
    [Header("Points, which player earned for killing enemy.")]
    [SerializeField] private int pointsCost;
    [Header("Layer mask: Ground.")]
    [SerializeField] private LayerMask groundMask;
    [Header("Layer mask: Enemy.")]
    [SerializeField] private LayerMask enemyMask;
    [Header("Game object prefab, which contains death animation VFX.")]
    [SerializeField] private GameObject enemyDeathVFXPrefab;
    [Header("Colliders components in children game objects.")]
    [SerializeField] private CircleCollider2D enemyAttackRangeTrigger;
    [SerializeField] private CircleCollider2D enemyFindingTrigger;
    [SerializeField] private CapsuleCollider2D enemyCheckTrigger;
    [Header("Game object prefab of bullet.")]
    [SerializeField] private GameObject weaponBulletPrefab;
    [Header("Game object prefab, which contains animation with shoot VFX.")]
    [SerializeField] private GameObject shootVFXPrefab;
    [Header("Image, which represents current character's health level.")]
    [SerializeField] private Image HPBar;
    [Header("Transform component of the player game object (Joy).")]
    [SerializeField] private Transform playerTransform;
    [Header("Transform array with all muzzles.")]
    [SerializeField] private Transform[] muzzles;
    [Header("Variables, which reflect some conditions.")]
    public bool playerInRange;
    public bool wallDetected;
    public bool playerDetected;

    //Массив трансформов активных точек вылета пуль.
    private Transform[] activeMuzzles = new Transform[2];
    //Аниматор врага.
    private Animator enemyAnim;
    //Rigidbody врага.
    private Rigidbody2D enemyRB;
    //Компонент здоровья врага.
    private Health enemyHealth;
    //Sprite renderer врага.
    private SpriteRenderer enemySR;
    //Transform, который хранит активную точку, откуда вылетают пули.
    private Vector2 targetPoint;
    //Точка, обозначающая направлени полета обеих пуль.
    private Vector3 bullet1Direction;
    private Vector3 bullet2Direction;
    //Текущий номер стейта врага.
    private float enemyStateNumber;
    //Переменная содержащее в себя количество времени ожидания врага.
    private float waitingTimer;
    //Процент текущего здоровья врага.
    private float enemyHP;
    //Таймер интервала атаки для обнуления.
    private float currentAttackInterval;
    //Направление атаки врага.
    private float attackDirection;
    //Постоянные значения для стейтов врага.
    private const float PATROL_STATE = 0;
    private const float WALK_STATE = 1;
    private const float ATTACK_STATE = 2;
    private const float DEATH_STATE = 3;
    private const float IDLE_STATE = 4;
    //Переменная содержащая значение, сгенерирован ли таймер ожидания.
    private bool timerGenerated;
    //Переменная отражающая, достиг ли враг точки.
    private bool enemyReachPoint;
    //Переменная отражающая, жив ли враг.
    private bool isAlive;
    //Переменная содержащая информацию о том, падает ли враг.
    private bool isFalling;
    //Переменная обозначающая, замечен ли другой враг спереди.
    private bool enemyForward;
    //Переменная обозначающая, замечен ли другой враг сзади.
    private bool enemyBackward;
    //Переменные, отражающие, есть ли земля спереди и сзади.
    private bool isGroundForward;
    private bool isGroundBackward;
    #endregion

    #region Методы
    /// <summary>
    /// На старте получаем компоненты.
    /// Переводим состояния "жив" в true и "падает" в false.
    /// Дефолтный стейт - ожидание (0).
    /// Устанавливаем дефолтное направление атаки на 0 - по середине.
    /// Присваиваем таймер для обнуления значение.
    /// </summary>
    void Start()
    {
        enemyAnim = GetComponent<Animator>();
        enemyRB = GetComponent<Rigidbody2D>();
        enemySR = GetComponent<SpriteRenderer>();
        enemyHealth = GetComponent<Health>();
        isAlive = true;
        isFalling = false;
        enemyStateNumber = 0;
        attackDirection = 0;
        currentAttackInterval = attackInterval;
    }

    /// <summary>
    /// В Update, обновляем необходимые переменные в методах.
    /// Проверяем есть ли земля под врагом.
    /// Проверяем есть ли земля впереди и сзади.
    /// Далее в зависимости от того, жив ли враг,
    /// заметил ли он игрока, находится ли он в радиусе атаки,
    /// выполняются методы.
    /// Постоянно обновляем активные точки, из которых
    /// будут вылетать пули.
    /// Если игрок в радиусе атаки, но он сильно ниже врага,
    /// то враг будет идти в сторону игрока, пока не подойдет к краю платформы.
    /// </summary>
    private void Update()
    {
        //DebugStuff();
        UpdateHP();
        EnemyStates();
        UpdateEnemySpriteFlip();
        GroundCheck();
        EnemyCheck();
        CheckVoid();

        if (isAlive)
        {
            if (playerDetected)
            {
                UpdateActiveMuzzles();

                if (playerInRange)
                {
                    if (playerTransform.position.y < (transform.position.y - 0.5f) && isGroundBackward && isGroundForward)
                    {
                        ChangeEnemyState(1);
                        targetPoint = new Vector2(playerTransform.position.x, transform.position.y);
                    }
                    else
                    {
                        enemyAnim.SetFloat("AttackDirection", attackDirection);
                        
                        if (playerTransform.position.x < transform.position.x)
                        {
                            enemySR.flipX = true;
                        }
                        else
                        {
                            enemySR.flipX = false;
                        }
                        EnemyAttack();
                    }
                }
                else
                {
                    attackInterval = currentAttackInterval;
                    EnemyDetectEnemy();
                    enemyAnim.SetBool("IsAttack", false);

                    if (isGroundBackward == false || isGroundForward == false)
                    {
                        ChangeEnemyState(4);
                        targetPoint = transform.position;
                    }
                    else
                    {
                        ChangeEnemyState(1);
                        targetPoint = new Vector2(playerTransform.position.x, transform.position.y);
                    }
                }
            }
            else
            {
                enemyAnim.SetBool("IsAttack", false);
                EnemyDetectEnemy();
                DetectVoid();
                EnemyReachPoint();
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

            case WALK_STATE:
                enemyAnim.SetFloat("Speed", 0.3f);
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

            case IDLE_STATE:
                enemyAnim.SetBool("IsAttack", false);
                enemyAnim.SetFloat("Speed", 0f);
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
                if (randomNumber < 0.5f) targetPoint = new Vector2(Random.Range(transform.position.x, transform.position.x + enemyFindingTrigger.radius), transform.position.y);
                else targetPoint = new Vector2(Random.Range(transform.position.x - enemyFindingTrigger.radius, transform.position.x), transform.position.y);
                ChangeEnemyState(1);
                timerGenerated = false;
                wallDetected = false;
            }
            else
            {
                if (enemySR.flipX)
                {
                    targetPoint = new Vector2(Random.Range(transform.position.x + (enemyCheckTrigger.size.x), transform.position.x + enemyFindingTrigger.radius), transform.position.y);
                    ChangeEnemyState(1);
                    timerGenerated = false;
                    wallDetected = false;
                }
                else
                {
                    targetPoint = new Vector2(Random.Range(transform.position.x - (enemyCheckTrigger.size.x), transform.position.x - enemyFindingTrigger.radius), transform.position.y);
                    ChangeEnemyState(1);
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

        if (playerDetected && isFalling == false)
        {
            enemyRB.velocity = movingDirection * speed;
        }
        else if (playerDetected && isFalling)
        {
            enemyRB.velocity = Vector2.down * speed;
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
    /// Метод меняет номер текущего стейта врага.
    /// </summary>
    /// <param name="enemyState"></param>
    private void ChangeEnemyState(int enemyState)
    {
        enemyStateNumber = enemyState;
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
    /// Метод кидает луч в землю, проверяя
    /// стоит ли сейчас враг на земле или нет.
    /// Переключает соответствующие переменные
    /// и передает их в аниматор.
    /// </summary>
    private void GroundCheck()
    {
        if (Physics2D.Raycast(transform.position, Vector2.down, groundRayDistance, groundMask))
        {
            isFalling = false;
            enemyAnim.SetBool("IsFalling", isFalling);
        }
        else
        {
            isFalling = true;
            enemyAnim.SetBool("IsFalling", isFalling);
        }
    }

    /// <summary>
    /// Метод поворачивает спрайт врага,
    /// в зависимости от направления его движения.
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
    /// Метод кидает два луча - вперед и назад,
    /// проверяя тем самым, попал ли он во врага.
    /// </summary>
    private void EnemyCheck()
    {
        enemyForward = Physics2D.Raycast(new Vector2(enemyAttackRangeTrigger.transform.position.x + (enemyCheckTrigger.size.x / 2), enemyAttackRangeTrigger.transform.position.y), Vector2.right, sidesRayDistance, enemyMask);
        enemyBackward = Physics2D.Raycast(new Vector2(enemyAttackRangeTrigger.transform.position.x - (enemyCheckTrigger.size.x / 2), enemyAttackRangeTrigger.transform.position.y), Vector2.left, sidesRayDistance, enemyMask);
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
                enemyStateNumber = 4;
            }
            else if (enemySR.flipX && enemyBackward)
            {
                enemyStateNumber = 4;
            }
            else
            {
                targetPoint = new Vector2(playerTransform.position.x, transform.position.y);
                enemyStateNumber = 1;
            }
        }
        else
        {
            if (enemySR.flipX == false && enemyForward)
            {
                enemyStateNumber = 1;
                targetPoint = new Vector2(-1 * speed, transform.position.y);
            }
            else if (enemySR.flipX && enemyBackward)
            {
                enemyStateNumber = 1;
                targetPoint = new Vector2(1 * speed, transform.position.y);
            }
        }
    }

    /// <summary>
    /// Метод передает в направление полета пули -
    /// весторы right, чтобы из точек они летели прямо.
    /// </summary>
    private void CalculateBulletDirections()
    {
        bullet1Direction = activeMuzzles[0].right;
        bullet2Direction = activeMuzzles[1].right;
    }

    /// <summary>
    /// Метод передает в качестве активных точек вылета пули
    /// местоположения объектов, в зависимости от местоположения игрока.
    /// Если он намного выше врага, берутся верхние точки.
    /// Если на одной уровне - средние точки.
    /// Если сильно ниже - нижние точки.
    /// </summary>
    private void UpdateActiveMuzzles()
    {
        if (enemySR.flipX)
        {
            if ((playerTransform.position.y < (transform.position.y + distanceDifference)) && (playerTransform.position.y > (transform.position.y - distanceDifference)))
            {
                activeMuzzles[0] = muzzles[8];
                activeMuzzles[1] = muzzles[9];
                attackDirection = 0;
            }
            else if (playerTransform.position.y < (transform.position.y - distanceDifference))
            {
                activeMuzzles[0] = muzzles[6];
                activeMuzzles[1] = muzzles[7];
                attackDirection = -1;
            }
            else if (playerTransform.position.y > (transform.position.y + distanceDifference))
            {
                activeMuzzles[0] = muzzles[10];
                activeMuzzles[1] = muzzles[11];
                attackDirection = 1;
            }
        }
        else
        {
            if ((playerTransform.position.y < (transform.position.y + distanceDifference)) && (playerTransform.position.y > (transform.position.y - distanceDifference)))
            {
                activeMuzzles[0] = muzzles[2];
                activeMuzzles[1] = muzzles[3];
                attackDirection = 0;
            }
            else if (playerTransform.position.y < (transform.position.y - distanceDifference))
            {
                activeMuzzles[0] = muzzles[0];
                activeMuzzles[1] = muzzles[1];
                attackDirection = -1;
            }
            else if (playerTransform.position.y > (transform.position.y + distanceDifference))
            {
                activeMuzzles[0] = muzzles[4];
                activeMuzzles[1] = muzzles[5];
                attackDirection = 1;
            }
        }
    }

    /// <summary>
    /// Метод переключает состояние врага в ожидание (4).
    /// Запускает таймер, по результатам которого стейт
    /// переключается в атаку (2).
    /// </summary>
    private void EnemyAttack()
    {
        ChangeEnemyState(4);
        attackInterval -= Time.deltaTime;

        if (attackInterval <= 0)
        {
            ChangeEnemyState(2);
        }
    }

    /// <summary>
    /// Метод спавнит две пули,
    /// и они летят в направлении игрока.
    /// </summary>
    public void ShootBullets()
    {
        GameObject shootVFX1 = Instantiate(shootVFXPrefab, activeMuzzles[0].position, activeMuzzles[0].rotation);
        GameObject currentBullet1 = Instantiate(weaponBulletPrefab, activeMuzzles[0].position, activeMuzzles[0].rotation);
        Rigidbody2D currentBullet1RB = currentBullet1.GetComponent<Rigidbody2D>();
        GameObject shootVFX2 = Instantiate(shootVFXPrefab, activeMuzzles[1].position, activeMuzzles[1].rotation);
        GameObject currentBullet2 = Instantiate(weaponBulletPrefab, activeMuzzles[1].position, activeMuzzles[1].rotation);
        Rigidbody2D currentBullet2RB = currentBullet2.GetComponent<Rigidbody2D>();
        CalculateBulletDirections();
        currentBullet1RB.velocity = bullet1Direction * fireSpeed;
        currentBullet2RB.velocity = bullet2Direction * fireSpeed;
    }

    /// <summary>
    /// Метод обнуляет интервал атаки.
    /// </summary>
    public void EndShootAnim()
    {
        attackInterval = currentAttackInterval;
    }


    /// <summary>
    /// Метод кидает два RayCast'а, которые проверяют,
    /// есть ли впереди врага земля или нет.
    /// </summary>
    private void CheckVoid()
    {
        isGroundForward = Physics2D.Raycast(enemyCheckTrigger.transform.position, new Vector2(0.4f, -0.65f), voidRayDistance, groundMask);
        isGroundBackward = Physics2D.Raycast(enemyCheckTrigger.transform.position, new Vector2(-0.4f, -0.65f), voidRayDistance, groundMask);
    }

    /// <summary>
    /// Метод проверяет, есть ли земля спереди или сзади, а также повернут ли спрайт врага.
    /// При этом, точка к которой идет враг, меняется на новую в противоположной от пустоты направлении.
    /// </summary>
    private void DetectVoid()
    {
        if (isGroundForward == false && enemySR.flipX == false)
        {
            targetPoint = new Vector2((transform.position.x - enemyCheckTrigger.size.x), transform.position.y);
        }
        else if (isGroundBackward == false && enemySR.flipX)
        {
            targetPoint = new Vector2((transform.position.x + enemyCheckTrigger.size.x), transform.position.y);
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
        GetComponent<SpawnObjectOnDeath>().SpawnObject();
        Destroy(gameObject);
    }

    /// <summary>
    /// Метод, содержащий разного рода данные для дебага.
    /// </summary>
    private void DebugStuff()
    {
        //Debug.DrawRay(transform.position, Vector2.down * groundRayDistance, Color.green);
        //Debug.Log($"Player In Range: {playerInRange}");
        Debug.Log($"Player Detected: {playerDetected}");
        Debug.Log($"Is falling: {isFalling}");
        //Debug.Log($"Wall Detecte: {wallDetected}");
        Debug.Log($"Enemy State number: {enemyStateNumber}");
        //Debug.Log($"Enemy reach point: {enemyReachPoint}");
        //Debug.Log(waitingTimer);
        Debug.DrawRay(new Vector2(enemyAttackRangeTrigger.transform.position.x + (enemyCheckTrigger.size.x / 2), enemyAttackRangeTrigger.transform.position.y), Vector2.right * sidesRayDistance, Color.red);
        Debug.DrawRay(new Vector2(enemyAttackRangeTrigger.transform.position.x - (enemyCheckTrigger.size.x / 2), enemyAttackRangeTrigger.transform.position.y), Vector2.left * sidesRayDistance, Color.red);
        Debug.DrawRay(enemyCheckTrigger.transform.position, new Vector2(0.4f, -0.65f) * voidRayDistance, Color.cyan);
        Debug.DrawRay(enemyCheckTrigger.transform.position, new Vector2(-0.4f, -0.65f) * voidRayDistance, Color.cyan);
        Debug.DrawLine(transform.position, targetPoint, Color.red);
    }
    #endregion
}
