using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateManager currentContext, PlayerStateFactory playerFactory) : base(currentContext, playerFactory){}
    public override void EnterState()
    {
        Debug.Log("player is walking");
        context.PlayerAnimator.SetBool("isWalking", true);
        context.AppliedMovementX = context.CurrentMovement.x * context.MoveSpeed;
    }
    public override void UpdateState()
    {
        context.AppliedMovementX = context.CurrentMovement.x * context.MoveSpeed;
        
        CheckSwitchStates();
    }
    public override void ExitState()
    {
        context.PlayerAnimator.SetBool("isWalking", false);
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
        } 
        else if (!context.IsMovementPressed )
        {
            SwitchState(factory.Idle());
        } else if (context.IsMovementPressed && context.IsRunPressed)
        {   
            SwitchState(factory.Run());
        }
    }
}
