using UnityEngine;

public class PlayerSlashState : PlayerBaseState
{
    public PlayerSlashState(PlayerStateManager currentContext, PlayerStateFactory playerFactory) : base(currentContext, playerFactory){}
    public override void EnterState()
    {
        Debug.Log("player is attacking");
        context.PlayerAnimator.SetBool("isSlashing", true);
        context.AppliedMovementX = 0f;
        // context.AppliedMovementY = 0f;
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState()
    {
        context.PlayerAnimator.SetBool("isSlashing", false);
    }

    public override void CheckSwitchStates()
    {
        if (context.IsHurt)
        {
            SwitchState(factory.Hurt());
        }
        else if (!context.SlashFinished)
        {
            return;
        }
        context.SlashFinished = false; 
        if (context.IsMovementPressed && context.IsRunPressed)
        {
            SwitchState(factory.Run());
        } else if (context.IsMovementPressed)
        {   
            SwitchState(factory.Walk());
        } else
        {
            SwitchState(factory.Idle());
        }
    }
}
