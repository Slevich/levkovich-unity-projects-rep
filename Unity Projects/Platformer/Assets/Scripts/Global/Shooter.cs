using UnityEngine;

namespace Platformer.Inputs
{
    public class Shooter : MonoBehaviour
    {
        [Header("GameObject which fly when player shoots")]
        [SerializeField] private GameObject bullet;

        [Header("Speed of the flying of the bullet")]
        [SerializeField] private float fireSpeed;

        [Header("Places from which bullet starts to fly")]
        [SerializeField] private Transform firePointLeft;
        [SerializeField] private Transform firePointRight;

        [Header("Parametr which uses in animator during player shoot")]
        [SerializeField] private string animationParametr;

        //Аниматор стрелка.
        private Animator fireHolderAnim;

        //Sprite Renderer стрелка для отслеживания повернут ли он.
        private SpriteRenderer fireHolderSR;

        //Расположение места, откуда полетит снаряд.
        private Transform currentFirePoint;

        //Горизонтальное направление полета снаряда.
        private float horizontalDirection;

        //Предыдущее положение персонажа, для стрельбы с места (повернут был или нет).
        private float previousDirection;

        private void Awake()
        {
            fireHolderAnim = GetComponent<Animator>();
            fireHolderSR = GetComponent<SpriteRenderer>();
            currentFirePoint = firePointLeft;
        }

        public void stopFireAnimation()
        {
            fireHolderAnim.SetBool(animationParametr, false);
        }

        public void playFireAnimation()
        {
            fireHolderAnim.SetBool(animationParametr, true);
        }

        private void Update()
        {
            horizontalDirection = Input.GetAxis(GlobalStringVars.HORIZONTAL_AXIS);

            if (fireHolderSR.flipX == true)
            {
                currentFirePoint = firePointRight;
                previousDirection = -1;
            }
            else
            {
                currentFirePoint = firePointLeft;
                previousDirection = 1;
            }
        }

        public void Shoot()
        {
            GameObject currentBullet = Instantiate(bullet, currentFirePoint.position, Quaternion.identity);
            Rigidbody2D currentBulletVelocity = currentBullet.GetComponent<Rigidbody2D>();

            if (horizontalDirection >= 0)
            {
                currentBulletVelocity.velocity = new Vector2(fireSpeed * previousDirection, currentBulletVelocity.velocity.y);
            }
            else
            {
                currentBulletVelocity.velocity = new Vector2(fireSpeed * previousDirection, currentBulletVelocity.velocity.y);
            }
        }
    }
}
