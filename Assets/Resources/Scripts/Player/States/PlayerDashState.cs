using UnityEngine;

public class PlayerDashState : State
{
    private PlayerStateMachine playerContext;
    public PlayerDashState(PlayerStateMachine currentContext) : base(currentContext)
    {
        playerContext = currentContext;
        isBaseState = true;
    }
    public override void EnterState()
    {
        playerContext.Anim.SetTrigger("Dash");
        // playerContext.AppliedMovementX = 0f;
        // playerContext.AppliedMovementY = 0f;
        playerContext.SetTimeScale(0.5f);
        playerContext.DashTrail.GetComponent<TrailRenderer>().enabled = true;
    }
    public override void UpdateState()
    {
        playerContext.AppliedMovementX = playerContext.CurrentMovementInput.x * playerContext.MoveSpeed * 2f;
        CheckSwitchStates();
    }
    public override void ExitState()
    {
        playerContext.SetTimeScale(1f);
        playerContext.DashTrail.GetComponent<TrailRenderer>().enabled = false;
        playerContext.Anim.ResetTrigger("Dash");
    }

    public override void CheckSwitchStates()
    {
        if (!playerContext.IsDashPressed)
        {
            SwitchState(new PlayerIdleState(playerContext));
        }
    }
}
