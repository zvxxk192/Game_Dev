using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public EnemyData data;
    public Transform attackPoint;

    private Animator anim;
    private EnemyStats stats;

    void Awake()
    {
        anim = GetComponent<Animator>();
        stats = GetComponent<EnemyStats>();
    }
    public void RequestAttack(Transform target)
    {
        FaceTarget(target);

        anim.CrossFade("Attack", 0.1f);
    }
    private void FaceTarget(Transform target)
    {
        Vector3 direaction = (target.position - transform.position).normalized;
        direaction.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direaction.x, 0, direaction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
    public void AnimationEvent_EnemyHit()
    {
        Collider[] hitTargets = Physics.OverlapSphere(attackPoint.position, data.AttackRange, data.TargetLayer);

        foreach (Collider target in hitTargets)
        {
            IDamageable damageable = target.GetComponent<IDamageable>();
            if (damageable != null)
            {
                DamageInfo info = new ()
                {
                    Damage = stats.Damage,
                    AttackerPos = transform.position,
                };
                damageable.TakeDamage(info);
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, data.LookRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, data.AttackRadius);

        Gizmos.DrawWireSphere(attackPoint.position, data.AttackRange);
    }
}
