using UnityEngine;
public class CrowWalkState : State
{
    private CrowStateMachine crowContext;
    public CrowWalkState(CrowStateMachine currentContext) : base(currentContext)
    {
        crowContext = currentContext;
        isBaseState = true;
    }
    public override void EnterState()
    {
        crowContext.Anim.Play("Walk");
        
    }
    public override void UpdateState()
    {
        Vector3 target = new Vector3(crowContext.Player.gameObject.transform.position.x, crowContext.Player.gameObject.transform.position.y, 0f);
        Vector3 currentPos = new Vector3(crowContext.RB.gameObject.transform.position.x, crowContext.RB.gameObject.transform.position.y, 0f);
        Vector3 direction = (target - currentPos).normalized;
        crowContext.AppliedMovementX = direction.x * crowContext.MoveSpeed;
        crowContext.AppliedMovementY = direction.y * crowContext.MoveSpeed;
        Debug.Log(crowContext.AppliedMovementX + ", " + crowContext.AppliedMovementY);
        Debug.Log("Can this cro walk");
        CheckSwitchStates();
    }
    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
        if (crowContext.IsStunned)
        {   
            SwitchState(new CrowStunState(crowContext));
        }
        if (crowContext.InRange() && !crowContext.InAttack)
        {
            SwitchState(new CrowPounceState(crowContext));
        }
    }
}
