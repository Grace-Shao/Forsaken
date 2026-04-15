using UnityEngine;
public class DogStartState : State
{
    private DogStateMachine dogContext;
    private float curTime;
    public DogStartState(DogStateMachine currentContext) : base(currentContext)
    {
        dogContext = currentContext;
        isBaseState = true;
    }
    public override void EnterState()
    {
        dogContext.Anim.SetTrigger("Idle");
        dogContext.AppliedMovementX = 0f;
        dogContext.AppliedMovementY = 0f;
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState()
    {
        dogContext.Anim.ResetTrigger("Idle");
        dogContext.AppliedMovementX = 0f;
        dogContext.AppliedMovementY = 0f;
    }

    public override void CheckSwitchStates()
    {
        if (dogContext.InRange())
        {
            SwitchState(new DogWindupState(dogContext));
        } else if (dogContext.InAggroRange())
        {
            SwitchState(new DogWalkState(dogContext));
        }
    }
}
