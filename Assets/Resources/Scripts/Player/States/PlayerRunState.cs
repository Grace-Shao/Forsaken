using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateManager currentContext, PlayerStateFactory playerFactory) : base(currentContext, playerFactory){}
    public override void EnterState()
    {
        Debug.Log("player is running");
        context.PlayerAnimator.SetBool("isRunning", true);
        context.AppliedMovementX = context.CurrentMovement.x * context.RunSpeed;
        
    }
    public override void UpdateState()
    {
        context.AppliedMovementX = context.CurrentMovement.x * context.RunSpeed;
        
        CheckSwitchStates();
    }
    public override void ExitState()
    {
        context.PlayerAnimator.SetBool("isRunning", false);
    }

    public override void CheckSwitchStates()
    {
        if (context.IsHurt)
        {
            SwitchState(factory.Hurt());
        }
        else if (context.IsHitPressed)
        {
            SwitchState(factory.Slash());
        } else if (context.Grounded && context.IsJumpPressed)
        {
            SwitchState(factory.Jump());
        }  else if (!context.IsMovementPressed)
        {
            SwitchState(factory.Idle());
        } else if (context.IsMovementPressed && !context.IsRunPressed)
        {   
            SwitchState(factory.Walk());
        }
    }
}
