using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private bool isPlayer = false;
    [SerializeField] private int attackDamage = 20;
    [SerializeField] GameObject boss;
    [SerializeField] GameObject player;
    [SerializeField] Vector3 attackOffset;

    public int AttackDamage {get {return attackDamage;}}
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!isPlayer && other.gameObject.tag.Equals("Player"))
        {
            other.gameObject.GetComponent<PlayerStateManager>().ApplyDamage(attackDamage);
            RecoilBoss();
            
        } else if (isPlayer && other.gameObject.tag.Equals("Enemy"))
        {
            other.gameObject.GetComponent<Boss>().ApplyDamage(attackDamage);
            RecoilPlayer();
        }
    }

    private void RecoilBoss()
    {
        Vector3 pos = boss.transform.position;
        pos += -1 * Mathf.Sign(boss.transform.localScale.x) * attackOffset;
        boss.transform.position = pos;
    }

    private void RecoilPlayer()
    {
        Vector3 pos = player.transform.position;
        pos += -1 * Mathf.Sign(player.transform.localScale.x) * attackOffset;
        player.transform.position = pos;
    }
}
