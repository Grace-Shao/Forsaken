using UnityEngine;

public class PlayerRunState : State
{
    private PlayerStateMachine playerContext;
    public PlayerRunState(PlayerStateMachine currentContext) : base(currentContext)
    {
        playerContext = currentContext;
        isBaseState = true;
    }
    public override void EnterState()
    {
        playerContext.Anim.SetBool("isRunning", true);
        playerContext.AppliedMovementX = playerContext.CurrentMovementInput.x * playerContext.RunSpeed;
        
    }
    public override void UpdateState()
    {
        playerContext.AppliedMovementX = playerContext.CurrentMovementInput.x * playerContext.RunSpeed;
        
        CheckSwitchStates();
    }
    public override void ExitState()
    {
        playerContext.Anim.SetBool("isRunning", false);
    }

    public override void CheckSwitchStates()
    {
        if (playerContext.IsHurt)
        {
            SwitchState(new PlayerHurtState(playerContext));
        }
        else if (playerContext.IsHitPressed)
        {
            SwitchState(new PlayerAttackState(playerContext));
        }
        else if (playerContext.IsShootPressed)
        {
            SwitchState(new PlayerShootState(playerContext));
        } else if (playerContext.Grounded && playerContext.IsJumpPressed)
        {
            SwitchState(new PlayerJumpState(playerContext));
        }  else if (!playerContext.IsMovementPressed)
        {
            SwitchState(new PlayerIdleState(playerContext));
        } else if (playerContext.IsMovementPressed && !playerContext.IsRunPressed)
        {   
            SwitchState(new PlayerWalkState(playerContext));
        }
    }
}
