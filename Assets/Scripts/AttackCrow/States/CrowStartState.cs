using UnityEngine;

public class CrowStartState : State
{
    private CrowStateMachine crowContext;
    private float curTime;

    public CrowStartState(CrowStateMachine currentContext) : base(currentContext)
    {
        crowContext = currentContext;
        isBaseState = true;
    }
    public override void EnterState()
    {
        crowContext.Anim.SetTrigger("Idle");
        crowContext.AppliedMovementX = 0f;
        crowContext.AppliedMovementY = 0f;
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState()
    {
        crowContext.Anim.ResetTrigger("Idle");
        crowContext.AppliedMovementX = 0f;
        crowContext.AppliedMovementY = 0f;
    }

    public override void CheckSwitchStates()
    {
        if (crowContext.InRange())
        {
            SwitchState(new CrowPounceState(crowContext));
        }
        else if (crowContext.InAggroRange())
        {
            SwitchState(new CrowWalkState(crowContext));
        }
    }
}
