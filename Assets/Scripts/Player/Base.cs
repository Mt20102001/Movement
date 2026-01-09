using UnityEngine;

namespace PlayerControllers
{
    public interface IController
    {
        public void Initialize();
        public void CustomUpdate(float tick);
    }


    public interface IMovement : IController
    {
        public DataMovementPlayer Configs { get; }
        public bool IsInitialized { get; }
        public Vector3 CurrentDir { get; }
        public float CurrentSpeed { get; }
        public bool IsRunning { get; }
        public bool IsMoving { get; }
        public bool IsOnAir { get; }
        public bool IsLanding { get; }
        public bool IsCrouching { get; }
        public bool IsJumping { get; }
    }
}