using UnityEngine;

public class CrowPounceState : State
{
    private CrowStateMachine crowContext;
    public CrowPounceState(CrowStateMachine currentContext) : base(currentContext)
    {
        
        crowContext = currentContext;
        isBaseState = true;
    }
    public override void EnterState()
    {
        Vector3 target = new Vector3(crowContext.Player.gameObject.transform.position.x, crowContext.RB.gameObject.transform.position.y, 0f);
        Vector3 currentPos = new Vector3(crowContext.RB.gameObject.transform.position.x, crowContext.RB.gameObject.transform.position.y, 0f);
        Vector3 direction = (target - currentPos).normalized;

        crowContext.InAttack = true;
        crowContext.RB.AddForce(new Vector2(direction.x * crowContext.JumpForce.x, direction.y * crowContext.JumpForce.y), ForceMode2D.Impulse);
        crowContext.AppliedMovementX = 0;
        crowContext.AppliedMovementY = 0;
        Debug.Log("cro hunt");
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState()
    {
        crowContext.InAttack = false;
    }

    public override void CheckSwitchStates()
    {
        SwitchState(new CrowStunState(crowContext));
    }
}
