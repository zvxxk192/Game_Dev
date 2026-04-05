using UnityEngine;

public struct DamageInfo
{
    public float Damage;
    public float PoiseDamage;
    public Vector3 AttackerPos;
}

public interface IDamageable
{
    void TakeDamage(DamageInfo info);
}
