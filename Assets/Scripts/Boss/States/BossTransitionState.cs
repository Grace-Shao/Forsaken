using UnityEngine;
public class BossTransitionState : State
{
    private BossStateMachine bossContext;
    private string triggerName = "";
    public BossTransitionState(BossStateMachine currentContext) : base(currentContext)
    {
        bossContext = currentContext;
        isBaseState = true;
    }
    public override void EnterState()
    {  
        bossContext.ClearLineRenderers();
        bossContext.IntroFinished = 0;
        if (bossContext.CurrentStage == 1)
        {
            triggerName = "phaseOne";
            bossContext.Anim.SetTrigger("phaseOne");
        } else if (bossContext.CurrentStage == 2)
        {
            triggerName = "phaseTwo";
           bossContext.Anim.SetTrigger("phaseTwo");
        } else if (bossContext.CurrentStage == 3)
        {
            triggerName = "phaseThree";
            bossContext.Anim.SetTrigger("phaseThree");
        } else
        {
            triggerName = "final";
            bossContext.Anim.SetTrigger("final");
        }
        bossContext.AppliedMovementX = 0f;
        bossContext.AppliedMovementY = 0f;
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState()
    {
        bossContext.IntroFinished = 1;
        bossContext.IsTransitioning = false;
        bossContext.AppliedMovementX = 0f;
        bossContext.AppliedMovementY = 0f;
        bossContext.Anim.ResetTrigger(triggerName);
    }

    public override void CheckSwitchStates()
    {
        if (bossContext.IntroFinished == 1)
        {
            if (bossContext.CurrentStage == 1)
            {
                SwitchState(new StageOne(bossContext));
            }
            else if (bossContext.CurrentStage == 2)
            {
                SwitchState(new StageTwo(bossContext));
            } else if (bossContext.CurrentStage == 3)
            {
                SwitchState(new StageThree(bossContext));
            } else
            {
                SwitchState(new BossFinalState(bossContext));
            }
        }
    }
}
