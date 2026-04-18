using UnityEngine;
public class BossStunState : State
{
    private BossStateMachine bossContext;
    public BossStunState(BossStateMachine currentContext) : base(currentContext)
    {
        bossContext = currentContext;
        isBaseState = true;
    }
    public override void EnterState()
    {
        bossContext.RB.gravityScale = 5f;
        bossContext.Anim.SetTrigger("stun");
        bossContext.AppliedMovementX = 0f;
        bossContext.AppliedMovementY = 0f;
    }
    public override void UpdateState()
    {
        // CheckSwitchStates();
    }
    public override void ExitState()
    {
        bossContext.IsStunned = false;
        bossContext.Anim.ResetTrigger("stun");
    }

    public override void CheckSwitchStates()
    {
        // if (bossContext.Health <= 0f)
        // {
        //     bossContext.BossDeath?.Invoke();
        // } 
    }
}
