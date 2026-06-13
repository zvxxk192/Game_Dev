using UnityEngine;
using System;

[Pausable]
public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cam;
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private PlayerStats stats;
    [SerializeField] private PlayerInput input;
    [SerializeField] private PlayerStateMachine stateMachine;

    private float turnSmoothVelocity;

    [Header("States")]
    public bool isLockedOn { get; private set; } = false;
    //public bool canMove { get; private set; } = true;
    public bool isRolling { get; private set; } = false;
    public bool isGrounded { get; private set; } = true;

    private Transform lockOnTarget;

    [Header("Physical Settings")]
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float groundDistance = 0.45f;  // 腳底的隱形球半徑
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private Vector3 groundCheckOffset;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheck;

    public float CurrentSpeedMagnitude { get; private set; }

    public event Action OnJumpTriggered;
    public event Action OnRollTriggered;

    void Awake()
    {
        if (anim == null)
            anim = GetComponent<Animator>();
        if (rb == null)
            rb = GetComponent<Rigidbody>();
        if (stats == null)
            stats = GetComponent<PlayerStats>();
        if (input == null)
            input = GetComponent<PlayerInput>();
        if (stateMachine == null)
            stateMachine = GetComponent<PlayerStateMachine>();
    }
    void Update()
    {
        CheckGround();

        GroundSnapping();
    }
    void CheckGround()
        => isGrounded = Physics.CheckSphere(groundCheck.position + groundCheckOffset, groundDistance, groundMask);
    void GroundSnapping()
    {
        if (stateMachine.CurrentState == stateMachine.AirState) return;

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, groundMask))
        {
            float targetY = hit.point.y + 1;

            if (!isGrounded)
            {
                // 強行拉回原位
                rb.position = new Vector3(rb.position.x, targetY, rb.position.z);

                // 只有當速度還向上時才把他抹零
                if (rb.linearVelocity.y > 0)
                    rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            }
        }

    }
    
    public void RequestJump()
    {
        if (isGrounded)  //跳躍
        {
            OnJumpTriggered?.Invoke();
        }
    }
    public void RequestRoll()
    {
        if (!isRolling)
        {
            isRolling = true;

            OnRollTriggered?.Invoke();
        }
    }
    public void RequestMove()
    {
        if (input.InputMagnitude >= 0.1f)
        {
            // 開始可以移動
            SetDisplacementEnabled(true);

            // 計算目標角度 (Atan2(x, z) 轉成度數) + 相機目前的旋轉角度
            float targetAngle = Mathf.Atan2(input.Direction.x, input.Direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

            // 計算平滑角度
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            // 旋轉角色
            Quaternion targetRotation = Quaternion.Euler(0f, angle, 0f);
            rb.rotation = targetRotation;
            //rb.MoveRotation(targetRotation);

            float currentSpeed = input.IsSprinting ? stats.RunSpeed : stats.WalkSpeed;

            // 移動時：方向 * 力道 * 速度 * 時間
            Vector3 moveDir = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
            // 計算最終速度的變量
            Vector3 targetVelocity = moveDir * currentSpeed * input.InputMagnitude;
            rb.linearVelocity = new Vector3 (targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);

            CurrentSpeedMagnitude = input.InputMagnitude * (input.IsSprinting ? 2 : 1);
        }
        else
        {
            SetDisplacementEnabled(false);
        }
    }
    public void SetDisplacementEnabled(bool enabled)
    {
        if (enabled)
        {
            // 解除 X Z 軸鎖定
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            StopMovement();
        }
    }
    void StopMovement()
    {
        if (stateMachine.CurrentState != stateMachine.AirState)
        {
            // 避免在斜坡走路時，下一幀讀取到向上的速度
            float gravity = Mathf.Min(0, rb.linearVelocity.y);
            rb.linearVelocity = new Vector3(0, gravity, 0);
        }
        else
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }

        // 鎖定 X Z 軸
        rb.constraints = RigidbodyConstraints.FreezeRotation |
                         RigidbodyConstraints.FreezePositionX |
                         RigidbodyConstraints.FreezePositionZ;

        CurrentSpeedMagnitude = 0f;
    }
    public void SetLockOnState(bool locked, Transform target)
    {
        isLockedOn = locked;
        lockOnTarget = target;
    }

    public void OnAnimationEvent_Jumpable()
    {
        // 物理公式：需要的速度 = 開根號(跳躍高度 * -2 * 重力)
        float jumpVelocityY = Mathf.Sqrt(jumpHeight * -2f * gravity);
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpVelocityY, rb.linearVelocity.z);
    }
    public void OnAnimationEvent_StopRoll()
    {
        isRolling = false;
    }
    public void TriggerRootMotion(bool useRootMotion)
    {
        anim.applyRootMotion = useRootMotion;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }
}