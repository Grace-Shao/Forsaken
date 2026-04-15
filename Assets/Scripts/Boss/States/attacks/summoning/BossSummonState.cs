
using UnityEngine;
public class BossSummonState : State
{
    private BossStateMachine bossContext;
    private GameObject attackDog;
    private GameObject attackCrow;

    private Transform t;

    public BossSummonState(BossStateMachine currentContext) : base(currentContext)
    {
        bossContext = currentContext;
        attackDog = bossContext.AttackDog;
        attackCrow = bossContext.AttackCrow;
        t = bossContext.RB.gameObject.transform;
    }

    public override void EnterState(){
        GameObject dog;
        GameObject crow;
        bossContext.CurEnemies += 1;
        bossContext.Anim.SetTrigger("summon");
        float randomChance = Random.Range(0f, 1f);
        if (attackDog != null && randomChance < 0.5f) {
            dog = Object.Instantiate(attackDog, t.position, t.rotation);
            dog.GetComponent<DogStateMachine>().Attack();
        } else if (attackCrow != null)
        {
            crow = Object.Instantiate(attackCrow, t.position, t.rotation);
            crow.transform.position = new Vector3(t.position.x, t.position.y + 5f, t.position.z);
            crow.GetComponent<CrowStateMachine>().Attack();
        }
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        bossContext.LastDroneSummon = Time.time;
        bossContext.Anim.ResetTrigger("summon");
    }

    //fill in transition logic
    public override void CheckSwitchStates()
    {
        SwitchState(new BossIdleState(bossContext));
    }

}