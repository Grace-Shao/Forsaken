using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] Transform player;
    
    private bool isFlipped = false;
    Animator animator;
    private int health;

    void Start()
    {
        health = 100;
        animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if (health <= 0)
        {
            Debug.Log("you win");
            Time.timeScale = 0f;
        }
    }

    public void FacePlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > player.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        } else if (transform.position.x < player.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.transform == player)
        {
            player.gameObject.GetComponent<PlayerStateManager>().ApplyDamage(10);
        }
    }

     public void ApplyDamage(int damage)
    {
        health -= damage;
        Debug.Log("Boss Health: " + health);
        animator.SetTrigger("Hurt");
    }



}
