using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class SwordCombat : MonoBehaviour, IWeapon
{
    [Header("Data Sources")]
    public WeaponData weaponData;
    public ComboSO defaultCombo;

    [Header("Reference")]
    public Animator anim;
    public CinemachineImpulseSource impulseSource;
    public PlayerMovement movementScript;
    public WeaponController weaponController;
    public PlayerEventsManager events;

    [Header("Attack Setting")]
    public Transform attackPoint;  // 攻擊判定的中心點 (要在編輯器裡設定)
    public LayerMask enemyLayers;
    private AttackAction currentAction
    {
        get
        {
            if (currentComboStep > 0)
                return defaultCombo.comboSteps[currentComboStep - 1];
            else
                return defaultCombo.counterAttack;
        }
    }

    [Header("SFX / VFX")]
    public AudioSource audioSource;

    [Header("Combo Setting")]
    private bool isAttacking = false;
    private int currentComboStep = 0;
    private bool inputBuffered = false;
    private float lastInputTime = 0f;
    private bool canNextCombo = false;  // 這個變數由 Animation Events 控制

    [Header("CounterAttack Settings")]
    [SerializeField] private float counterAttackThreshold = 3f;
    [SerializeField] private float counterAttackSlowTime = 0.2f;
    [SerializeField] private float counterAttackDuration = 0.5f;
    public float counterAttackWindowEndTime { get; private set; } = 0f;

    public bool IsAttacking => isAttacking;
    public bool CanCounterAttack => Time.time <= counterAttackWindowEndTime;

    private bool isInitialized = false;

    void OnEnable()
    {
        if (events != null)
        {
            events.OnPerfectDodge += OpenCounterAttackWindow;
            isInitialized = true;
        }
    }
    void Start()
    {
        if (!isInitialized)
            events.OnPerfectDodge += OpenCounterAttackWindow;
    }
    void OnDisable()
    {
        if (isInitialized)
            events.OnPerfectDodge -= OpenCounterAttackWindow;
    }
    void Update()
    {
        if (movementScript != null && !movementScript.isGrounded) return;
        // 待優化 : 冷卻時間
        if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && !anim.IsInTransition(0) && currentComboStep != -1)
        {
            ResetCombo();
        }
        if(Time.time - lastInputTime > weaponData.BufferTime)
        {
            inputBuffered = false;
        }
        CheckCombatExecution();
    }
    void CheckCombatExecution()
    {
        if(!isAttacking && inputBuffered)
        {
            StartAttack(1);
        }
        else if(isAttacking && inputBuffered && canNextCombo)
        {
            if (currentComboStep < defaultCombo.comboSteps.Count) 
                StartAttack(currentComboStep + 1);
        }
    }
    void StartAttack(int step)
    {
        if (step > defaultCombo.comboSteps.Count) return;

        //消耗
        inputBuffered = false;
        canNextCombo = false;
        //初始化
        currentComboStep = step;
        SetAttackState(true);

        anim.CrossFade(currentAction.AnimName, currentAction.AnimTransitionTime);
    }
    void ResetCombo()
    {
        if (currentComboStep != 0)
        {
            // 只有當數值不是 0 的時候才執行重置，避免每一幀都 print log
            SetAttackState(false);
            currentComboStep = 0;
            inputBuffered = false;
            canNextCombo = false;
        }
    }
    void SetAttackState(bool attacking)
    {
        isAttacking = attacking;
        movementScript.SetMovementEnabled(!attacking);
    }

    void DetectHit()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, weaponData.AttackRange, enemyLayers);

        // 如果真的有砍到東西
        if (hitEnemies.Length == 0 || currentComboStep > defaultCombo.comboSteps.Count) return;
        // Hit Stop
        weaponController.TriggerHitStop(currentAction.HitStopDuration);
        // Camera Shake
        impulseSource.GenerateImpulse(currentAction.ImpulseForce);

        foreach (Collider enemy in hitEnemies)
        {
            IDamageable target = enemy.GetComponent<IDamageable>();
            if (target != null)
            {
                weaponController.DealDamage(target, currentAction.DamageMultiplier, currentAction.PoiseMultiplier);
                // 特效
                weaponController.PlayAttackEffect(attackPoint, enemy, currentAction.OverrideHitEffect);
            }
        }
    }
    void OpenComboWindow()
    {
        canNextCombo = true;
        if (inputBuffered && currentComboStep < 3) StartAttack(currentComboStep + 1);
    }
    void PlayAttackSound()
    {
        if (audioSource != null && currentComboStep != 0)
        {
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.PlayOneShot(currentAction.AttackSound);
        }
    }

    public void OpenCounterAttackWindow()
        => counterAttackWindowEndTime = Time.time + counterAttackThreshold;
    public void StopCounterAttack()
        => ResetCombo();

    public void RequestAttack()
    {
        lastInputTime = Time.time;
        inputBuffered = true;
    }
    public void RequestCounterAttack()
    {
        StartAttack(-1);
        counterAttackWindowEndTime = 0f;
        StartCoroutine(WitchTimeRoutine());
    }
    IEnumerator WitchTimeRoutine()
    {
        Time.timeScale = counterAttackSlowTime;
        yield return new WaitForSecondsRealtime(counterAttackDuration);
        Time.timeScale = 1f;
    }
    public void RequestSpecialAttack() { }
    public void InterruptAttack() => ResetCombo();

    public void OnCombatAnimationEvent(string eventName)
    {
        switch (eventName)
        {
            case "DetectHit":
                DetectHit();
                break;
            case "OpenComboWindow":
                OpenComboWindow(); 
                break;
            case "PlayAttackSound":
                PlayAttackSound();
                break;
            case "StopCounterAttack":
                StopCounterAttack();
                break;
            default:
                Debug.LogWarning($"未處理戰鬥動畫: {eventName}");
                break;
        }
    }
}
