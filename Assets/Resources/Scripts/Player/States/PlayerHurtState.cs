using UnityEngine;

//fill this in after basic enemy is created
public class PlayerHurtState : PlayerBaseState
{
    public PlayerHurtState(PlayerStateManager currentContext, PlayerStateFactory playerFactory) : base(currentContext, playerFactory){}
    public override void EnterState()
    {
        Debug.Log("player is hurt");
        context.PlayerAnimator.SetBool("isHurt", true);
        context.AppliedMovementX = 0f;
        context.AppliedMovementY = 0f;
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState()
    {
        context.PlayerAnimator.SetBool("isHurt", false);
    }

    public override void CheckSwitchStates()
    {
        if (!context.HurtFinished)
        {
            return;
        }
        context.HurtFinished = false;
        if (context.IsHitPressed)
        {
            SwitchState(factory.Slash());
        }
        else if (context.IsMovementPressed && context.IsRunPressed)
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
