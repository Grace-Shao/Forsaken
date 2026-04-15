using UnityEngine;
public class DogWindupState : State
{
    private DogStateMachine dogContext;
    public DogWindupState(DogStateMachine currentContext) : base(currentContext)
    {
        dogContext = currentContext;
        isBaseState = true;
    }
    public override void EnterState()
    {
        dogContext.Anim.SetTrigger("Windup");
        dogContext.AppliedMovementX = 0f;
        dogContext.AppliedMovementY = 0f;
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState()
    {
        dogContext.WindUpFinished = false;
        dogContext.Anim.ResetTrigger("Windup");
    }

    public override void CheckSwitchStates()
    {
        if (dogContext.WindUpFinished)
        {
            SwitchState(new DogPounceState(dogContext));
        } 
    }
}
