using UnityEngine;

public class PlayerWalkState : State
{
    private PlayerStateMachine playerContext;
    public PlayerWalkState(PlayerStateMachine currentContext) : base(currentContext)
    {
        playerContext = currentContext;
        isBaseState = true;
    }
    public override void EnterState()
    {
        playerContext.Anim.SetBool("isWalking", true);
        playerContext.AppliedMovementX = playerContext.CurrentMovementInput.x * playerContext.MoveSpeed;
    }
    public override void UpdateState()
    {
        playerContext.AppliedMovementX = playerContext.CurrentMovementInput.x * playerContext.MoveSpeed;
        
        CheckSwitchStates();
    }
    public override void ExitState()
    {
        playerContext.Anim.SetBool("isWalking", false);
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
        } 
        else if (!playerContext.IsMovementPressed )
        {
            SwitchState(new PlayerIdleState(playerContext));
        } else if (playerContext.IsMovementPressed && playerContext.IsRunPressed)
        {   
            SwitchState(new PlayerRunState(playerContext));
        }
    }
}
