using UnityEngine;

public class TriggerMobRush : MonoBehaviour
{
    [SerializeField] private MobRushManager manager;
    [SerializeField] private EndlessModeManager endlessMode;
    [SerializeField] private Animator board;
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (manager != null)
            {
               manager.BeginMobRush(); 
            } else
            {
                endlessMode.BeginMobRush();
            }
            
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            board.SetTrigger("Fall");
        }
    }
}
