using UnityEngine;

public class BreakablePillar : MonoBehaviour, IDamageable
{
    [SerializeField] private int health;
    [SerializeField] private GameObject broken;
    private float totalHealth;
    private float cooldown = 0;

    public int Health {get {return health; } set {health = value;}}
    public float Cooldown {get {return cooldown; } set {cooldown = value;}}

    void Start()
    {
       totalHealth = health; 
    }
    public void ApplyDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            broken.SetActive(true);
        }
    }
}
