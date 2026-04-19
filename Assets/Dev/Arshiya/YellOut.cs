using UnityEngine;

public class YellOut : MonoBehaviour
{
    [SerializeField] PlayerStateMachine player;
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.Interact();
        }
    }
}
