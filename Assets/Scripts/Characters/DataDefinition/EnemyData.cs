using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Scriptable Objects/Enemy/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Sense Setting")]
    public float LookRadius = 10f;  // 發現玩家的距離
    public float AttackRadius = 2f; // 開始攻擊的距離

    [Header("Move Setting")]
    public float BasePatrolSpeed = 1f;
    public float BaseChaseSpeed = 3f;
    public float PatrolWaitTime = 2f; // 巡邏點發呆時間

    [Header("Attack Setting")]
    public float BaseDamage = 10;
    public float AttackCooldown = 2f;
    public float AttackRange = 1f;   // 實際判定傷害的範圍
    public LayerMask TargetLayer;    // 攻擊目標圖層 (Player)

    [Header("Defense Setting")]
    public float BaseMaxHp = 100f;
    //public float HitStunTime = 0.5f;
    public float BaseKnockbackForce = 5f;

    [Header("Poise Stats")]
    public float BaseMaxPoise = 50f;
    public float BasePoiseResetTime = 5f;
    public float BaseStaggerTime = 1f;

    [Header("Scaling Curves (X: Level, Y: Magnification)")]
    public AnimationCurve HealthScaleCurve;
    public AnimationCurve DamageScaleCurve;
    public AnimationCurve PoiseScaleCurve;
}