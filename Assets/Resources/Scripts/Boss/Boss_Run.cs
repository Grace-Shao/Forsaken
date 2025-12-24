using UnityEngine;

public class Boss_Run : StateMachineBehaviour
{

    Transform player;
    Rigidbody2D rb;
    float target_distance = 2f;
    public float speed = 2.5f;
    Boss boss;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       player = GameObject.FindGameObjectWithTag("Player").transform;
       //refactor once boss prefab rig is in
       rb = animator.GetComponent<Rigidbody2D>();
       boss = animator.GetComponent<Boss>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.FacePlayer();
        Vector2 target = new Vector2(player.position.x, animator.gameObject.transform.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        if (Vector2.Distance(newPos, target) <= target_distance)
        {
            animator.SetTrigger("Attack");
        } else {
            rb.MovePosition(newPos);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       animator.ResetTrigger("Attack");
    }
}
