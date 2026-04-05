using UnityEngine;
using System;

public class EnemyReaction : MonoBehaviour, IDamageable
{
    [Header("Data Sources")]
    public EnemyData data;

    [Header("Loot References")]
    public GameObject lootPrefab;

    [Header("Enemy State")]
    public bool isEnemyDead { get; private set; } = false;

    public Vector3 LastAttackerPos { get; private set; }

    private float lastHitTime;

    private Animator anim;
    private EnemyStats stats;
    private EnemyStateMachine stateMachine;

    void Awake()
    {
        anim = GetComponent<Animator>();
        stats = GetComponent<EnemyStats>();
        stateMachine = GetComponent<EnemyStateMachine>();
    }
    void Update()
    {
        if (stats.CurrentPoise < stats.MaxPoise && Time.time - lastHitTime > stats.PoiseResetTime)
        {
            stats.CurrentPoise = stats.MaxPoise;
        }
    }
    public void TakeDamage(DamageInfo info)
    {
        if (isEnemyDead) return;

        lastHitTime = Time.time;
        stats.CurrentHp -= info.Damage;
        stats.CurrentPoise -= info.PoiseDamage;
        Debug.Log($"{name} 被打了! 剩餘血量: {stats.CurrentHp}");

        if (stats.CurrentHp <= 0)
        {
            stateMachine.ChangeState(stateMachine.DeadState);
        }
        else if(stats.CurrentPoise <= 0)
        {
            LastAttackerPos = info.AttackerPos;
            stats.CurrentPoise = stats.MaxPoise;
            stateMachine.ChangeState(stateMachine.HurtState);
        }
    }
    public void InstantiateDeadLoot()
    {
        isEnemyDead = true;

        if(lootPrefab != null)
        {
            Instantiate(lootPrefab, transform.position + Vector3.up * 1f, Quaternion.identity);
        }
    }
}
