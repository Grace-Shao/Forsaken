using UnityEngine;

public class ArmCannonPIckup : MonoBehaviour
{
    [SerializeField] CutsceneManager manager;
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            manager.PlayCutScene(2);
        }
    }
}
