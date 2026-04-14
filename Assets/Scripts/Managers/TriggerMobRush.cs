using UnityEngine;

public class TriggerMobRush : MonoBehaviour
{
    [SerializeField] private MobRushManager manager;
    [SerializeField] private Animator board;
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            manager.BeginMobRush();
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            board.SetTrigger("Fall");
        }
    }
}
