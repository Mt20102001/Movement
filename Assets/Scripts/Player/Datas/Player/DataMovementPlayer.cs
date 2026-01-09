using UnityEngine;

namespace PlayerControllers
{
    [CreateAssetMenu(fileName = "Config Player", menuName = "Datas/Player/Config Player", order = 1)]
    public class DataMovementPlayer : ScriptableObject
    {
        public float MoveCrouchSpeed;
        public float WalkSpeed;
        public float RunSpeed;
        public float RotationSpeed;
        public float JumpForce;
        public float NormalHeight;
        public float CrouchHeight;
        public float Duration;
        public float StartJumpUpTimeSec;
        public float LandingTimeSec;
        public float Gravity;
    }
}