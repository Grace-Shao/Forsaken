using UnityEngine;

public class BossShootState : State
{
    private BossStateMachine bossContext;
    public BossShootState(BossStateMachine currentContext) : base(currentContext)
    {
        bossContext = currentContext;
    }
    public override void EnterState()
    {
        bossContext.TimeInState = 0f;
        bossContext.Anim.SetTrigger("shoot");
        bossContext.AppliedMovementX = 0f;
        bossContext.AppliedMovementY = 0f;

    }
    public override void UpdateState()
    {
        bossContext.TimeInState += Time.deltaTime;
        // Call Shoot on the ranged weapon
        
        if (bossContext.RangedWeapon != null && bossContext.ShootStarted)
        {
            Debug.Log("shooting");
            bossContext.RangedWeapon.Shoot();
            bossContext.ShootStarted = false;
        }
        CheckSwitchStates();
    }
    public override void ExitState()
    {
        bossContext.ShootFinished = false; 
        bossContext.Anim.ResetTrigger("shoot");
        bossContext.ShootStarted = false;
    }

    public override void CheckSwitchStates()
    {
        if (bossContext.StateTimedOut || bossContext.ShootFinished)
        {
            SwitchState(new BossIdleState(bossContext));
        }
    }
}
