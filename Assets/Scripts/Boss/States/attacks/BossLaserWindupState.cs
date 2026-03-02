using UnityEngine;
public class BossLaserWindupState : State
{
    private BossStateMachine bossContext;

    public BossLaserWindupState(BossStateMachine currentContext) : base(currentContext)
    {
        bossContext = currentContext;
    }

    public override void EnterState()
    {
        //bossContext.flashCharacter();
        bossContext.Anim.SetTrigger("laser_windup");
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        bossContext.Anim.ResetTrigger("laser_windup");
    }

    public override void CheckSwitchStates()
    {
        if (bossContext.LasersFinished == 1)
        {
            SwitchState(new BossLaserState(bossContext));
        }
    }

}