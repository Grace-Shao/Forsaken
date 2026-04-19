using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ResponseHandler : MonoBehaviour
{
    [SerializeField] private RectTransform responseBox;
    [SerializeField] private RectTransform responseButtonTemplate;
    [SerializeField] private RectTransform responseContainer;
    [SerializeField] private Sprite responseNormalSprite;
    [SerializeField] private Sprite responseHoverSprite;
    [SerializeField] private Sprite responseDotSprite;
    [SerializeField] private float responseVerticalSpacingMultiplier = 0.75f;

    private DialogueUI dialogueUI;

    private List<GameObject> tempResponseButtons = new List<GameObject>();

    private void Awake()
    {
        dialogueUI = GetComponent<DialogueUI>();
        if (dialogueUI == null)
        {
            dialogueUI = GetComponentInParent<DialogueUI>();
        }

        if (dialogueUI == null)
        {
            dialogueUI = GetComponentInChildren<DialogueUI>(true);
        }

        if (responseButtonTemplate != null)
        {
            responseButtonTemplate.gameObject.SetActive(false);
        }
    }

    public void ShowResponses(Response[] responses)
    {
        if (responseButtonTemplate != null)
        {
            responseButtonTemplate.gameObject.SetActive(false);
        }

        VerticalLayoutGroup verticalLayoutGroup = responseContainer.GetComponent<VerticalLayoutGroup>();
        Vector2 templatePosition = responseButtonTemplate.anchoredPosition;
        Vector2 templateSize = responseButtonTemplate.sizeDelta;
        float templateHeight = responseButtonTemplate.rect.height * Mathf.Abs(responseButtonTemplate.localScale.y);
        float responseStepY = templateHeight * responseVerticalSpacingMultiplier;

        if (verticalLayoutGroup != null)
        {
            verticalLayoutGroup.spacing = responseStepY - templateHeight;
        }

        for (int i = 0; i < responses.Length; i++)
        {
            Response response = responses[i];
            GameObject responseButton = Instantiate(responseButtonTemplate.gameObject, responseContainer);
            RectTransform responseTransform = responseButton.GetComponent<RectTransform>();
            TMP_Text responseText = responseButton.GetComponentInChildren<TMP_Text>(true);
            Button responseButtonComponent = responseButton.GetComponent<Button>();
            LayoutElement responseLayoutElement = responseButton.GetComponent<LayoutElement>();
            if (responseButtonComponent == null)
            {
                responseButtonComponent = responseButton.GetComponentInChildren<Button>(true);
            }

            responseTransform.anchorMin = responseButtonTemplate.anchorMin;
            responseTransform.anchorMax = responseButtonTemplate.anchorMax;
            responseTransform.pivot = responseButtonTemplate.pivot;
            responseTransform.sizeDelta = templateSize;
            responseTransform.localScale = responseButtonTemplate.localScale;
            responseTransform.localRotation = responseButtonTemplate.localRotation;

            if (verticalLayoutGroup == null)
            {
                responseTransform.anchoredPosition = templatePosition + new Vector2(0f, -responseStepY * i);
            }
            else if (responseLayoutElement != null)
            {
                responseLayoutElement.preferredHeight = templateHeight;
            }

            responseButton.gameObject.SetActive(true);

            if (responseText != null)
            {
                responseText.gameObject.SetActive(true);
                responseText.enabled = true;
                responseText.text = response.ResponseText;
            }
            else
            {
                Debug.LogWarning("Response template does not contain a TMP_Text component.", responseButtonTemplate);
            }

            if (responseButtonComponent != null)
            {
                responseButtonComponent.onClick.RemoveAllListeners();
                responseButtonComponent.onClick.AddListener(() => OnPickedResponse(response));
                responseButtonComponent.interactable = true;
            }
            else
            {
                Debug.LogWarning("Response template does not contain a Button component.", responseButtonTemplate);
            }

            ResponseHoverVisual hoverVisual = responseButton.GetComponent<ResponseHoverVisual>();
            if (hoverVisual != null)
            {
                hoverVisual.Configure(responseNormalSprite, responseHoverSprite, responseDotSprite);
            }

            tempResponseButtons.Add(responseButton);
        }

        if (verticalLayoutGroup != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(responseContainer);
        }

        responseBox.gameObject.SetActive(true);
    }

    public void OnPickedResponse(Response response)
    {
        if (response == null)
        {
            Debug.LogWarning("Clicked response is null.", this);
            return;
        }

        if (response.DialogueObject == null)
        {
            Debug.LogWarning("Clicked response has no DialogueObject assigned.", this);
            return;
        }

        responseBox.gameObject.SetActive(false);

        foreach (GameObject button in tempResponseButtons)
        {
            Destroy(button);
        }

        tempResponseButtons.Clear();

        if (dialogueUI != null)
        {
            dialogueUI.ShowDialogue(response.DialogueObject);
        }
        else
        {
            Debug.LogError("ResponseHandler could not find a DialogueUI on this object, its parents, or its children.", this);
        }
    }
}
