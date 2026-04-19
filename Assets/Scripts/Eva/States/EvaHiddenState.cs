using UnityEngine;
public class EvaHiddenState : State
{
    private EvaStateMachine evaContext;
    public EvaHiddenState(EvaStateMachine currentContext) : base(currentContext)
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

    }
    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
    }
}
