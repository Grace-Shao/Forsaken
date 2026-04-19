using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private TMP_Text speakerName;
    [SerializeField] private Image portraitImage;

    public bool IsOpen { get; private set; }

    private ResponseHandler responseHandler;
    private TypewriterEffect typewriterEffect;

    private void Awake()
    {
        typewriterEffect = GetComponent<TypewriterEffect>();
        if (typewriterEffect == null)
        {
            typewriterEffect = GetComponentInChildren<TypewriterEffect>(true);
        }

        responseHandler = GetComponent<ResponseHandler>();
        if (responseHandler == null)
        {
            responseHandler = GetComponentInChildren<ResponseHandler>(true);
        }

        if (responseHandler == null)
        {
            responseHandler = GetComponentInParent<ResponseHandler>();
        }
    }

    private void Start()
    {
        CloseDialogueBox();
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        if (!HasRequiredReferences())
        {
            return;
        }

        IsOpen = true;
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        if (dialogueObject.CharacterPortrait != null)
        {
            portraitImage.sprite = dialogueObject.CharacterPortrait;
            portraitImage.gameObject.SetActive(true);
        }
        else
        {
            portraitImage.gameObject.SetActive(false);
        }

        for (int i = 0; i < dialogueObject.Dialogue.Length; i++)
        {
            DialogueObject.DialogueLine line = dialogueObject.Dialogue[i];

            if (speakerName != null)
            {
                speakerName.text = line.name;
            }

            if (line.portrait != null)
            {
                portraitImage.sprite = line.portrait;
                portraitImage.gameObject.SetActive(true);
            }
            else
            {
                portraitImage.gameObject.SetActive(false);
            }

            yield return typewriterEffect.Run(line.text, textLabel);

            if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses)
            {
                break;
            }

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
        }

        if (dialogueObject.HasResponses)
        {
            responseHandler.ShowResponses(dialogueObject.Responses);
        }
        else
        {
            CloseDialogueBox();
        }
    }

    private void CloseDialogueBox()
    {
        IsOpen = false;

        if (dialogueBox != null)
        {
            dialogueBox.SetActive(false);
        }

        if (textLabel != null)
        {
            textLabel.text = string.Empty;
        }

        if (speakerName != null)
            speakerName.text = "";
    }

    private bool HasRequiredReferences()
    {
        if (dialogueBox == null)
        {
            Debug.LogError("DialogueUI is missing Dialogue Box reference.", this);
            return false;
        }

        if (textLabel == null)
        {
            Debug.LogError("DialogueUI is missing Text Label reference.", this);
            return false;
        }

        if (portraitImage == null)
        {
            Debug.LogError("DialogueUI is missing Portrait Image reference.", this);
            return false;
        }

        if (typewriterEffect == null)
        {
            Debug.LogError("DialogueUI could not find a TypewriterEffect on this object or its children.", this);
            return false;
        }

        if (responseHandler == null)
        {
            Debug.LogError("DialogueUI could not find a ResponseHandler on this object, its children, or its parents.", this);
            return false;
        }

        return true;
    }
}