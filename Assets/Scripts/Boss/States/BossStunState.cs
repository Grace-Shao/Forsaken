using UnityEditor.Experimental.GraphView;
using UnityEngine;
public class BossStunState : State
{
    private BossStateMachine bossContext;
    private float curTime;
    public BossStunState(BossStateMachine currentContext) : base(currentContext)
    {
        bossContext = currentContext;
        isBaseState = true;
    }
    public override void EnterState()
    {
        Debug.Log("currently stunned");
        bossContext.Anim.Play("Idle");
        bossContext.AppliedMovementX = 0f;
        bossContext.AppliedMovementY = 0f;
        curTime = 0f;
    }
    public override void UpdateState()
    {
        curTime += Time.deltaTime;
        CheckSwitchStates();
    }
    public override void ExitState()
    {
        bossContext.IsStunned = false;
    }

    public override void CheckSwitchStates()
    {
        if (curTime > bossContext.StunTime)
        {
            SwitchState(new BossTransitionState(bossContext));
        } 
    }
}
