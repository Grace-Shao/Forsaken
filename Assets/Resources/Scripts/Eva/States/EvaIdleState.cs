using UnityEngine;
public class EvaIdleState : State
{
    private EvaStateMachine evaContext;
    public EvaIdleState(EvaStateMachine currentContext) : base(currentContext)
    {
        evaContext = currentContext;
        isBaseState = true;
    }
    public override void EnterState()
    {
        evaContext.Anim.Play("Idle");
        evaContext.AppliedMovementX = 0f;
        evaContext.AppliedMovementY = 0f;
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
        if (evaContext.FollowRange())
        {
            SwitchState(new EvaFollowState(evaContext));
        }
    }
}
