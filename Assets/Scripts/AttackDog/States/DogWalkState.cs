using UnityEngine;
public class DogWalkState : State
{
    private DogStateMachine dogContext;
    public DogWalkState(DogStateMachine currentContext) : base(currentContext)
    {
        dogContext = currentContext;
        isBaseState = true;
    }
    public override void EnterState()
    {
        dogContext.AppliedMovementY = 0;
        dogContext.Anim.SetTrigger("Walk");
        
    }
    public override void UpdateState()
    {
        Debug.Log("walking");
        Vector3 target = new Vector3(dogContext.Player.gameObject.transform.position.x, dogContext.RB.gameObject.transform.position.y, 0f);
        Vector3 currentPos = new Vector3(dogContext.RB.gameObject.transform.position.x, dogContext.RB.gameObject.transform.position.y, 0f);
        Vector3 direction = (target - currentPos).normalized;
        dogContext.AppliedMovementX = direction.x * dogContext.MoveSpeed;
        
        CheckSwitchStates();
    }
    public override void ExitState()
    {
        dogContext.Anim.ResetTrigger("Walk");
    }

    public override void CheckSwitchStates()
    {
        if (dogContext.InRange() && !dogContext.InAttack)
        {
            SwitchState(new DogWindupState(dogContext));
        }
    }
}
