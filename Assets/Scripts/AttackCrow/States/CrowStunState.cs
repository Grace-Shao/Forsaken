using UnityEngine;
public class CrowStunState : State
{
    private CrowStateMachine crowContext;
    private float curTime;
    public CrowStunState(CrowStateMachine currentContext) : base(currentContext)
    {
        crowContext = currentContext;
        isBaseState = true;
    }
    public override void EnterState()
    {
        crowContext.Anim.SetTrigger("Idle");
        crowContext.AppliedMovementX = 0f;
        crowContext.AppliedMovementY = 0f;
        curTime = 0f;
    }
    public override void UpdateState()
    {
        curTime += Time.deltaTime;
        CheckSwitchStates();
    }
    public override void ExitState()
    {
        crowContext.Anim.ResetTrigger("Idle");
        crowContext.IsStunned = false;
    }

    public override void CheckSwitchStates()
    {
        if (curTime > crowContext.StunTime)
        {
            if (crowContext.InRange())
            {
                SwitchState(new CrowPounceState(crowContext));
            } else if (!crowContext.InRange())
            {
                SwitchState(new CrowWalkState(crowContext));
            }
        } 
    }
}
