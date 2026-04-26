using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected int attackDamage;
    protected Transform weilder;
    protected Transform victim;

    public void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        
    }

    public virtual void Attack(IDamageable damageable)
    {
        damageable.ApplyDamage(attackDamage);
    }

    public void SetDamage(int damage)
    {
        attackDamage = damage;
    }

}
