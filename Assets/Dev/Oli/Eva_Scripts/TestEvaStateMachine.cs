using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class TestEvaStateMachine : StateMachine,IDamageable
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float followDistance;
    [SerializeField] private float timeInIdle;
    [SerializeField] private List<Transform> hidingSpots = new List<Transform>();
    // Use the child sprite reference found in Init()
    [Header("VFX")]
    [SerializeField] private ParticleSystem scaredParticles;
    //private ParticleSystem damageTakenParticles;
    [Header("UI Setup")]
    [SerializeField] private Image healthBar;
    [SerializeField] private int maxHealth = 50;

    private float damageCooldown = 0.6f; // tweak.
    private bool isFlipped = false;
    private bool isHurt = false; 
    private bool isTransitioning = false;
    private bool hurtFinished = false;
    private int health;
    private float nextDamageTime = 0f; // Tracks the internal timestamp clock

    
    
    public bool IsHurt{get {return isHurt;} set {isHurt = value;}}
    public bool IsTransitioning {get {return isTransitioning;} set {isTransitioning = value;}}
    public bool HurtFinished {get {return hurtFinished; } set {hurtFinished = value;}}
    public int Health {get {return health;} set {health = value;}}
    public float Cooldown {get {return damageCooldown;} set {damageCooldown = value;}}
    
    public ParticleSystem ScaredParticles {get {return scaredParticles;}}
    // This allows states to read the current state, but not change it directly
    // public State CurrentState {get {return currentState;}}
    public float FollowDistance {get {return followDistance;}}
    // public float MoveSpeed {get {return moveSpeed;}}
    public Transform TargetHideSpot { get; set; }

    public List<Transform> GetHidingSpots() {return hidingSpots;}

    public Action<TestEvaStateMachine> EvaDeath;

    protected override void Init()
    {
        base.Init();
        sprite = transform.Find("Sprite");
        SetHealth(maxHealth);
        scaredParticles = GetComponentInChildren<ParticleSystem>();
        //damageTakenParticles = sprite.Find("hit received particles eva").GetComponent<ParticleSystem>();
        gameManager.CombatStarted += Hide;
        gameManager.CombatEnded += StopHiding;
        nextDamageTime = 0f;
        if (healthBar != null)
        {
            healthBar.fillAmount = Health / (float)maxHealth;
        }
    }

    protected override void EnterBeginningState()
    {
        IsTransitioning = false;
        currentState = new TestEvaIdleState(this);
        currentState.EnterStates();
    }

    protected override void UpdateState()
    {
        //HandleHideInput();
        
        if (!IsTransitioning)
        {
            rb.linearVelocity = appliedMovement;
        }
        currentState.UpdateStates();
    }

    protected override void FaceMovement()
    {
        Transform currentTarget = player.transform;
        //Determine if Eva should be facing player or hiding spot
        if (currentState is TestEvaMoveToHideState && TargetHideSpot != null)
        {
            currentTarget = TargetHideSpot;
        }

        Vector3 flipped = sprite.localScale;
        flipped.x *= -1f;
        if (sprite.position.x < currentTarget.position.x && isFlipped)
        {
            sprite.localScale = flipped;
            isFlipped = false;
        } else if (sprite.position.x > currentTarget.position.x && !isFlipped)
        {
            sprite.localScale = flipped;
            isFlipped = true;
        }
    }

    public bool FollowRange()
    {
        return Vector3.Distance(transform.position,Player.transform.position) >= FollowDistance;
    }

    public void SetHealth(int value) {
        health = value;
        // UpdateHealthUI();
    }

    public void Hide()
    {

        Debug.Log($"play scaredparticles");
        
        if (scaredParticles != null) scaredParticles.Play();
        StartHiding();
        
    }

    public void StopHiding()
    {
        if (currentState is TestEvaHiddenState || currentState is TestEvaMoveToHideState)
        {
            //Debug.Log("J, Off, switching to idle.");
            currentState.SwitchState(new TestEvaIdleState(this));
            if (scaredParticles != null) scaredParticles.Stop();
        }
    }
    //Handle Hide Input and trigger hiding behavior
    // public void HandleHideInput()
    // {
    //     if (Input.GetKey(KeyCode.J))
    //     {
    //         //quit hidden or moving to hidespot if pressed j
    //         //Debug.Log("J key was pressed! Current frame: " + Time.frameCount);
    //         if (currentState is EvaHiddenState || currentState is EvaMoveToHideState)
    //         {
    //             //Debug.Log("J, Off, switching to idle.");
    //             currentState.SwitchState(new EvaIdleState(this));
    //             if (scaredParticles != null) scaredParticles.Stop();
    //         }
    //         else //idle/follow then start hiding if pressed j
    //         {
    //             //Debug.Log("J, On, starting to hide.");
    //             StartHiding();
    //             if (scaredParticles != null) scaredParticles.Play();
    //         }
    //     }
    // }
    

    //Function to find closest hiding spot and switch to move to hide state
    public void StartHiding()
    {

        if (hidingSpots.Count == 0) return;

        float closestDistance = Mathf.Infinity;
        Transform closestSpot = null;

        foreach (Transform spot in hidingSpots)
        {
            float distance = Vector3.Distance(transform.position, spot.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestSpot = spot;
            }
        }

        if (closestSpot != null)
        {
            TargetHideSpot = closestSpot;
            currentState.SwitchState(new TestEvaMoveToHideState(this));
        }
            
    }
 
 
    //Eva taking damage
    public void ApplyDamage(int damage) {
        if (Time.time < nextDamageTime) return;

        SetHealth(health - damage);
        if (healthBar != null)
        {
            healthBar.fillAmount = Health / (float)maxHealth;
        }
        IsHurt = true;
        currentState.SwitchState(new TestEvaHurtState(this));

        nextDamageTime = Time.time + damageCooldown;

        //damageTakenParticles.Play();
    
        
        //Debug.Log($"<color=cyan>[EVA HEALTH]</color> Eva took {damage} damage. Current Health is {Health}/{maxHealth}");
        // damageTakenParticles.Play();

        if (Health <= 0f)
        {
            EvaDeath?.Invoke(this);
            gameObject.SetActive(false);
        }
    }
}
