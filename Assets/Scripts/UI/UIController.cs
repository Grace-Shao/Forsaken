using UnityEngine;
using UnityEngine.UIElements;
using Unity.Properties;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }

    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private MonoBehaviour damageableComponent; // Assign in Inspector
    private IDamageable damageableData;

    private CustomBar healthBar;
    private CustomBar cooldownBar;

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
        // Set the data source
        barPanel.dataSource = damageableData;

        // Get all CustomBar elements
        this.healthBar = barPanel.Query<CustomBar>().Where(el => el.ClassListContains("healthBar")).First();
        this.cooldownBar = barPanel.Query<CustomBar>().Where(el => el.ClassListContains("cooldownBar")).First();

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

        Debug.Log(healthBar.label);

        Debug.Log("DamageableData bindings initialized");
    }

    public void SetDataSource(object dataSource)
    {
        if (rootElement != null)
        {
            rootElement.dataSource = dataSource;
        }
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
