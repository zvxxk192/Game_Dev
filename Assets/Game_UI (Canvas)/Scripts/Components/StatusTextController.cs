using TMPro;
using UnityEditor.Experimental;
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


    public void Setup(GameObject player)
    {
        events = player.GetComponent<PlayerEventsManager>();
        stats = player.GetComponent<PlayerStats>();
        weaponController = player.GetComponent<WeaponController>();
        wallet = player.GetComponent<PlayerWallet>();

        gameObject.SetActive(true);
    }
    private void OnEnable()
    {
        BindEvent();
    }
    private void BindEvent()
    {
        if (events == null) return;

        isInitialize = true;

        events.OnPlayerLevelUp += HandlePlayerLevelUp;

        events.OnPlayerExpChanged += HandlePlayerExpChanged;

        events.OnPlayerHpChanged += HandlePlayerHpChanged;

        events.OnPlayerGoldChanged += HandlePlayerGoldChanged;
    }
    private void Start()
    {
        if (!isInitialize) BindEvent();

        levelTextValue.text = stats.CurrentLevel.ToString();
        expTextValue.text = $"{(int)stats.CurrentExp} / {(int)stats.ExpToNextLevel}";
        hpTextValue.text = $"{(int)stats.CurrentHp} / {(int)stats.MaxHp}";
        strengthTextValue.text = ((int)weaponController.CurrentDamage).ToString();
        goldTextValue.text = wallet.CurrentGold.ToString();
        agilityTextValue.text = ((int)stats.WalkSpeed).ToString();
    }
    private void OnDisable()
    {
        if (events == null) return;

        events.OnPlayerLevelUp -= HandlePlayerLevelUp;

        events.OnPlayerExpChanged -= HandlePlayerExpChanged;

        events.OnPlayerHpChanged -= HandlePlayerHpChanged;

        events.OnPlayerGoldChanged -= HandlePlayerGoldChanged;
    }
    private void HandlePlayerLevelUp(int currentLevel)
    {
        levelTextValue.text = currentLevel.ToString();

        // 因為升級了，所有成長數值皆需要更新
        expTextValue.text = $"{(int)stats.CurrentExp} / {(int)stats.ExpToNextLevel}";

        hpTextValue.text = $"{(int)stats.CurrentHp} / {(int)stats.MaxHp}";

        strengthTextValue.text = weaponController.CurrentDamage.ToString();
    }
    private void HandlePlayerExpChanged(float currentExp, float maxExp)
        => expTextValue.text = $"{(int)currentExp} / {(int)maxExp}";
    private void HandlePlayerHpChanged(float currentHp, float maxHp, bool isIncrease)
        => hpTextValue.text = $"{(int)currentHp} / {(int)maxHp}";
    private void HandlePlayerGoldChanged(int currentGold)
        => goldTextValue.text = currentGold.ToString();
}
