using UnityEngine;

namespace Platformer.Inputs
{
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(Shooter))]
    public class PlayerInput : MonoBehaviour
    {
        public bool inputIsActive;

        //Компонент со скриптом "Player Movement".
        private PlayerMovement playerMovement;

        //Компонент со скриптом "Shooter".
        private Shooter shooter;

        //Компонент со скриптом "Melee Attack".
        private MeleeAttack meleeAttack;

        private void Awake()
        {
            playerMovement = GetComponent<PlayerMovement>();
            shooter = GetComponent<Shooter>();
            meleeAttack = GetComponent<MeleeAttack>();
            inputIsActive = true;
        }

        private void Update()
        {
            if (inputIsActive)
            {
                float horizontalDirection = Input.GetAxis(GlobalStringVars.HORIZONTAL_AXIS);
                bool isJumpButtonPressed = Input.GetButtonDown(GlobalStringVars.JUMP_BUTTON);

                if (Input.GetButtonDown(GlobalStringVars.SHOOT_BUTTON))
                {
                    shooter.playFireAnimation();
                }

                if (Input.GetButtonDown(GlobalStringVars.HIT_BUTTON))
                {
                    meleeAttack.PlayAttackAnimation();
                }
                else if (Input.GetButtonUp(GlobalStringVars.HIT_BUTTON))
                {
                    meleeAttack.StopAttackAnimation();
                }

                playerMovement.MoveCharacter(horizontalDirection, isJumpButtonPressed);
            }
        }
    }
}
