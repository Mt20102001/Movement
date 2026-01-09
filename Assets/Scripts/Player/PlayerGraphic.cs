using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerGraphic : MonoBehaviour
{
    private PlayerController controller;
    private Animator animator;
    private float currentMoveParam;
    private float currentRunParam;
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


        float moveParam = animator.GetFloat("IsMoving");
        currentMoveParam = Mathf.Lerp(moveParam, controller.Movement.IsMoving ? 1 : 0, duration);
        animator.SetFloat("IsMoving", currentMoveParam);

        float runParam = animator.GetFloat("IsRunning");
        currentRunParam = Mathf.Lerp(runParam, controller.Movement.IsRunning ? 1 : 0, duration);
        animator.SetFloat("IsRunning", currentRunParam);
    }
}
