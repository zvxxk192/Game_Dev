using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Animator anim;
    private Rigidbody rb;
    private PlayerStats stats;
    private PlayerInput input;

    private float turnSmoothVelocity;

    [Header("States")]
    public bool isLockedOn { get; private set; } = false;
    public bool canMove { get; private set; } = true;
    public bool isRolling { get; private set; } = false;
    public bool isGrounded { get; private set; } = true;

    private Transform lockOnTarget;

    [Header("Physical Stats")]
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float groundDistance = 0.4f;  // 腳底的隱形球半徑
    [SerializeField] private float turnSmoothTime = 0.1f;
    public LayerMask groundMask;         
    public Transform groundCheck;        

    public float CurrentSpeedMagnitude { get; private set; }

    public event Action OnJumpTriggered;
    public event Action OnRollTriggered;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        stats = GetComponent<PlayerStats>();
        input = GetComponent<PlayerInput>();
    }
    void Update()
    {
        CheckGround();
    }
    void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
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
            Vector3 targetVelocity = moveDir * currentSpeed * input.InputMagnitude;
            rb.linearVelocity = new Vector3 (targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);

            CurrentSpeedMagnitude = input.InputMagnitude * (input.IsSprinting ? 2 : 1);
        }
        else
        {
            StopMovement();
        }
    }
    public void SetMovementEnabled(bool enabled)
    {
        canMove = enabled;
        if (!enabled)
        {
            StopMovement();
        }
    }
    void StopMovement()
    {
        rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
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
        if(useRootMotion)
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }
    }
}