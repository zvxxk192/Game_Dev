using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Combat/Attack Action")]
public class AttackAction : ScriptableObject
{
    [Header("Animation Setting")]
    public string AnimName = "Attack";
    public float AnimTransitionTime = 0.1f;

    [Header("Value Setting")]
    public float DamageMultiplier = 1f; // 傷害倍率 
    public float PoiseMultiplier = 1f;  // 軔性傷害倍率

    [Header("Feedback")]
    public float HitStopDuration = 0.007f; // 砍到人的頓禎時間
    public float ImpulseForce = 1f;    // 鏡頭震動大小

    [Header("VFX (不填則使用武器預設值)")]
    public GameObject OverrideHitEffect;

    [Header("SFX")]
    public AudioClip AttackSound;
}