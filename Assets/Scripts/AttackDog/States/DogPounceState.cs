using UnityEngine;

public class DogPounceState : State
{
    private DogStateMachine dogContext;
    private float curTimeInState;
    public DogPounceState(DogStateMachine currentContext) : base(currentContext)
    {
        
        dogContext = currentContext;
        isBaseState = true;
    }
    public override void EnterState()
    {
        Vector3 target;
        if (dogContext.IsEndless)
        {
            target = new Vector3(dogContext.Eva.gameObject.transform.position.x, dogContext.RB.gameObject.transform.position.y, 0f);    
        }
        else 
        {
            target = new Vector3(dogContext.Player.gameObject.transform.position.x, dogContext.RB.gameObject.transform.position.y, 0f);
        }
        Vector3 currentPos = new Vector3(dogContext.RB.gameObject.transform.position.x, dogContext.RB.gameObject.transform.position.y, 0f);
        Vector3 direction = (target - currentPos).normalized;
        curTimeInState = 0f;
        dogContext.InAttack = true;
        dogContext.OnGround = false;
        dogContext.RB.AddForce(new Vector2(direction.x * dogContext.JumpForce.x, dogContext.JumpForce.y), ForceMode2D.Impulse);
        dogContext.AppliedMovementX = 0;
        dogContext.AppliedMovementY = 0;
    }
    public override void UpdateState()
    {
        Debug.Log("updating");
        curTimeInState += Time.deltaTime;
        dogContext.AppliedMovementY = 0f;
        CheckSwitchStates();
    }
    public override void ExitState()
    {
        dogContext.InAttack = false;
        dogContext.OnGround = true;
    }

    public override void CheckSwitchStates()
    {
        if ((!dogContext.InAttack && dogContext.OnGround) || (curTimeInState > dogContext.StunTime))
        {
            Debug.Log("switching");
            SwitchState(new DogStunState(dogContext));
        }
    }
}
