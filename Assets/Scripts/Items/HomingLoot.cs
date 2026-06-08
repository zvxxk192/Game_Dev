using UnityEngine;

public class HomingLoot : MonoBehaviour
{
    [Header("Loot Settings")]
    [SerializeField] private EnemyStats stats;
    [SerializeField] private PlayerWallet wallet;
    [SerializeField] private PlayerLeveling leveling;

    [Header("Magnet Settings")]
    [SerializeField] private float magnetRadius = 8f;
    [SerializeField] private float flySpeed = 15f;
    [SerializeField] private float collectDistance = 1f;

    private Transform targetPlayer;
    private bool isSucking = false;

    private void Update()
    {
        if (!isSucking)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, magnetRadius);
            foreach(var hit in hits)
            {
                if (hit.GetComponent<PlayerWallet>() != null || hit.GetComponent<PlayerLeveling>() != null)
                {
                    targetPlayer = hit.transform;
                    isSucking = true;
                    break;
                }
            }
        }
        else if(targetPlayer != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPlayer.position, flySpeed * Time.deltaTime);
            if((transform.position - targetPlayer.position).sqrMagnitude <= collectDistance * collectDistance)
            {
                CollectLoot();
            }
        }
    }
    private void CollectLoot()
    {
        wallet = targetPlayer.GetComponent<PlayerWallet>();
        leveling = targetPlayer.GetComponent<PlayerLeveling>();

        if (wallet != null) wallet.AddGold(stats.LootGoldValue);
        if (leveling != null) leveling.AddExp(stats.LootExpValue);

        Destroy(gameObject);
    }
}

