using UnityEngine;

public class Player_Melee : Weapon
{
    private PlayerStateMachine playerContext;
    protected override void Init()
    {
        weilder = GameObject.FindGameObjectWithTag("Player").transform;
        playerContext = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStateMachine>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        string layer = LayerMask.LayerToName(other.gameObject.layer);
        if (layer == "Enemies" || other.gameObject.CompareTag("Breakable"))
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                playerContext.updateEnergy(playerContext.AttackGain);
                Attack(damageable);
            }
        }
    }

}
