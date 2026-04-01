using UnityEngine;

public class RoomBound : MonoBehaviour
{
    [SerializeField] private MenuManager menuManager;

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            menuManager.OpenSceneProgression();
        }
    }
}
