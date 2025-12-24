using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateManager currentContext, PlayerStateFactory playerFactory) : base(currentContext, playerFactory){}
    public override void EnterState()
    {
        Debug.Log("Player is Idle");
        context.PlayerAnimator.SetBool("isIdle", true);
        context.AppliedMovementX = 0f;
        context.AppliedMovementY = 0f;
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState()
    {
        context.PlayerAnimator.SetBool("isIdle", false);
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
        } else if (context.IsMovementPressed && context.IsRunPressed)
        {
            SwitchState(factory.Run());
        } else if (context.IsMovementPressed)
        {   
            SwitchState(factory.Walk());
        } 
    }
}
