using UnityEngine;
public class BossDashWindupState : State
{
    private BossStateMachine bossContext;
    public BossDashWindupState(BossStateMachine currentContext) : base(currentContext)
    {
        bossContext = currentContext;
    }
    public override void EnterState()
    {
        bossContext.AttackFinished = 0;
        bossContext.IsDashing = true;
        bossContext.WindUpFinished = false;
        bossContext.Anim.SetTrigger("charge");
        bossContext.AppliedMovementX = 0;
        bossContext.LastDashTime = Time.time;
        bossContext.TimeInState = 0f;
    }
    public override void UpdateState()
    {
        bossContext.TimeInState += Time.deltaTime;
        CheckSwitchStates();
    }
    public override void ExitState()
    {
        
        bossContext.IsDashing = false;
        bossContext.WindUpFinished = false;
        bossContext.Anim.ResetTrigger("charge");
    }

    public override void CheckSwitchStates()
    {
        if (bossContext.IsStunned)
        {
            SwitchState(new BossStunState(bossContext));
        }
        else if (bossContext.StateTimedOut || bossContext.WindUpFinished == true)
        {
            SwitchState(new BossChargedDashState(bossContext));
        }
    }
}
