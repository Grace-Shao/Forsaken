using UnityEngine;
public class BossChargedDashState : State
{
    private BossStateMachine bossContext;
    public BossChargedDashState(BossStateMachine currentContext) : base(currentContext)
    {
        bossContext = currentContext;
    }
    public override void EnterState()
    {
        bossContext.AttackFinished = 0;
        bossContext.Anim.SetTrigger("dashAttack");
        bossContext.AppliedMovementX = ((bossContext.Flipped ? -1 : 1)) * bossContext.MoveSpeed * 3;
        Physics2D.IgnoreLayerCollision(6, 7, true);
        Physics2D.IgnoreLayerCollision(7, 8, true);
        bossContext.LastDashTime = Time.time;
        bossContext.IsDashing = true;
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState()
    {
        bossContext.Anim.ResetTrigger("dashAttack");
        bossContext.AttackFinished = 0;
        bossContext.IsDashing = false;
        Physics2D.IgnoreLayerCollision(6, 7, false);
        Physics2D.IgnoreLayerCollision(7, 8, false);
    }

    public override void CheckSwitchStates()
    {
        if (bossContext.AttackFinished == 1)
        {
            SwitchState(new BossIdleState(bossContext));
        }
    }
}
