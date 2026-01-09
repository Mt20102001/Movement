using UnityEngine;

public class PlayerGraphic : MonoBehaviour
{
    private PlayerController controller;
    private Animator animator;
    private float currentCrouchParam;
    [SerializeField] private float duration = 0.75f;

    void Start()
    {
        controller = GetComponentInParent<PlayerController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller == null)
        {
            Debug.LogError("Cannot Find PlayerController");
            return;
        }

        if (animator == null)
        {
            Debug.LogError("Cannot Find Animator");
            return;
        }


        if (controller.Movement.IsJumping)
        {
            animator.SetTrigger("Jump");
        }

        animator.SetBool("OnAir", controller.Movement.IsOnAir);

        float crouchParam = animator.GetFloat("IsCrouching");
        currentCrouchParam = Mathf.Lerp(crouchParam, controller.Movement.IsCrouching ? 1f : 0f, duration);
        animator.SetFloat("IsCrouching", currentCrouchParam);

        if (!controller.Movement.IsCrouching)
        {
            animator.SetFloat("IsMoving", controller.Movement.CurrentSpeed / controller.Movement.Configs.WalkSpeed);
            animator.SetFloat("IsRunning", controller.Movement.CurrentSpeed / controller.Movement.Configs.RunSpeed);
        }
        else
        {
            animator.SetFloat("IsMoving", controller.Movement.CurrentSpeed / controller.Movement.Configs.MoveCrouchSpeed);
            animator.SetFloat("IsRunning", 0f);
        }
    }
}
