using UnityEngine;
public class EvaStateMachine : StateMachine
{
    [SerializeField] private float followDistance;
    [SerializeField] private float timeInIdle;

    private bool isFlipped = false;
    private bool isHurt = false; 
    private bool isTransitioning = false;
    private int hurtFinished = 0;
    private int health;
    

    public bool IsHurt{get {return isHurt;} set {isHurt = value;}}
    public bool IsTransitioning {get {return isTransitioning;} set {isTransitioning = value;}}
    public int HurtFinished {get {return hurtFinished; } set {hurtFinished = value;}}
    public int Health {get {return health;} set {health = value;}}
    public float FollowDistance {get {return followDistance;}}

    protected override void Init()
    {
        base.Init();
        sprite = transform.Find("Sprite");
        Health = 100;
    }

    protected override void EnterBeginningState()
    {
        IsTransitioning = false;
        currentState = new EvaIdleState(this);
        currentState.EnterStates();
    }

    protected override void UpdateState()
    {
        if (!IsTransitioning)
        {
            rb.linearVelocity = appliedMovement;
        }
        currentState.UpdateStates();
    }

    protected override void FaceMovement()
    {
        Vector3 flipped = sprite.localScale;
        flipped.x *= -1f;
        if (sprite.position.x < player.transform.position.x && isFlipped)
        {
            sprite.localScale = flipped;
            isFlipped = false;
        } else if (sprite.position.x > player.transform.position.x && !isFlipped)
        {
            sprite.localScale = flipped;
            isFlipped = true;
        }
    }

    public bool FollowRange()
    {
        return Vector3.Distance(transform.position,Player.transform.position) >= FollowDistance;
    }
}