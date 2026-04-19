using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueObject")]
public class DialogueObject : ScriptableObject
{
    [System.Serializable]
    public class DialogueLine
    {
        public string name;
        public Sprite portrait;
        [TextArea] public string text;
    }

    [SerializeField] private Sprite characterPortrait;
    [SerializeField] private DialogueLine[] dialogue;
    [SerializeField] private Response[] responses;

    public Sprite CharacterPortrait => characterPortrait;

    public DialogueLine[] Dialogue => dialogue;

    public bool HasResponses => Responses != null && Responses.Length > 0;

    public Response[] Responses => responses;
}