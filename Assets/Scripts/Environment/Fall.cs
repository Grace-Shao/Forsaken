using UnityEngine;

public class Fall : MonoBehaviour
{

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            MenuManager.RestartScene();
        }
    }
}
