using UnityEngine;
using System.Collections;
using System;

public class PlayerReaction : MonoBehaviour, IDamageable
{
    public Vector3 LastAttackerPos { get; private set; }

    [Header("Stats")]
    public bool isPlayerDead { get; private set; } = false;
    public bool IsInvincible { get; private set; } = false;

    [Header("Perfect Dodge Settings")]
    public float perfectDodgeSlowTime = 0.2f;
    public float perfectDodgeDuration = 0.01f;
    private bool hasPerfectDodge = false;  // 保證只觸發一次

    private PlayerMovement movement;
    private Animator anim;
    private Collider col;
    private Rigidbody rb;
    private PlayerInput input;
    private PlayerStats stats;
    private PlayerStateMachine stateMachine;
    private PlayerEventsManager events;

    void Awake()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        input = GetComponent<PlayerInput>();
        movement = GetComponent<PlayerMovement>();
        stats = GetComponent<PlayerStats>();
        stateMachine = GetComponent<PlayerStateMachine>();
        events = GetComponent<PlayerEventsManager>();
    }
    public void OnAnimationEvent_SetInvincible(bool state)
    {
        IsInvincible = state;
        if (!state) hasPerfectDodge = false;
    }
    public void TakeDamage(DamageInfo info)
    {
        if (isPlayerDead) return;

        if (IsInvincible)
        {
            if (!hasPerfectDodge)
            {
                TriggerPerfectDodge(info);
            }
            return;
        }
        
        stats.CurrentHp -= info.Damage;
        if (stats.CurrentHp < 0) stats.CurrentHp = 0;
        Debug.Log($"<color=red>{name} 受傷了 ! 剩餘血量: {stats.CurrentHp}</color>");

        if (stats.CurrentHp > 0f)
        {
            LastAttackerPos = info.AttackerPos;
            stateMachine.ChangeState(stateMachine.HurtState);
        }
        else
        {
            stateMachine.ChangeState(stateMachine.DeadState);
        }
    }
    void TriggerPerfectDodge(DamageInfo info)
    {
        hasPerfectDodge = true;
        Debug.Log("<color=cyan>極限閃避觸發!</color>");
        StartCoroutine(WitchTimeRoutine());

        events.TriggerPerfectDodge();
        // 這裡可以呼叫：
        // - 攝影機腳本：瞬間拉近 FOV (Zoom in)、加上色差 (Chromatic Aberration)
        // - 特效腳本：在原地留下一個主角的殘影、爆出藍色火花
        // - 音效腳本：播放那種「嗡——」的重低音音效

    }
    IEnumerator WitchTimeRoutine()
    {
        Time.timeScale = perfectDodgeSlowTime;
        yield return new WaitForSecondsRealtime(perfectDodgeDuration);
        Time.timeScale = 1f;
    }

    public void RequestHurt(Vector3 attackerPos)
    {

        int num = UnityEngine.Random.Range(1, 3);
        anim.CrossFade($"StandingReact{num}", 0.1f);

        ApplyKnockback(attackerPos);

    }
    void ApplyKnockback(Vector3 attackerPos)
    {
        Vector3 dir = (attackerPos - transform.position).normalized;
        dir.y = 0;
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(dir);

        rb.linearVelocity = Vector3.zero;
        rb.AddForce(-dir, ForceMode.Impulse);
    }
    public void RequestDie()
    {
        isPlayerDead = true;
        Debug.Log($"{name} 已死亡!");
        anim.CrossFade("Death", 0.1f);

        if (col != null) col.enabled = false;
        if (input != null) input.SetInputEnabled(false);
        rb.isKinematic = true;

        GameStateManager.Instance.ChangeState(GameState.PlayerDead);
    }
}
