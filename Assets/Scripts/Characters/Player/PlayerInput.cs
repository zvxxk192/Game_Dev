using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerMovement playerMov;
    private PlayerStateMachine stateMachine;
    private WeaponController weaponController;
    private PlayerInteractor playerInteractor;

    [Header("Input Outputs")]
    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }
    public Vector3 Direction { get; private set; }
    public float InputMagnitude { get; private set; }
    public bool IsSprinting { get; private set; } = false;    // 是否正在長按跑步


    [Header("Control Flags")]
    public bool inputEnabled = true;

    void Awake()
    {
        playerMov = GetComponent<PlayerMovement>();
        stateMachine = GetComponent<PlayerStateMachine>();
        weaponController = GetComponent<WeaponController>();
        playerInteractor = GetComponent<PlayerInteractor>();
    }
    void Update()
    {
        if (!inputEnabled)
        {
            ResetInput();
            return;
        }

        Horizontal = Input.GetAxisRaw("Horizontal");
        Vertical = Input.GetAxisRaw("Vertical");
        Direction = new Vector3(Horizontal, 0f, Vertical).normalized;
        InputMagnitude = Mathf.Clamp01(Direction.magnitude);

        HandlePlayerStateInput();

        HandleGameStateInput();

        HandlePlayerInteractInput();
    }
    void HandlePlayerStateInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            stateMachine.HandleInput(PlayerCommand.Jump);
        }
        if (Input.GetButtonDown("Fire1"))
        {
            if (weaponController.currentWeapon.CanCounterAttack)
                stateMachine.HandleInput(PlayerCommand.CounterAttack);
            else
                stateMachine.HandleInput(PlayerCommand.Attack);
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            stateMachine.HandleInput(PlayerCommand.LockOn);
        }
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            stateMachine.HandleInput(PlayerCommand.Roll);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            IsSprinting = !IsSprinting;
        }
        if (InputMagnitude < 0.1f)
        {
            IsSprinting = false;
        }
    }
    void HandleGameStateInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameStateManager.Instance.CurrentState == GameState.Playing)
                GameStateManager.Instance.ChangeState(GameState.Paused);
            else if (GameStateManager.Instance.CurrentState == GameState.Paused)
                GameStateManager.Instance.ChangeState(GameState.Playing);
        }
    }
    void HandlePlayerInteractInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
            playerInteractor.PlayerInteract();
    }
    void ResetInput()
    {
        Horizontal = 0;
        Vertical = 0;
        Direction = Vector3.zero;
        InputMagnitude = 0;
        IsSprinting = false;
    }
    public void SetInputEnabled(bool enabled) => inputEnabled = enabled;
}
