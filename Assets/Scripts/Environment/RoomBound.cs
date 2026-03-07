using UnityEngine;

public class RoomBound : MonoBehaviour
{
    [SerializeField] private CutsceneManager cutsceneManager;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            cutsceneManager.PlayCutScene(2);
        }
    }
}
