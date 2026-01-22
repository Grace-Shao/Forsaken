using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamageable
{   
    [SerializeField] protected int health;
    [SerializeField] protected float speed;
    [SerializeField] protected float targetDistance;
    [SerializeField] protected float cooldown;
    protected float canTakeDamage = 0f;
    protected Rigidbody2D rb;
    protected bool isFlipped;
    protected Transform player;
    protected Vector2 target;

    public float Speed {get {return speed;} set {speed = value;}}
    public float TargetDistance {get {return targetDistance;} set {targetDistance = value;}}
    public int Health {get {return health;} set {health = value;}}
    public float Cooldown {get {return cooldown;} set {cooldown = value;}}
    public void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Update()
    {
        target = new Vector2(player.position.x, rb.gameObject.transform.position.y);
        Vector2 current = rb.gameObject.transform.position;
        if (Vector2.Distance(current, target) > TargetDistance)
        {
            Move();
        } else
        {
            Attack();
        }
        
    }

    //by default, this moves to some position close to the player. Override for more complex behavior
    public virtual void Move()
    {
        Debug.Log("moving");
        FacePlayer();
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, Speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
    }

    //override this method for each implemented Enemy to include more complex attack logic
    public virtual void Attack()
    {
        Debug.Log("attacking");
    }

    //DO NOT CHANGE
    //this flips the sprites to face the player
    public void FacePlayer()
    {
        Vector3 flipped = rb.gameObject.transform.localScale;
        flipped.x *= -1f;
        if (rb.gameObject.transform.position.x < player.position.x && isFlipped)
        {
            rb.gameObject.transform.localScale = flipped;
            isFlipped = false;
        } else if (rb.gameObject.transform.position.x > player.position.x && !isFlipped)
        {
            rb.gameObject.transform.localScale = flipped;
            isFlipped = true;
        }
    }

    //DO NOT CHANGE
    //update the current health
    public virtual void ApplyDamage(int damage)
    {
        if (Time.time > canTakeDamage)
        {
            canTakeDamage = Time.time + cooldown;
            Health -= damage;
            Debug.Log("Enemy Health: " + Health);

        }
        
        if (Health <= 0)
        {
            Debug.Log("Enemy killed!");
            rb.gameObject.SetActive(false);
        }
    }
}
