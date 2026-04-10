using UnityEngine;

public class TriggerMobRush : MonoBehaviour
{
    [SerializeField] MobRushManager manager;
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            manager.BeginStageOne();
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
