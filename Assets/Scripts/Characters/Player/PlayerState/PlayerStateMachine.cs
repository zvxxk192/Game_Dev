using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{    
    public IPlayerState CurrentState { get; private set; }

    public WeaponController WeaponController { get; private set; }
    public PlayerMovement PlayerMov { get; private set; }
    public TargetLockSystem LockSystem { get; private set; }
    public PlayerReaction PlayerReact { get; private set; }

    public PlayerGroundedState GroundedState { get; private set; }
    public PlayerAirState AirState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }
    public PlayerRollState RollState { get; private set; }
    public PlayerHurtState HurtState { get; private set; }
    public PlayerDeadState DeadState { get; private set; }
    public PlayerCounterAttackState CounterAttackState { get; private set; }

    void Awake()
    {
        WeaponController = GetComponent<WeaponController>();
        PlayerMov = GetComponent<PlayerMovement>();
        LockSystem = GetComponent<TargetLockSystem>();
        PlayerReact = GetComponent<PlayerReaction>();

        GroundedState = new PlayerGroundedState(this);
        AirState = new PlayerAirState(this);
        AttackState = new PlayerAttackState(this);
        RollState = new PlayerRollState(this);
        HurtState = new PlayerHurtState(this);
        DeadState = new PlayerDeadState(this);
        CounterAttackState = new PlayerCounterAttackState(this);

        ChangeState(GroundedState);
    }
    void Update() => CurrentState?.Tick();
    void FixedUpdate() => CurrentState?.FixedTick();

    public void ChangeState(IPlayerState newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState?.Enter();
    }
    public void HandleInput(PlayerCommand command)
    {
        if (command == PlayerCommand.LockOn)
        {
            LockSystem.RequestLockOn();
            return;
        }
        CurrentState?.HandleInput(command);
    }
}

