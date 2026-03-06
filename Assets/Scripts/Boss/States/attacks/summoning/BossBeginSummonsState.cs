using UnityEngine;
public class BossBeginSummonsState : State
{
    private BossStateMachine bossContext;
    public BossBeginSummonsState(BossStateMachine currentContext) : base(currentContext)
    {
        bossContext = currentContext;
    }
    public override void EnterState()
    {
        bossContext.LastDroneSummon = Time.time;
        bossContext.AppliedMovementX = 0;
        bossContext.AttackFinished = 0;
        bossContext.Anim.SetTrigger("walk");
        Debug.Log("Attemping Summon Here!");
        TargetDistance 
    }
    public override void UpdateState()
    {
        //Walking over to summon spot
        Vector3 target = new Vector3(bossContext.SummonPos.gameObject.transform.position.x, bossContext.SummonPos.gameObject.transform.position.y, 0f);
        Vector3 currentPos = new Vector3(bossContext.RB.gameObject.transform.position.x, bossContext.RB.gameObject.transform.position.y, 0f);
        Vector3 direction = (target - currentPos).normalized;
        bossContext.AppliedMovementX = direction.x * bossContext.MoveSpeed;
        CheckSwitchStates();
    }
    public override void ExitState()
    {
        bossContext.Anim.ResetTrigger("walk");
    }

    public override void CheckSwitchStates()
    {
        if (bossContext.RB.gameObject.transform.position - bossContext.SummonPos.gameObject.transform.position )
            SwitchState(new BossSummonState(bossContext));
    }
}
