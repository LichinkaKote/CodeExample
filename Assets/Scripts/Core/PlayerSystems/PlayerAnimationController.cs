using Assets.Scripts.Core.Animators;
using Assets.Scripts.Core.Input;
using System;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Core.PlayerSystems
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private const string IDLE = "Idle";
        private const string MOVE = "Walk";
        private const string MOVE_BACK = "WalkBack";
        private const string SHOOT = "Fire";
        private SpineAnimator animator;
        private Stats stats;
        private LevelInputListener levelInputListener;
        private WeaponController weaponController;
        private Vector3 direction => levelInputListener.AxisInput;
        private float speed => stats.MoveSpeed;
        private Vector2 Target => weaponController.Target;
        private bool isAimToRight;

        private MoveState moveState;
        private bool isLookRight;
        private string currentAnim = IDLE;
        private enum MoveState
        {
            Idle,
            MoveLeft,
            MoveRight,
            UpORDown
        }
        private void Awake()
        {
            enabled = false;
        }
        public void Init(SpineAnimator animator, Stats stats, LevelInputListener levelInputListener, WeaponController weaponController)
        {
            this.animator = animator;
            this.stats = stats;
            this.levelInputListener = levelInputListener;
            this.weaponController = weaponController;
            weaponController.attack += Attack;
            enabled = true;
        }

        private void Attack()
        {
            animator.PlayParralel(1, SHOOT, false);
        }

        private void Update()
        {
            UpadateAnimation();
        }

        private void UpadateAnimation()
        {
            bool isIdle = direction.x == 0f && direction.y == 0f;
            MoveState newMoveState = MoveState.UpORDown;
            if (isIdle) { newMoveState = MoveState.Idle; }
            else
            {
                if (direction.x > 0) newMoveState = MoveState.MoveRight;
                else if (direction.x < 0) newMoveState = MoveState.MoveLeft;
            }

            bool moveStateChanged = moveState != newMoveState;
            if (moveStateChanged) moveState = newMoveState;

            isAimToRight = ((Vector3)Target - transform.position).x > 0f;
            bool lookDirChanged = isLookRight != isAimToRight;
            if (lookDirChanged) isLookRight = isAimToRight;

            if (moveStateChanged || lookDirChanged)
            {
                switch (moveState)
                {
                    case MoveState.Idle:
                        currentAnim = IDLE;
                        break;
                    case MoveState.MoveLeft:
                        currentAnim = isLookRight ? MOVE_BACK : MOVE;
                        break;
                    case MoveState.MoveRight:
                        currentAnim = isLookRight ? MOVE : MOVE_BACK;
                        break;
                    case MoveState.UpORDown:
                        currentAnim = MOVE;
                        break;
                }
                animator.Play(currentAnim, isLookRight, animSpeed: speed);
            }
        }

        private void OnDestroy()
        {
            weaponController.attack -= Attack;
        }
    }
}