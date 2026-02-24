using UnityEngine;
using UnityEngine.UIElements;
using Unity.Properties;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerBars : MonoBehaviour
{
    private PlayerBarData barData;

    private CustomBar healthBar;
    private CustomBar manaBar;
    private CustomBar xpBar;


    [SerializeField] private MonoBehaviour damageableComponent; // Assign in Inspector if auto-detection fails
    private IDamageable info;

    
    private void Start()
    {
        if (barData == null)
        {
            barData = ScriptableObject.CreateInstance<PlayerBarData>();
        }

        if (damageableComponent != null)
        {
            info = damageableComponent as IDamageable;
            if (info == null)
            {
                Debug.LogError("Assigned damageableComponent does not implement IDamageable!");
            }
        }

        // Auto-find IDamageable if not assigned
        if (info == null)
        {
            info = GetComponentInParent<IDamageable>();
            if (info == null)
            {
                Debug.LogWarning("No IDamageable found in parent hierarchy for PlayerBars. UI will not be updated.");
            }
        }

        // Initialize bar data from IDamageable
        if (info != null)
        {
            barData.health = info.Health;
            barData.mana = 100f;
            barData.xp = 0f;
        }
        else
        {
            barData.health = 100f;
            barData.mana = 100f;
            barData.xp = 0f;
        }

        // Bind to UIController
        if (UIController.Instance != null)
        {
            UIController.Instance.SetDataSource(barData);
            FindAndInitializeBars();
        }
        else
        {
            Debug.LogError("UIController singleton not found!");
        }
    }

    private void Update()
    {
        // Sync health from IDamageable to barData
        if (info != null && barData != null)
        {
            barData.health = info.Health;
        }
    }

    private void FindAndInitializeBars()
    {
        var uiDocument = UIController.Instance.GetUIDocument();
        if (uiDocument == null)
        {
            Debug.LogError("UIDocument not found in UIController!");
            return;
        }

        var rootElement = uiDocument.rootVisualElement;
        if (rootElement == null)
        {
            Debug.LogError("Root element not found in UIDocument!");
            return;
        }

        healthBar = rootElement.Q<CustomBar>("HealthBar");
        manaBar = rootElement.Q<CustomBar>("ManaBar");
        xpBar = rootElement.Q<CustomBar>("XPBar");

        if (healthBar == null)
            Debug.LogError("HealthBar not found in PlayerBars.uxml!");
        if (manaBar == null)
            Debug.LogError("ManaBar not found in PlayerBars.uxml!");
        if (xpBar == null)
            Debug.LogError("XPBar not found in PlayerBars.uxml!");
    }

    public void SetHealth(float value)
    {
        if (info != null)
        {
            info.Health = (int)value;
        }
        if (barData != null)
        {
            barData.health = value;
        }
        if (healthBar != null)
        {
            healthBar.barValue = (int)value;
        }
    }

    public void SetMana(float value)
    {
        if (barData != null)
        {
            barData.mana = value;
        }
        if (manaBar != null)
        {
            manaBar.barValue = (int)value;
        }
    }

    public void SetXP(float value)
    {
        if (barData != null)
        {
            barData.xp = value;
        }
        if (xpBar != null)
        {
            xpBar.barValue = (int)value;
        }
    }

    public float GetHealth() => barData?.health ?? 0f;
    public float GetMana() => barData?.mana ?? 0f;
    public float GetXP() => barData?.xp ?? 0f;

    public PlayerBarData GetBarData() => barData;
    public CustomBar GetHealthBar() => healthBar;
    public CustomBar GetManaBar() => manaBar;
    public CustomBar GetXPBar() => xpBar;
}