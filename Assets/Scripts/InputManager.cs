using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool JumpInput { get; private set; }
    public bool CrouchInput { get; private set; }
    public bool RunInput { get; private set; }
    public bool ThrowGrenadeInput { get; private set; }
    public bool AttackInput { get; private set; }
    public bool StartAttackInput { get; private set; }
    public bool EndAttackInput { get; private set; }
    public bool AimInput { get; private set; }
    public bool ReloadInput { get; private set; }
    public bool QuickChangeWeaponInput { get; private set; }
    public bool ChangeFireModeInput { get; private set; }

    [SerializeField] private float mouseSensitivity = 3f;

    private PlayerInputActions actions;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        actions = new PlayerInputActions();
    }

    void OnEnable()
    {
        actions.Enable();

        actions.Player.Attack.started += _ => StartAttackInput = true;
        actions.Player.Attack.canceled += _ => EndAttackInput = true;
    }

    void OnDisable()
    {
        actions.Disable();
    }

    void Update()
    {
        MoveInput = actions.Player.Move.ReadValue<Vector2>();
        if (MoveInput.sqrMagnitude > 1f)
            MoveInput.Normalize();

        LookInput = actions.Player.Look.ReadValue<Vector2>() * mouseSensitivity;

        AttackInput = actions.Player.Attack.IsPressed();
        // AimInput = actions.Player.Aim.IsPressed();
        JumpInput = actions.Player.Jump.IsPressed();
        CrouchInput = actions.Player.Crouch.WasPressedThisFrame();
        RunInput = actions.Player.Sprint.IsPressed();

        // ReloadInput = actions.Player.Reload.WasPressedThisFrame();
        // ThrowGrenadeInput = actions.Player.ThrowGrenade.WasPressedThisFrame();
        // QuickChangeWeaponInput = actions.Player.QuickChangeWeapon.WasPressedThisFrame();
        // ChangeFireModeInput = actions.Player.ChangeFireMode.WasPressedThisFrame();
    }

    void LateUpdate()
    {
        StartAttackInput = false;
        EndAttackInput = false;
    }
}
