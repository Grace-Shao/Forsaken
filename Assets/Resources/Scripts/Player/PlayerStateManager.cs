using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerStateManager : MonoBehaviour
{
    //control variables
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float runSpeed = 7f;

    //Game Objects
    private Animator animator;
    private Rigidbody2D player;
    private Transform sprite;

    //player input system
    PlayerInput playerInput;
    Vector2 currentMovementInput;
    Vector3 appliedMovement;
    bool isMovementPressed;
    bool isRunPressed;
    bool isJumpPressed;
    bool isHitPressed;
    bool isHurt; 
    bool slashFinished = false;
    bool hurtFinished = false;
    bool grounded = true;

    //player info
    private int health = 100;

    //States
    PlayerBaseState currentState;
    PlayerStateFactory states;

    //getters and settesr
    public PlayerBaseState CurrentState {get {return currentState; } set {currentState = value;}}
    public Animator PlayerAnimator {get {return animator;}}
    public Transform Sprite {get {return sprite;}}
    public bool IsMovementPressed {get {return isMovementPressed;} set {isMovementPressed = value;}}
    public bool IsRunPressed {get {return isRunPressed;} set {isRunPressed = value;}}
    public bool IsJumpPressed {get {return isJumpPressed;} set {isJumpPressed = value;}}
    public bool IsHitPressed {get {return isHitPressed;} set {isHitPressed = value;}}
    public bool IsHurt{get {return isHurt;} set {isHurt = value;}}
    public bool SlashFinished {get {return slashFinished; } set {slashFinished = value;}}
    public bool HurtFinished {get {return hurtFinished; } set {hurtFinished = value;}}
    public bool Grounded {get {return grounded;} set {grounded = value;}}
    public float AppliedMovementX {get {return appliedMovement.x;} set {appliedMovement.x = value;}}
    public float AppliedMovementY {get {return appliedMovement.y;} set {appliedMovement.y = value;}}
    public Vector2 CurrentMovement {get {return currentMovementInput;}}
    public float RunSpeed {get {return runSpeed;}}
    public float MoveSpeed {get {return moveSpeed;}}
    public int Health {get {return health;} set {health = value;}}

    void Awake()
    {
        //set reference variables
        playerInput = new PlayerInput();
        player = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        sprite = transform.Find("sprite");
        states = new PlayerStateFactory(this);
        currentState = states.Idle();
        currentState.EnterState();

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
    
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState();
        player.linearVelocity = appliedMovement;


        if (player.linearVelocity.x != 0)
        {
            sprite.localScale = new Vector3(Mathf.Sign(player.linearVelocity.x), 1, 1);
        }
        
        if (Health <= 0)
        {
            Debug.Log("you lost");
            Time.timeScale = 0f;
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
        if (currentState.GetType() == typeof(PlayerHurtState))
        {
            return;
        }
        health -= damage;
        Debug.Log("Health: " + Health);
        currentState.SwitchState(states.Hurt());
    }

    void OnSlashAnimationStart()
    {
        SlashFinished = false;
    }
    void OnSlashAnimationFinish()
    {
        SlashFinished = true;
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
