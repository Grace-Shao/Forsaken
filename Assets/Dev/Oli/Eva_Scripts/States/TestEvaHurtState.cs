using UnityEngine;
public class TestEvaHurtState : State
{
    private TestEvaStateMachine evaContext;
    private float hurtDuration = 0.3f; 
    private float timer;
    public TestEvaHurtState(TestEvaStateMachine currentContext) : base(currentContext)
    {
        evaContext = currentContext;
        isBaseState = true;
    }
    public override void EnterState()
    {
        //evaContext.CanMove = false;
        //evaContext.Anim.SetTrigger("hurt");
        timer = 0f;
        evaContext.AppliedMovementX = 0f;
        evaContext.AppliedMovementY = 0f;
    }
    public override void UpdateState()
    {
        timer += Time.deltaTime;

        if (timer >= hurtDuration)
        {
            CheckSwitchStates();
        }
    }

    public override void ExitState()
    {
        //evaContext.Anim.ResetTrigger("hurt");
        evaContext.HurtFinished = false;
        evaContext.IsHurt = false;
        //evaContext.CanMove = true;
    }

    public override void CheckSwitchStates()
    {

        SwitchState(new TestEvaIdleState(evaContext));
    }
}
