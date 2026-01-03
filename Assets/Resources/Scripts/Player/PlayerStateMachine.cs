using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerStateMachine : StateMachine, IDamageable
{
    //control variables
    [SerializeField] private  float runSpeed = 7f;

    //player input system
    private PlayerInput playerInput;
    private Vector2 currentMovementInput;
    private bool isMovementPressed;
    private bool isRunPressed;
    private bool isJumpPressed;
    private bool isHitPressed;
    private bool isShootPressed;
    private bool isHurt; 
    private bool attackFinished = false;
    private bool shootStarted = false;
    private bool shootFinished = false;
    private bool hurtFinished = false;
    private bool grounded = true;

    //player info
    private int health;
    private float damageCooldown;
    private float canTakeDamage;

    //getters and settesr
    public bool IsMovementPressed {get {return isMovementPressed;} set {isMovementPressed = value;}}
    public bool IsRunPressed {get {return isRunPressed;} set {isRunPressed = value;}}
    public bool IsJumpPressed {get {return isJumpPressed;} set {isJumpPressed = value;}}
    public bool IsHitPressed {get {return isHitPressed;} set {isHitPressed = value;}}
    public bool IsShootPressed {get {return isShootPressed;} set {isShootPressed = value;}}
    public bool IsHurt{get {return isHurt;} set {isHurt = value;}}
    public bool AttackFinished {get {return attackFinished; } set {attackFinished = value;}}
    public bool ShootStarted {get {return shootStarted; } set {shootStarted = value;}}
    public bool ShootFinished {get {return shootFinished; } set {shootFinished = value;}}

    public bool HurtFinished {get {return hurtFinished; } set {hurtFinished = value;}}
    public bool Grounded {get {return grounded;} set {grounded = value;}}
    public Vector2 CurrentMovementInput {get {return currentMovementInput;}}
    public float RunSpeed {get {return runSpeed;}}
    public int Health {get {return health;} set {health = value;}}
    public float Cooldown {get {return damageCooldown;} set {damageCooldown = value;}}

    protected override void Init()
    {
        base.Init();

        //set reference variables
        playerInput = new PlayerInput();

        //set player input callbacks
        playerInput.CharacterControls.Move.started += OnMovementPerformed;
        playerInput.CharacterControls.Move.canceled += OnMovementCancelled;
        playerInput.CharacterControls.Move.performed += OnMovementPerformed;
        playerInput.CharacterControls.Run.started += OnRun;
        playerInput.CharacterControls.Run.canceled += OnRun;
        playerInput.CharacterControls.Jump.started += OnJump;
        playerInput.CharacterControls.Jump.canceled += OnJump;
        playerInput.CharacterControls.Hit.started += OnHit;
        playerInput.CharacterControls.Hit.canceled += OnHit;
        playerInput.CharacterControls.Shoot.started += OnShoot;
        playerInput.CharacterControls.Shoot.canceled += OnShoot;

        Health = 100;
        Cooldown = 1f;
        canTakeDamage = 0f; 
    }

    protected override void EnterBeginningState()
    {
        currentState = new PlayerIdleState(this);
        currentState.EnterState();
    }

    protected override void UpdateState()
    {
        currentState.UpdateState();
        rb.linearVelocity = appliedMovement;
    }

    protected override void FaceMovement()
    {
        
        if (rb.linearVelocity.x != 0)
        {
            sprite.localScale = new Vector3(Mathf.Sign(rb.linearVelocity.x), 1, 1);
        }
    }

    void OnMovementPerformed(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        isMovementPressed = currentMovementInput.x != 0f;
    }

    void OnMovementCancelled(InputAction.CallbackContext context)
    {
        currentMovementInput = Vector2.zero;
        isMovementPressed = false;
    }

    void OnRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }

    void OnJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }
    void OnHit(InputAction.CallbackContext context)
    {
        isHitPressed = context.ReadValueAsButton();
    }
    void OnShoot(InputAction.CallbackContext context)
    {
        isShootPressed = context.ReadValueAsButton();
    }
    void OnHurt(InputAction.CallbackContext context)
    {
        isHurt = context.ReadValueAsButton();
    }

    void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }

    void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }

    public void ApplyDamage(int damage)
    {
        if (Time.time > canTakeDamage)
        {
            canTakeDamage = Time.time + Cooldown;
            Health -= damage;
            Debug.Log("Health: " + Health);
            IsHurt = true;
        }

        if (Health <= 0f)
        {
            Debug.Log("You Lost!");
            Time.timeScale = 0f;
        }
       
    }

    void OnAttackAnimationStart()
    {
        AttackFinished = false;
    }
    void OnAttackAnimationFinish()
    {
        AttackFinished = true;
    }

    void OnShootAnimationStart()
    {
        ShootFinished = false;
    }
    void TriggerBulletShooting()
    {
        ShootStarted = true;
    }
    void OnShootAnimationFinish()
    {
        ShootFinished = true;
        ShootStarted = false;
    }

    void OnHurtAnimationStart()
    {
        HurtFinished = false;
    }
    void OnHurtAnimationFinish()
    {
        HurtFinished = true;
    }

    void OnJumpAnimationStart()
    {
        grounded = false;
    }
    void OnJumpAnimationEnd()
    {
        grounded = true;
    }

}
