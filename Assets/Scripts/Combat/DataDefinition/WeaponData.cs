using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Scriptable Objects/Weapon System/Basic Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Attack Properticy")]
    public int BaseDamage = 10;
    public float BasePoiseDamage = 10f;
    public float AttackRange = 1.5f;

    [Header("SFX / VFX")]
    public GameObject DefaultHitEffectPrefab; //砍中的火花粒子效果

    [Header("Feedback")]
    public float BufferTime = 1.5f;

    [Header("Scaling Curves (X: Level, Y: Magnification)")]
    public AnimationCurve DamageScaleCurve;
    public AnimationCurve PoiseDamageScaleCurve;
}
