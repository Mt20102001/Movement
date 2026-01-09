using UnityEngine;

namespace PlayerControllers
{
    [CreateAssetMenu(fileName = "Config Player", menuName = "Datas/Player/Config Player", order = 1)]
    public class DataMovementPlayer : ScriptableObject
    {
        public float MinSpeed;
        public float MaxSpeed;
        public float RotationSpeed;
        public float JumpForce;
        public float NormalHeight;
        public float CrouchHeight;
    }
}