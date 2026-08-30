using UnityEngine;

public class BossMeleeAttackState : State
{
    private BossStateMachine bossContext;
    public BossMeleeAttackState(BossStateMachine currentContext) : base(currentContext)
    {
        
        bossContext = currentContext;
    }
    public override void EnterState()
    {
        bossContext.TimeInState = 0f;
        bossContext.AttackFinished = 0;
        bossContext.Anim.SetTrigger("melee");
        bossContext.AppliedMovementX = 0f;
        AudioControl.Instance.PlaySFX("HUESlash", bossContext.gameObject);
    }
    public override void UpdateState()
    {
        bossContext.TimeInState += Time.deltaTime;
        CheckSwitchStates();
    }
    public override void ExitState()
    {
        bossContext.AttackFinished = 1;
        bossContext.Anim.ResetTrigger("melee");
    }

    public override void CheckSwitchStates()
    {
        if (bossContext.StateTimedOut || bossContext.AttackFinished == 1)
        {
            SwitchState(new BossIdleState(bossContext));
        }
    }
}
