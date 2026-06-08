using TMPro;
using UnityEngine;

public class StatusTextController : MonoBehaviour
{
    [SerializeField] private PlayerEventsManager events;

    [Header("Data Sources")]
    [SerializeField] private PlayerStats stats;
    [SerializeField] private WeaponController weaponController;
    [SerializeField] private PlayerWallet wallet;

    [Header("UI Text")]
    [SerializeField] private TextMeshProUGUI levelTextValue;
    [SerializeField] private TextMeshProUGUI expTextValue;
    [SerializeField] private TextMeshProUGUI hpTextValue;
    [SerializeField] private TextMeshProUGUI strengthTextValue;
    [SerializeField] private TextMeshProUGUI goldTextValue;
    [SerializeField] private TextMeshProUGUI agilityTextValue;

    private bool isInitialize = false;

    private void OnEnable()
    {
        if (events == null) return;

        isInitialize = true;

        events.OnPlayerLevelUp += HandlePlayerLevelUp;

        events.OnPlayerExpChanged += (currentExp, maxExp) 
            => { expTextValue.text = $"{(int)currentExp} / {(int)maxExp}"; };

        events.OnPlayerHpChanged += (currentHp, maxHp, isIncrease)
            => { hpTextValue.text = $"{(int)currentHp} / {(int)maxHp}"; };

        events.OnPlayerGoldChanged += (currentGold)
            => { goldTextValue.text = currentGold.ToString(); };
    }
    private void Start()
    {
        levelTextValue.text = stats.CurrentLevel.ToString();
        expTextValue.text = $"{(int)stats.CurrentExp} / {(int)stats.ExpToNextLevel}";
        hpTextValue.text = $"{(int)stats.CurrentHp} / {(int)stats.MaxHp}";
        strengthTextValue.text = ((int)weaponController.CurrentDamage).ToString();
        goldTextValue.text = wallet.CurrentGold.ToString();
        agilityTextValue.text = ((int)stats.WalkSpeed).ToString();
    }

    private void HandlePlayerLevelUp(int currentLevel)
    {
        levelTextValue.text = currentLevel.ToString();

        // 因為升級了，所有成長數值皆需要更新
        expTextValue.text = $"{(int)stats.CurrentExp} / {(int)stats.ExpToNextLevel}";

        hpTextValue.text = $"{(int)stats.CurrentHp} / {(int)stats.MaxHp}";

        strengthTextValue.text = weaponController.CurrentDamage.ToString();
    }
}
