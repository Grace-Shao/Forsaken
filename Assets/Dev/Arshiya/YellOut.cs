using UnityEngine;

public class YellOut : MonoBehaviour
{
    [SerializeField] PlayerStateMachine player;
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }
        if (other.gameObject.CompareTag("Player"))
        {
            player.Interact();
        }
    }

    public void TriggerInteract()
    {
        player.Interact();
    }
}
