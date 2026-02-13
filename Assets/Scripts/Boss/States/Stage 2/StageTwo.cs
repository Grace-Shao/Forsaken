using UnityEngine;

public class StageTwo : State
{
    private BossStateMachine bossContext;
    public StageTwo(BossStateMachine currentContext) : base(currentContext)
    {
        
        bossContext = currentContext;
        isBaseState = true;
        InitializeSubStates();
    }
    public override void InitializeSubStates()
    {
        // if (bossContext.GrappleInRange())
        // {
        //     SetSubState(new BossGrappleState(bossContext));
        // }
        // else 
        if (bossContext.InRange())
        {
            SetSubState(new BossAttackState(bossContext));
        } else if (bossContext.canDash())
        {   
            SetSubState(new BossDashWindupState(bossContext));
        } else
        {   
            SetSubState(new BossIdleState(bossContext));
        } 
    }
    public override void EnterState()
    {
        bossContext.GrapplingFinished = 1;
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
