using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "DamageableData", menuName = "Scriptable Objects/DamageableData")]
public class DamageableData : ScriptableObject, IDamageable
{
    [SerializeField, CreateProperty]
    private int health = 100;
    [SerializeField, CreateProperty]
    private float cooldown = 1f;

    public int Health
    {
        get { return health; }
        set { health = value; }
    }

    public float Cooldown
    {
        get { return cooldown; }
        set { cooldown = value; }
    }

    public void ApplyDamage(int damage)
    {
        Health -= damage;
        Debug.Log($"Damage applied: {damage}. Current health: {Health}");
    }
}
