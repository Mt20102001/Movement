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
        public bool IsLanding { get; private set; }
        public bool IsCrouching { get; private set; }
        public bool IsJumping { get; private set; }
        public DataMovementPlayer Configs => dataPlayer;

        [Header("Requirements")]
        [SerializeField] private DataMovementPlayer dataPlayer;
        [SerializeField] private Transform mainCamera;

        private CharacterController character;

        private float currentDelayStartJump;
        private float currentDelayLanding;
        private float verticalVelocity;

        private Vector3 cacheMoveDirection;
        private bool wasGround;
        private bool isGrounded;

        public void Initialize()
        {
            character = GetComponent<CharacterController>();
            isGrounded = true;
            wasGround = isGrounded;
            IsInitialized = true;
        }

        public void CustomUpdate(float tick)
        {
            if (!IsInitialized) return;

            IsOnAir = !character.isGrounded;
            isGrounded = !IsOnAir;

            if (isGrounded)
            {
                if (verticalVelocity < 0f)
                    verticalVelocity = -2f;
            }
            else
            {
                verticalVelocity += dataPlayer.Gravity * tick;
            }

            if (currentDelayStartJump > 0f)
            {
                currentDelayStartJump -= tick;
                if (currentDelayStartJump <= 0f)
                {
                    verticalVelocity = dataPlayer.JumpForce;
                    IsJumping = true;
                }
            }

            if (currentDelayLanding > 0f)
            {
                currentDelayLanding -= tick;
                if (currentDelayLanding <= 0f)
                {
                    currentDelayLanding = 0f;
                    IsLanding = false;
                }
            }

            if (IsLanding)
            {
                CurrentSpeed = Mathf.Lerp(CurrentSpeed, 0f, tick * dataPlayer.Duration);
                character.Move(Vector3.up * verticalVelocity * tick);
                IsOnAir = !character.isGrounded;
                return;
            }

            if (!IsCrouching)
                CurrentSpeed = IsMoving
                    ? Mathf.Lerp(CurrentSpeed, IsRunning ? dataPlayer.RunSpeed : dataPlayer.WalkSpeed, tick * dataPlayer.Duration)
                    : Mathf.Lerp(CurrentSpeed, 0, tick * dataPlayer.Duration);
            else
                CurrentSpeed = IsMoving
                    ? Mathf.Lerp(CurrentSpeed, dataPlayer.MoveCrouchSpeed, tick * dataPlayer.Duration)
                    : Mathf.Lerp(CurrentSpeed, 0, tick * dataPlayer.Duration);

            Vector3 finalMove =
                cacheMoveDirection.normalized * CurrentSpeed +
                Vector3.up * verticalVelocity;

            character.Move(finalMove * tick);

            if (IsMoving && !IsLanding)
            {
                transform.rotation = Quaternion.Lerp(
                    transform.rotation,
                    Quaternion.LookRotation(cacheMoveDirection),
                    tick * dataPlayer.RotationSpeed
                );
            }

            if (wasGround != isGrounded)
            {
                if (wasGround && !isGrounded)
                {
                    IsJumping = false;
                }
                else if (!wasGround && isGrounded)
                {
                    IsLanding = true;
                    currentDelayLanding = dataPlayer.LandingTimeSec;
                }

                wasGround = isGrounded;
            }
        }

        public void Move(Vector2 inputMove, bool inputRun)
        {
            CurrentDir = new Vector3(inputMove.x, 0f, inputMove.y).normalized;
            IsMoving = CurrentDir.sqrMagnitude > 0.01f;
            IsRunning = IsMoving && inputRun && !IsCrouching;

            if (IsMoving)
            {
                float targetAngle =
                    Mathf.Atan2(CurrentDir.x, CurrentDir.z) * Mathf.Rad2Deg
                    + mainCamera.eulerAngles.y;

                cacheMoveDirection =
                    Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            }
            else
            {
                cacheMoveDirection = Vector3.zero;
            }
        }

        public void Crouch()
        {
            if (IsJumping || IsOnAir || IsLanding) return;

            IsCrouching = !IsCrouching;

            character.height = IsCrouching ? dataPlayer.CrouchHeight : dataPlayer.NormalHeight;
            character.center = new Vector3(0f, character.height / 2f, 0f);
        }

        public void ForceCrouchState(bool isCrouching)
        {
            IsCrouching = isCrouching;
            character.height = IsCrouching ? dataPlayer.CrouchHeight : dataPlayer.NormalHeight;
            character.center = new Vector3(0f, character.height / 2f, 0f);
        }

        public void Jump()
        {
            if (!isGrounded || IsLanding) return;
            currentDelayStartJump = dataPlayer.StartJumpUpTimeSec;
        }
    }
}
