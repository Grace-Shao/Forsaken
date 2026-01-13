using UnityEngine;
public class BossStateMachine : StateMachine, IDamageable
{
    [Header("Object References")]
    [SerializeField] private GameManager manager;

    [Header("Attack Controls")]
    [SerializeField] private  float targetDistance;
    [SerializeField] private  float timeInIdle;
    [SerializeField] private  int damage;
    [SerializeField] private float damageCooldown;

    [Header("Grapple Settings")]
    [SerializeField] private float grappleTargetDistance;
    [SerializeField] private float grappleDuration;
    [SerializeField] private float grappleSpeed;

    private GameObject swordPoint;
    
    private bool isFlipped = false;
    private int grapplingFinished = 0;
    private int attackFinished = 0;
    private int hurtFinished = 0;
    private int introFinished = 0;
    private int health;
    
    public bool FightStarted {get {return manager.FightStarted;}}
    public bool IsTransitioning {get {return manager.IsTransitioning;} set {manager.IsTransitioning = value;}}
    public int GrapplingFinished {get {return grapplingFinished;} set {grapplingFinished = value;}}
    public int AttackFinished {get {return attackFinished; } set {attackFinished = value;}}
    public int HurtFinished {get {return hurtFinished; } set {hurtFinished = value;}}
    public int IntroFinished {get {return introFinished; } set {introFinished = value;}}
    public int Health {get {return health;} set {health = value;}}
    public int Damage {get {return damage;} set {damage = value;}}
    public float Cooldown {get {return damageCooldown;} set {damageCooldown = value;}}
    public float TimeInIdle {get {return timeInIdle;}}
    public float TargetDistance {get {return targetDistance;}}
    public float GrappleDuration {get {return grappleDuration;}}
    public float GrappleSpeed {get {return grappleSpeed;}}
    public float GrappleTargetDistance {get {return grappleTargetDistance;}}
    public int CurrentStage {get {return manager.CurrentStage;} set {manager.CurrentStage = value;}}

    protected override void Init()
    {
        base.Init();
        sprite = transform.Find("Sprite");
        Health = 100;
        swordPoint = sprite.Find("Broadsword").Find("sword trail tracker").gameObject;
    }

    protected override void EnterBeginningState()
    {
        IsTransitioning = false;
        currentState = new BossStartState(this);
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
    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.transform == player)
        {
            player.gameObject.GetComponent<PlayerStateMachine>().ApplyDamage(Damage);
        }
    }

    public void ApplyDamage(int damage)
    {
        if (IntroFinished == 1 && manager.FightStarted)
        {
            Health -= damage;
            Debug.Log("Enemy Health: " + Health);
            sprite.GetComponent<DamageFlash>().BeginFlash();
            if (!player.gameObject.GetComponent<PlayerStateMachine>().IsDashing && player.gameObject.GetComponent<PlayerStateMachine>().DashUnlocked)
            {
                player.gameObject.GetComponent<PlayerStateMachine>().CurrentDashMeter += 1;
                Debug.Log("dash meter is: " + player.gameObject.GetComponent<PlayerStateMachine>().CurrentDashMeter);
            }
            
        }
        if (Health <= 0f)
        {
            manager.CheckWinStatus();
        }
    }



    public bool InRange()
    {
        return Vector3.Distance(transform.position,Player.transform.position) <= TargetDistance;
    }

    public bool GrappleInRange()
    {
        return Vector2.Distance(transform.position, Player.transform.position) > GrappleTargetDistance;
    }

    public void OnAttackStart()
    {
        attackFinished = 0;
        swordPoint.SetActive(true);

    }

    public void OnAttackEnd()
    {
        attackFinished = 1;
        swordPoint.SetActive(false);

    }
    

}