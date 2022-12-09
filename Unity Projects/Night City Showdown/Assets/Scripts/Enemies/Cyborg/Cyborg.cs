using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cyborg : MonoBehaviour
{
    #region Переменные
    [Header("Borders of waiting until moving.")]
    [SerializeField] private float minWaitBorder;
    [SerializeField] private float maxWaitBorder;
    [Header("Speed of movement.")]
    [SerializeField] private float speed;
    [Header("The amount of time that passes between the release of a new bullet.")]
    [SerializeField] private float attackFireRate;
    [Header("The amount of time that passes between attacks.")]
    [SerializeField] private float attackInterval;
    [Header("The number of bullets fired in a single attack.")]
    [SerializeField] private int bulletsPerAttack;
    [Header("The speed of the bullet firing.")]
    [SerializeField] private float fireSpeed;
    [Header("The length of the ray launched into the ground.")]
    [SerializeField] private float groundRayDistance;
    [Header("The length of the ray launched to sides.")]
    [SerializeField] private float voidRayDistance;
    [Header("Distance to the point at which the enemy stops.")]
    [SerializeField] private float enemyStopDistance;
    [Header("The value at which the enemy's aim is raised, relative to the player's transform.")]
    [SerializeField] private float lookOffset;
    [Header("Points, which player earned for killing enemy.")]
    [SerializeField] private int pointsCost;
    [Header("Layer mask: Ground.")]
    [SerializeField] private LayerMask groundMask;
    [Header("Game object prefab, which contains death animation VFX.")]
    [SerializeField] private GameObject enemyDeathVFXPrefab;
    [Header("Colliders components in children game objects.")]
    [SerializeField] private CircleCollider2D enemyAttackRangeTrigger;
    [SerializeField] private CircleCollider2D enemyFindingTrigger;
    [SerializeField] private CircleCollider2D enemyCheckTrigger;
    [Header("Game objects, which contains weapon hands.")]
    [SerializeField] private GameObject weaponHands;
    [Header("Game object with forward weapon hands.")]
    [SerializeField] private GameObject weaponForwardHands;
    [Header("Game object with backward weapon hands.")]
    [SerializeField] private GameObject weaponBackwardHands;
    [Header("Game object prefab of bullet.")]
    [SerializeField] private GameObject weaponBulletPrefab;
    [Header("Game object prefab, which contains animation with shoot VFX.")]
    [SerializeField] private GameObject shootVFXPrefab;
    [Header("Transform components of game objects with muzzles positions.")]
    [SerializeField] private Transform weaponForwardMuzzleflashPosition;
    [SerializeField] private Transform weaponBackwardMuzzleflashPosition;
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
    //Transform, который хранит активную точку, откуда вылетают пули.
    private Transform activeMuzzle;
    //Точка, к которой направляется враг.
    private Vector2 targetPoint;
    //Точка, содержащее направление полета пули.
    private Vector2 bulletDirection;
    //Текущий номер стейта врага.
    private int enemyStateNumber;
    //Переменная содержащее в себя количество времени ожидания врага.
    private float waitingTimer;
    //Процент текущего здоровья врага.
    private float enemyHP;
    //Таймер интервала атаки для обнуления.
    private float currentAttackInterval;
    //Таймер темпа огня для обнуления.
    private float currentFireRate;
    //Количество пуль в атаке для обнуления.
    private int currentBulletsPerAttack;
    //Постоянные значения для стейтов врага.
    private const int PATROL_STATE = 0;
    private const int IDLE_STATE = 1;
    private const int WALK_STATE = 2;
    private const int ATTACK_STATE = 3;
    private const int DEATH_STATE = 4;
    //Переменная содержащая значение, сгенерирован ли таймер ожидания.
    private bool timerGenerated;
    //Переменная отражающая, достиг ли враг точки.
    private bool enemyReachPoint;
    //Переменная отражающая, жив ли враг.
    private bool isAlive;
    //Переменная содержащая информацию о том, падает ли враг.
    private bool isFalling;
    //Переменные, отражающие, есть ли земля спереди и сзади.
    private bool isGroundForward;
    private bool isGroundBackward;
    #endregion

    #region Методы
    /// <summary>
    /// На старте получаем компоненты.
    /// Переводим состояния "жив" в true и "падает" в false.
    /// Дефолтный стейт - ожидание (0).
    /// Присваиваем переменным для обнуления значения.
    /// </summary>
    void Start()
    {
        enemyAnim = GetComponent<Animator>();
        enemyRB = GetComponent<Rigidbody2D>();
        enemySR = GetComponent<SpriteRenderer>();
        isAlive = true;
        isFalling = false;
        enemyHealth = GetComponent<Health>();
        enemyStateNumber = 0;
        currentAttackInterval = attackInterval;
        currentFireRate = attackFireRate;
        currentBulletsPerAttack = bulletsPerAttack;
    }

    /// <summary>
    /// В Update, обновляем необходимые переменные в методах.
    /// Проверяем есть ли земля под врагом.
    /// Проверяем есть ли земля впереди и сзади.
    /// Далее в зависимости от того, жив ли враг,
    /// заметил ли он игрока, находится ли он в радиусе атаки,
    /// выполняются методы.
    /// </summary>
    private void Update()
    {
        //DebugStuff();
        UpdateHP();
        UpdateActiveMuzzle();
        EnemyStates();
        UpdateEnemySpriteFlip();
        GroundCheck();
        CheckVoid();

        if (isAlive)
        {
            if (playerDetected)
            {
                if (playerInRange)
                {
                    if (playerTransform.position.y < (transform.position.y - 0.5f) && isGroundBackward && isGroundForward)
                    {
                        ChangeEnemyState(2);
                        targetPoint = new Vector2(playerTransform.position.x, transform.position.y);
                    }
                    else
                    {
                        enemyRB.velocity = Vector2.zero;

                        if (playerTransform.position.x < transform.position.x)
                        {
                            enemySR.flipX = true;
                        }
                        else
                        {
                            enemySR.flipX = false;
                        }

                        ChangeEnemyState(1);
                        AttackPlayer();
                    }
                }
                else
                {
                    attackInterval = currentAttackInterval;
                    attackFireRate = currentFireRate;

                    if (playerTransform.position.x < transform.position.x)
                    {
                        enemySR.flipX = true;
                    }
                    else
                    {
                        enemySR.flipX = false;
                    }

                    if (isGroundBackward == false || isGroundForward == false)
                    {
                        ChangeEnemyState(1);
                        targetPoint = transform.position;
                    }
                    else
                    {
                        ChangeEnemyState(2);
                        targetPoint = new Vector2(playerTransform.position.x, transform.position.y);
                    }
                }
            }
            else
            {
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
    /// Метод, переключающий стейты врага
    /// в зависимости от его номера.
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
                enemyAnim.SetFloat("Speed", 0.3f);
                EnemyGoesToPosition();
                break;

            case ATTACK_STATE:
                enemyRB.velocity = Vector2.zero;
                break;

            case DEATH_STATE:
                weaponHands.SetActive(false);
                enemyRB.velocity = Vector2.zero;
                enemyAnim.SetBool("IsDead", true);
                break;
        }
    }

    /// <summary>
    /// Метод генерирует рандомную точку, в зависимости
    /// от того, уперся ли враг в стену или нет.
    /// Если да - точка формируется в потивоположной стороне
    /// от столкновения.
    /// Если нет - точка формируется спереди или сзади от врага.
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
                ChangeEnemyState(2);
                timerGenerated = false;
                wallDetected = false;
            }
            else
            {
                if (enemySR.flipX)
                {
                    targetPoint = new Vector2(Random.Range(transform.position.x + (enemyCheckTrigger.radius * 2), transform.position.x + enemyFindingTrigger.radius), transform.position.y);
                    ChangeEnemyState(2);
                    timerGenerated = false;
                    wallDetected = false;
                }
                else
                {
                    targetPoint = new Vector2(Random.Range(transform.position.x - (enemyFindingTrigger.radius * 2), transform.position.x - enemyCheckTrigger.radius), transform.position.y);
                    ChangeEnemyState(2);
                    timerGenerated = false;
                    wallDetected = false;
                }
            }
        }
    }

    /// <summary>
    /// Метод рассчитывает направление движения врага.
    /// В зависимости от того, замечен ли игрок,
    /// передает его направление движения.
    /// Если игрок не замечен, враг идет к рандомной точке.
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

            if (EnemyReachPoint() || wallDetected)
            {
                ChangeEnemyState(0);
            }
        }
    }

    /// <summary>
    /// Метод меняет номер стейта врага.
    /// </summary>
    /// <param name="enemyState"></param>
    private void ChangeEnemyState(int enemyState)
    {
        enemyStateNumber = enemyState;
    }

    /// <summary>
    /// Метод рассчитывает расстояние между
    /// врагом и точкой, возращает значение переменной типа bool.
    /// </summary>
    /// <returns></returns>
    private bool EnemyReachPoint()
    {
        if (Vector2.Distance(transform.position, targetPoint) < enemyStopDistance)
        {
           return enemyReachPoint = true;
        }
        else
        {
           return enemyReachPoint = false;
        }
    }

    /// <summary>
    /// Метод кидает луч в землю,
    /// проверяет находится ли враг
    /// на земле или нет.
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
    /// в зависимости от направления движения.
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
    /// Метод меняет активную точку выстрела пули
    /// в зависимости от поворота спрайта.
    /// Также меняются в зависимости от этого руки с оружием.
    /// </summary>
    private void UpdateActiveMuzzle()
    {
        if (enemySR.flipX)
        {
            activeMuzzle = weaponBackwardMuzzleflashPosition;
            weaponForwardHands.SetActive(false);
            weaponBackwardHands.SetActive(true);
        }
        else
        {
            activeMuzzle = weaponForwardMuzzleflashPosition;
            weaponBackwardHands.SetActive(false);
            weaponForwardHands.SetActive(true);
        }
    }

    /// <summary>
    /// Метод рассчитывает направление полета пули.
    /// </summary>
    private void CalculateBulletDirection()
    {
        Vector3 targetPosition = new Vector3(playerTransform.position.x, playerTransform.position.y + lookOffset, 0);
        Vector3 heading = targetPosition - activeMuzzle.position;
        float bulletDistance = heading.magnitude;
        bulletDirection = heading / bulletDistance;
    }

    /// <summary>
    /// Метод делает так, чтобы оружие с руками смотрело на игрока.
    /// Также запускается таймер атаки, по истечения которого
    /// запускается атака врага.
    /// Запускается еще один таймер, по истечению которого спавнится пуля,
    /// летящая в направлении игрока. После спавна пули, таймер темпа огня
    /// запускается заново и так далее, пока враг не выстрелит нужное количество
    /// пуль за одну атаку.
    /// </summary>
    private void AttackPlayer()
    {
        if (weaponForwardHands.activeInHierarchy)
        {
            weaponForwardHands.transform.LookAt(new Vector2(playerTransform.position.x, playerTransform.position.y + lookOffset));
        }
        else
        {
            weaponBackwardHands.transform.LookAt(new Vector2(playerTransform.position.x, playerTransform.position.y + lookOffset));
        }

        attackInterval -= Time.deltaTime;
        
        if (attackInterval <= 0)
        {
            attackFireRate -= Time.deltaTime;

            if (attackFireRate <= 0 && bulletsPerAttack > 0)
            {
                GameObject shootVFX = Instantiate(shootVFXPrefab, activeMuzzle.position, activeMuzzle.rotation);
                GameObject currentBullet = Instantiate(weaponBulletPrefab, activeMuzzle.position, activeMuzzle.rotation);
                Rigidbody2D currentBulletRB = currentBullet.GetComponent<Rigidbody2D>();
                CalculateBulletDirection();
                currentBulletRB.velocity = bulletDirection * fireSpeed;
                bulletsPerAttack--;
                attackFireRate = currentFireRate;
            }
            else if (bulletsPerAttack <= 0)
            {
                bulletsPerAttack = currentBulletsPerAttack;
                attackInterval = currentAttackInterval;
            }
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
            targetPoint = new Vector2((transform.position.x - enemyCheckTrigger.radius), transform.position.y);
        }
        else if (isGroundBackward == false && enemySR.flipX)
        {
            targetPoint = new Vector2((transform.position.x + enemyCheckTrigger.radius), transform.position.y);
        }
    }

    /// <summary>
    /// Метод, содержащий разного рода данные для дебага.
    /// </summary>
    private void DebugStuff()
    {
        //Debug.DrawRay(transform.position, Vector2.down * groundRayDistance, Color.green);
        //Debug.Log($"Player In Range: {playerInRange}");
        //Debug.Log($"Player Detected: {playerDetected}");
        //Debug.Log($"Wall Detecte: {wallDetected}");
        //Debug.Log($"Enemy State number: {enemyStateNumber}");
        //Debug.Log($"Enemy reach point: {enemyReachPoint}");
        ///Debug.Log(waitingTimer);
        Debug.DrawRay(enemyCheckTrigger.transform.position, new Vector2(0.4f, -0.65f) * voidRayDistance, Color.cyan);
        Debug.DrawRay(enemyCheckTrigger.transform.position, new Vector2(-0.4f, -0.65f) * voidRayDistance, Color.cyan);
        Debug.Log($"IsGroundForward: {isGroundForward}");
        Debug.Log($"IsGroundBackward: {isGroundBackward}");
        Debug.DrawLine(transform.position, targetPoint, Color.red);
    }
    #endregion
}

