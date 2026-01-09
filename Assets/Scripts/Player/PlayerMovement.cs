using System;
using UnityEngine;

namespace PlayerControllers
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour, IMovement
    {
        public bool IsInitialized { get; private set; }
        public Vector3 CurrentDir { get; private set; }
        public float CurrentSpeed { get; private set; }
        public bool IsRunning { get; private set; }
        public bool IsMoving { get; private set; }
        public bool IsOnAir { get; private set; }
        public bool IsCrouching { get; private set; }
        public bool IsJumping { get; private set; }
        public DataMovementPlayer Configs => dataPlayer;

        [Header("Requirements")]
        [SerializeField] private DataMovementPlayer dataPlayer;
        [SerializeField] private Transform mainCamera;

        private CharacterController character;

        public void Initialize()
        {
            character = GetComponent<CharacterController>();
            IsInitialized = true;
        }


        public void CustomUpdate(float tick)
        {
            if (!IsInitialized) return;

            if (!IsCrouching)
                CurrentSpeed = IsMoving ? Mathf.Lerp(CurrentSpeed, IsRunning ? dataPlayer.RunSpeed : dataPlayer.WalkSpeed, tick * dataPlayer.Duration) : Mathf.Lerp(CurrentSpeed, 0, tick * dataPlayer.Duration);
            else
                CurrentSpeed = IsMoving ? Mathf.Lerp(CurrentSpeed, dataPlayer.MoveCrouchSpeed, tick * dataPlayer.Duration) : Mathf.Lerp(CurrentSpeed, 0, tick * dataPlayer.Duration);


            if (IsMoving)
            {
                character.Move(cacheMoveDirection.normalized * CurrentSpeed * tick);
                transform.rotation = Quaternion.Lerp(
                   transform.rotation,
                   Quaternion.LookRotation(cacheMoveDirection),
                   tick * dataPlayer.RotationSpeed
               );
            }
        }

        Vector3 cacheMoveDirection;
        public void Move(Vector2 inputMove, bool inputRun)
        {
            cacheMoveDirection = inputMove;
            CurrentDir = new Vector3(inputMove.x, 0f, inputMove.y).normalized;
            IsMoving = CurrentDir.magnitude > 0.1f;
            IsRunning = IsMoving && inputRun && !IsCrouching;

            if (IsMoving)
            {
                float targetAngle = Mathf.Atan2(CurrentDir.x, CurrentDir.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
                cacheMoveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            }
        }


        public void Crouch()
        {
            IsCrouching = !IsCrouching;
            if (IsCrouching)
            {
                character.height = dataPlayer.CrouchHeight;
                character.center = new Vector3(0f, dataPlayer.CrouchHeight / 2f, 0f);
            }
            else
            {
                character.height = dataPlayer.NormalHeight;
                character.center = new Vector3(0f, dataPlayer.NormalHeight / 2f, 0f);
            }
        }


        public void ForceCrouchState(bool isCrouching)
        {
            IsCrouching = isCrouching;
            if (IsCrouching)
            {
                character.height = dataPlayer.CrouchHeight;
                character.center = new Vector3(0f, dataPlayer.CrouchHeight / 2f, 0f);
            }
            else
            {
                character.height = dataPlayer.NormalHeight;
                character.center = new Vector3(0f, dataPlayer.NormalHeight / 2f, 0f);
            }
        }
    }
}