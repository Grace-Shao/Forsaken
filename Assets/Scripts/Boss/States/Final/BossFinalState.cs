using UnityEngine;

public class BossFinalState : State
{
    private BossStateMachine bossContext;
    public BossFinalState(BossStateMachine currentContext) : base(currentContext)
    {
        
        bossContext = currentContext;
        isBaseState = true;
        InitializeSubStates();
    }
    public override void InitializeSubStates()
    {
        if (bossContext.GrapplingFinished == 0)
        {   
            SetSubState(new BossBeginUltimateState(bossContext));
        } 
        else 
        {
            SetSubState(new BossUltimateState(bossContext));
        }
    }
    public override void EnterState()
    {
        bossContext.InUltimate = true;
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState()
    {
        Debug.Log("exitting state");
        bossContext.InUltimate = false;
    }

    public override void CheckSwitchStates()
    {
    }
}
