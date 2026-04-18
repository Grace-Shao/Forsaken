using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResponseHoverVisual : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Sprite normalBackgroundSprite;
    [SerializeField] private Sprite hoverBackgroundSprite;
    [SerializeField] private Image dotImage;
    [SerializeField] private Sprite dotSprite;

    private void Awake()
    {
        if (backgroundImage == null)
        {
            backgroundImage = GetComponent<Image>();
        }
    }

    private void OnEnable()
    {
        ResetVisuals();
    }

    public void Configure(Sprite normalSprite, Sprite hoverSprite, Sprite hoverDotSprite)
    {
        normalBackgroundSprite = normalSprite;
        hoverBackgroundSprite = hoverSprite;
        dotSprite = hoverDotSprite;
        ResetVisuals();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (backgroundImage != null)
        {
            if (hoverBackgroundSprite != null)
            {
                backgroundImage.sprite = hoverBackgroundSprite;
            }
        }

        if (dotImage != null)
        {
            if (dotSprite != null)
            {
                dotImage.sprite = dotSprite;
            }

            dotImage.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ResetVisuals();
    }

    public void ResetVisuals()
    {
        if (backgroundImage != null)
        {
            if (normalBackgroundSprite != null)
            {
                backgroundImage.sprite = normalBackgroundSprite;
            }
        }

        if (dotImage != null)
        {
            dotImage.gameObject.SetActive(false);
        }
    }
}