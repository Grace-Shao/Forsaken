using UnityEngine;
public class DogStunState : State
{
    private DogStateMachine dogContext;
    private float curTime;
    public DogStunState(DogStateMachine currentContext) : base(currentContext)
    {
        dogContext = currentContext;
        isBaseState = true;
    }
    public override void EnterState()
    {
        dogContext.Anim.SetTrigger("Idle");
        dogContext.AppliedMovementX = 0f;
        dogContext.AppliedMovementY = 0f;
        curTime = 0f;
    }
    public override void UpdateState()
    {
        curTime += Time.deltaTime;
        CheckSwitchStates();
    }
    public override void ExitState()
    {
        dogContext.IsStunned = false;
        dogContext.Anim.ResetTrigger("Idle");
    }

    public override void CheckSwitchStates()
    {
        if (curTime > dogContext.StunTime)
        {
            if (dogContext.InRange())
            {
                SwitchState(new DogWindupState(dogContext));
            } else if (!dogContext.InRange())
            {
                SwitchState(new DogWalkState(dogContext));
            }
        } 
    }
}
