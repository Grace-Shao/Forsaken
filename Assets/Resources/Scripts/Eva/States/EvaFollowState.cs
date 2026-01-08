using UnityEngine;
public class EvaFollowState : State
{
    private EvaStateMachine evaContext;
    public EvaFollowState(EvaStateMachine currentContext) : base(currentContext)
    {
        evaContext = currentContext;
        isBaseState = true;
    }
    public override void EnterState()
    {
        evaContext.Anim.Play("Walk");
        
    }
    public override void UpdateState()
    {
        Vector3 target = new Vector3(evaContext.Player.gameObject.transform.position.x, evaContext.RB.gameObject.transform.position.y, 0f);
        Vector3 currentPos = new Vector3(evaContext.RB.gameObject.transform.position.x, evaContext.RB.gameObject.transform.position.y, 0f);
        Vector3 direction = (target - currentPos).normalized;
        evaContext.AppliedMovementX = direction.x * evaContext.MoveSpeed;
        
        CheckSwitchStates();
    }
    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
        if (!evaContext.FollowRange())
        {
            SwitchState(new EvaIdleState(evaContext));
        }
    }
}
