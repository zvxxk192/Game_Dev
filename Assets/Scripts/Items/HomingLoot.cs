using UnityEngine;

public class HomingLoot : MonoBehaviour
{
    [Header("Loot Settings")]
    public int goldValue = 10;
    public int expValue = 20;

    [Header("Magnet Settings")]
    public float magnetRadius = 8f;
    public float flySpeed = 15f;
    public float collectDistance = 1f;

    private Transform targetPlayer;
    private bool isSucking = false;

    void Update()
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
    void CollectLoot()
    {
        PlayerWallet wallet = targetPlayer.GetComponent<PlayerWallet>();
        if (wallet != null) wallet.AddGold(goldValue);

        PlayerLeveling leveling = targetPlayer.GetComponent<PlayerLeveling>();
        if (leveling != null) leveling.AddExp(expValue);

        Destroy(gameObject);
    }
}

