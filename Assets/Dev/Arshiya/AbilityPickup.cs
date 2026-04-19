using UnityEngine;

public class AbilityPickup : MonoBehaviour
{
    public Material outlineMaterial;
    public Color outlineColor = Color.white;
    public float outlineThickness = .2f;
    private SpriteRenderer outlineRenderer;

    [SerializeField] CutsceneManager manager;

    private void Start()
    {
        // create outline renderer not on object but as a child
        GameObject outlineObject = new GameObject("Outline");
        outlineObject.transform.SetParent(transform);
        outlineObject.transform.localPosition = Vector3.zero;
        outlineObject.transform.localScale = Vector3.one * (1 + outlineThickness);
        outlineObject.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        outlineRenderer = outlineObject.AddComponent<SpriteRenderer>();
        outlineRenderer.sprite = GetComponent<SpriteRenderer>().sprite;
        outlineRenderer.material = outlineMaterial;
        outlineRenderer.color = outlineColor;
        outlineRenderer.sortingLayerID = GetComponent<SpriteRenderer>().sortingLayerID;
        outlineRenderer.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - 1; //
    }
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("collision");
            manager.PlayCutScene(2);
        }
    }
}
