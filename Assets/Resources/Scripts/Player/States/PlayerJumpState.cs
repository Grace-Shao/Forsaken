using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerStateManager currentContext, PlayerStateFactory playerFactory) : base(currentContext, playerFactory){}
    public override void EnterState()
    {
        Debug.Log("player is jumping");
        context.PlayerAnimator.SetBool("isJumping", true);
        context.Grounded = false;
        context.AppliedMovementX = context.CurrentMovement.x * context.MoveSpeed / 3f;
    }
    public override void UpdateState()
    {
        context.AppliedMovementX = context.CurrentMovement.x * context.MoveSpeed;
        context.AppliedMovementY = 0f ;
        CheckSwitchStates();
    }
    public override void ExitState()
    {
        context.PlayerAnimator.SetBool("isJumping", false);
        context.Grounded = true;
    }

    public override void CheckSwitchStates()
    {
        if (context.IsHurt)
        {
            SwitchState(factory.Hurt());
        } else if (context.Grounded && !context.IsMovementPressed )
        {
            SwitchState(factory.Idle());
        } else if (context.Grounded && !context.IsRunPressed)
        {
            SwitchState(factory.Walk());
        } else if (context.Grounded && context.IsRunPressed)
        {
            SwitchState(factory.Run());
        }
    }
}
