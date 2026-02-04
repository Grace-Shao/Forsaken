using UnityEngine;

public class PlayerBlockState : State
{
    private PlayerStateMachine playerContext;
    private bool allowParry = true;
    public PlayerBlockState(PlayerStateMachine currentContext, bool allowParry = true) : base(currentContext)
    {
        playerContext = currentContext;
        isBaseState = true;
        this.allowParry = allowParry;

    }
    public override void EnterState()
    {
        if (allowParry) playerContext.StartParryCooldown();
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState()
    {
        // playerContext.CanMove = true;
        playerContext.CanParry = false;
    }

    public override void CheckSwitchStates()
    {
        if (playerContext.IsParrying && allowParry) {
            SwitchState(new PlayerParryState(playerContext));
        }
        
        if (!playerContext.IsBlocking) {
            if (playerContext.IsMovementPressed) {
                if (playerContext.IsRunPressed) {
                    SwitchState(new PlayerRunState(playerContext));
                }
                else {
                    SwitchState(new PlayerWalkState(playerContext));
                }
            }
            else {
                SwitchState(new PlayerIdleState(playerContext));
            }
        }
        
    }
}