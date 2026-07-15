using UnityEngine;

public class AttackTrigger : MonoBehaviour 
{
    // Reference to the parent dog script
    [SerializeField] private DogStateMachine dogScript; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the thing entering the bite zone is Eva
        if (other.gameObject.CompareTag("Eva"))
        {
            Debug.Log("<color=orange>[TRIGGER HIT]</color> BiteZone child detected Eva!");
            
            TestEvaStateMachine eva = other.gameObject.GetComponent<TestEvaStateMachine>();
            if (eva != null)
            {
                // Send damage directly to Eva using the Dog's damage value
                eva.ApplyDamage(dogScript.Damage); 
            }
        }
    }
//     private void OnTriggerEnter2D(Collider2D other)
//     {
//         // This will print the name of EVERYTHING the BiteBox touches
//         Debug.Log($"<color=cyan>[PHYSICS CHECK]</color> BiteBox just touched: {other.gameObject.name} on Layer: {LayerMask.LayerToName(other.gameObject.layer)}");
//     }
}