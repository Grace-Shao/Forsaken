using UnityEngine;

public class PlayerParryState : State
{
    private PlayerStateMachine playerContext;
    public PlayerParryState(PlayerStateMachine currentContext) : base(currentContext)
    {
        playerContext = currentContext;
        isBaseState = true;
    }
    public override void EnterState()
    {
        Debug.Log("Parrying");
        playerContext.CanParry = true;
        // TODO: parry animation
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState()
    {        
        Debug.Log("Parried");
    }

    public override void CheckSwitchStates() {
        if (playerContext.IsParrying) return;
        if (playerContext.IsBlocking) {
            SwitchState(new PlayerBlockState(playerContext, false)); // don't allow repeated parries - reblock
        }

        else if (playerContext.IsMovementPressed) {
            if (playerContext.IsRunPressed) {
                SwitchState(new PlayerRunState(playerContext));
            }
            else {
                SwitchState(new PlayerWalkState(playerContext));
            }
        }
        
        else SwitchState(new PlayerIdleState(playerContext));
        
    }
}