using UnityEngine;
using UnityEngine.UIElements;
using Unity.Properties;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }

    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private MonoBehaviour damageableComponent; // Assign in Inspector
    private IDamageable damageableData;

    private VisualElement rootElement;
    private VisualElement barPanel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeUIDocument();
    }

    private void InitializeUIDocument()
    {
        if (damageableComponent != null)
        {
            damageableData = damageableComponent as IDamageable;
            if (damageableData == null)
            {
                Debug.LogError("Assigned damageableComponent does not implement IDamageable!");
            }
        }

        if (uiDocument == null)
        {
            uiDocument = GetComponent<UIDocument>();
        }

        if (uiDocument == null)
        {
            Debug.LogError("UIDocument component not found on UIController GameObject!");
            return;
        }

        rootElement = uiDocument.rootVisualElement;
        if (rootElement == null)
        {
            Debug.LogError("Root visual element not found!");
            return;
        }

        // Find the bar panel from CustomBars UXML
        barPanel = rootElement.Query<VisualElement>("Panel").First();
        if (barPanel != null)
        {
            BindDamageableBars();
        }
    }

    private void BindDamageableBars()
    {
        if (damageableData == null)
        {
            damageableData = Resources.Load<DamageableData>("DamageableData");
        }

        if (damageableData == null)
        {
            Debug.LogWarning("DamageableData not assigned and not found in Resources folder. Bars will not be bound to data.");
            return;
        }

        // Set the data source
        barPanel.dataSource = damageableData;

        // Get all CustomBar elements
        var healthBar = barPanel.Query<VisualElement>().Where(el => el.ClassListContains("healthBar")).First();
        var cooldownBar = barPanel.Query<VisualElement>().Where(el => el.ClassListContains("staminaBar")).First();

        // Bind health bar
        if (healthBar != null)
        {
            healthBar.SetBinding("barValue", new DataBinding()
            {
                dataSourcePath = new PropertyPath(nameof(DamageableData.Health)),
                bindingMode = BindingMode.ToTarget
            });
        }

        // Bind cooldown bar
        if (cooldownBar != null)
        {
            cooldownBar.SetBinding("barValue", new DataBinding()
            {
                dataSourcePath = new PropertyPath(nameof(DamageableData.Cooldown)),
                bindingMode = BindingMode.ToTarget
            });
        }

        Debug.Log("DamageableData bindings initialized");
    }

    public void SetDataSource(object dataSource)
    {
        if (rootElement != null)
        {
            rootElement.dataSource = dataSource;
        }
    }

    public void BindDamageableData(DamageableData newDamageableData)
    {
        if (barPanel == null)
        {
            Debug.LogWarning("Bar panel not found. Cannot bind DamageableData.");
            return;
        }

        if (newDamageableData == null)
        {
            Debug.LogWarning("DamageableData is null. Cannot bind.");
            return;
        }

        damageableData = newDamageableData;

        // Set the data source
        barPanel.dataSource = damageableData;

        // Get all CustomBar elements
        var healthBar = barPanel.Query<VisualElement>().Where(el => el.ClassListContains("health")).First();
        var cooldownBar = barPanel.Query<VisualElement>().Where(el => el.ClassListContains("mana")).First();

        // Bind health bar
        if (healthBar != null)
        {
            healthBar.SetBinding("barValue", new DataBinding()
            {
                dataSourcePath = new PropertyPath(nameof(DamageableData.Health)),
                bindingMode = BindingMode.ToTarget
            });
        }

        // Bind cooldown bar
        if (cooldownBar != null)
        {
            cooldownBar.SetBinding("barValue", new DataBinding()
            {
                dataSourcePath = new PropertyPath(nameof(DamageableData.Cooldown)),
                bindingMode = BindingMode.ToTarget
            });
        }

        Debug.Log("DamageableData bound to UI bars");
    }

    public VisualElement GetRootElement()
    {
        return rootElement;
    }

    public UIDocument GetUIDocument()
    {
        return uiDocument;
    }
}
