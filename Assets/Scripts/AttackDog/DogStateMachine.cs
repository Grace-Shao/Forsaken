using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System;
public class DogStateMachine : StateMachine, IDamageable
{
    [Header("Attack Controls")]
    [SerializeField] private float targetDistance;
    [SerializeField] private float aggroDistance;
    [SerializeField] private float stunTime;
    [SerializeField] private float stunInterval;
    [SerializeField] private int damage;
    [SerializeField] private float damageCooldown;
    [SerializeField] private float jumpForceX;
    [SerializeField] private float jumpForceY;
    [SerializeField] private int maxHealth = 50;
    [SerializeField] private bool isEndless = false;
    [SerializeField] private Image healthBarFill;
    
    private TestEvaStateMachine eva;
    private bool isFlipped = false;
    private bool isStunned = false;
    private bool inAttack = false;
    private bool onGround = false;
    private bool windUpFinished = false;
    private int health;
    private ParticleSystem damageTakenParticles;
    private ParticleSystem attackIndicator;
    public bool IsStunned {get {return isStunned;} set {isStunned = value;}}
    public bool WindUpFinished { get {return windUpFinished;} set { windUpFinished = value; } }
    public bool InAttack {get {return inAttack; } set {inAttack = value;}}
    public bool OnGround {get { return onGround; } set { onGround = value; } }
    public bool Flipped { get {return isFlipped;}}
    public int Health {get {return health;} set {health = value;}}
    public int Damage {get {return damage;} set {damage = value;}}
    public Vector2 JumpForce {get {return new Vector2(jumpForceX, jumpForceY);}}
    public float Cooldown {get {return damageCooldown;} set {damageCooldown = value;}}
    public float StunTime {get {return stunTime;}}
    public float StunInterval {get {return stunInterval;}}
    public float TargetDistance {get {return targetDistance;}}
    public float AggroDistance {get {return aggroDistance;} set {aggroDistance = value;}}

    public bool IsEndless {get {return isEndless;}}
    public TestEvaStateMachine Eva {get {return eva;}}
    public Action<DogStateMachine> DogDeath;
    protected override void Init()
    {
        base.Init();
        sprite = transform.Find("Sprite");
        Health = maxHealth;
        damageTakenParticles = sprite.Find("hit received particles").GetComponent<ParticleSystem>();
        attackIndicator = sprite.Find("HeadTop").Find("Attack Indicator").GetComponent<ParticleSystem>();

        if (isEndless)
        {
            eva = GameObject.FindGameObjectWithTag("Eva").GetComponent<TestEvaStateMachine>();
        }
        EnterBeginningState();
    }

    protected override void EnterBeginningState()
    {
        currentState = new DogStartState(this);
        currentState.EnterStates();
    }

    protected override void UpdateState()
    {
        if (!inAttack)
        {
            rb.linearVelocity = appliedMovement;
        }
        else
        {
            rb.AddForce(appliedMovement, ForceMode2D.Impulse);
        }
        currentState.UpdateStates();
    }

    protected override void FaceMovement()
    {
        Vector3 flipped = sprite.localScale;
        flipped.x *= -1f;
        float targetX = player.transform.position.x;
        if (isEndless)
        {
            targetX = eva.transform.position.x;
        }
        if (sprite.position.x < targetX && isFlipped)
        {
            sprite.localScale = flipped;
            isFlipped = false;
        }
        else if (sprite.position.x > targetX && !isFlipped)
        {
            sprite.localScale = flipped;
            isFlipped = true;
        }
    }
    public void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log($"Dog physically collided with: {other.gameObject.name} (Tag: {other.gameObject.tag})");
        if (other.gameObject.CompareTag("Player"))
        {
            player.gameObject.GetComponent<PlayerStateMachine>().ApplyDamage(Damage);
        }
        if (isEndless && other.gameObject.CompareTag("Eva"))
        {
            eva.gameObject.GetComponent<TestEvaStateMachine>().ApplyDamage(Damage);
        }
        if (other.gameObject.CompareTag("Ground"))
        {
            onGround = true;
        }
    }

    public void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            onGround = false;
        }
    }

    public void flashCharacter()
    {
        sprite.GetComponent<DamageFlash>().BeginFlash();
    }

    public void ApplyDamage(int damage)
    {
        Health -= damage;
        Debug.Log("Enemy Health: " + Health);
        flashCharacter();
        healthBarFill.fillAmount = Health / (float)maxHealth;
        damageTakenParticles.Play();
        if (Health <= 0)
        {
            AggroEnd?.Invoke(this);
            DogDeath?.Invoke(this);
            gameObject.SetActive(false);
        }
    }

    public bool InRange()
    {
        return Mathf.Abs(transform.position.x - Player.transform.position.x) <= TargetDistance;
    }
    public bool InAggroRange()
    {
        return Mathf.Abs(transform.position.x - Player.transform.position.x) <= aggroDistance;
    }

    public void onWindupStart()
    {
        windUpFinished = false;
    }

    public void onWindupEnd()
    {
        windUpFinished = true;
    }

    public void OnAttackStart()
    {
        inAttack = true;

    }

    public void OnAttackEnd()
    {
        inAttack = false;
    }

    public void AttackIndicator()
    {
        attackIndicator.Play();
    }
    
    public void Attack()
    {
        currentState.SwitchState(new DogWalkState(this));
    }

}