using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour
{
    [Header("References")]
    public GameObject startingWeaponObject;
    public WeaponData weaponData;

    public IWeapon currentWeapon { get; private set; }

    private PlayerStats stats;

    public float CurrentDamage
    {
        get
        {
            if (weaponData == null) return 0f;
            float multiplier = weaponData.DamageScaleCurve.Evaluate(stats.CurrentLevel);
            return weaponData.BaseDamage * multiplier;
        }
    }
    public float CurrentPoiseDamage
    {
        get
        {
            if (weaponData == null) return 0f;
            float multiplier = weaponData.PoiseDamageScaleCurve.Evaluate(stats.CurrentLevel);
            return weaponData.BasePoiseDamage * multiplier;
        }
    }

    void Awake()
    {
        stats = GetComponent<PlayerStats>();
    }
    void Start()
    {
        if (startingWeaponObject != null)
        {
            EquipWeapon(startingWeaponObject.GetComponent<IWeapon>());
        }
    }

    public void EquipWeapon(IWeapon newWeapon)
    {
        currentWeapon = newWeapon;
    }
    public void DealDamage(IDamageable target, float damageMultiplier, float poiseMultiplier)
    {
        DamageInfo info = new DamageInfo()
        {
            Damage = CurrentDamage * damageMultiplier,
            PoiseDamage = CurrentPoiseDamage * poiseMultiplier,
            AttackerPos = transform.position
        };
        target.TakeDamage(info);
    }
    public void TriggerHitStop(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(HitStopRoutine(duration));
    }
    IEnumerator HitStopRoutine(float duration)
    {
        Time.timeScale = 0.1f;
        // *** WaitForSeconds是物件 ***
        yield return new WaitForSeconds(duration);
        Time.timeScale = 1.0f;
    }
    public void PlayAttackEffect(Transform attackPoint, Collider enemy, GameObject overrideHitEffect)
    {
        GameObject vfxToSpawn = overrideHitEffect == null
                                ? weaponData.DefaultHitEffectPrefab
                                : overrideHitEffect;

        if (vfxToSpawn != null)
        {
            Vector3 hitPos = enemy.ClosestPoint(attackPoint.position);
            // Quaternion.identity 是世界座標 (0, 0, 0) ， 這裡的意思是不要旋轉
            GameObject vfx = Instantiate(vfxToSpawn, hitPos, Quaternion.identity);
            Destroy(vfx, 1.0f);
        }
    }
}
