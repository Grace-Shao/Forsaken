using UnityEngine;

public class StageOne : State
{
    private BossStateMachine bossContext;
    public StageOne(BossStateMachine currentContext) : base(currentContext)
    {
        
        bossContext = currentContext;
        isBaseState = true;
        InitializeSubStates();
    }
    public override void InitializeSubStates()
    {
        if (bossContext.canDash())
        {
            SetSubState(new BossDashWindupState(bossContext));
        } else if (bossContext.InRange())
        {   
            SetSubState(new BossAttackState(bossContext));
        } else
        {
            SetSubState(new BossIdleState(bossContext));
        }
    }
    public override void EnterState()
    {
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
        if (bossContext.IsTransitioning)
        {
            SwitchState(new BossTransitionState(bossContext));
        }
        
    }
}
