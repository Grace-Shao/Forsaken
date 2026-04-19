using System.Collections;
using UnityEngine;

public class Player_Melee : Weapon
{
    private PlayerStateMachine playerContext;

    [SerializeField]
    [Range(0f, 0.33f)]
    float _hitStopDuration = 0.1f;
    [SerializeField]
    [Range(0f, 0.33f)]
    float _delayDuration = 0.1f;

    bool _isHitStopped = false;

    protected override void Init()
    {
        weilder = GameObject.FindGameObjectWithTag("Player").transform;
        playerContext = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStateMachine>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (weilder.gameObject.GetComponent<PlayerStateMachine>().IsBlocking)
        {
            return;
        }
        string layer = LayerMask.LayerToName(other.gameObject.layer);
        if (layer == "Enemies" || other.gameObject.CompareTag("Breakable"))
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                playerContext.updateEnergy(playerContext.AttackGain);
                Attack(damageable);

                DoHitStop();
            }
        }
    }

    void DoHitStop()
    {
        if (!_isHitStopped)
        {
            StartCoroutine(DelayCoroutine());
        }
    }

    IEnumerator DelayCoroutine()
    {
        _isHitStopped = true;
        yield return new WaitForSecondsRealtime(_delayDuration);
        StartCoroutine(HitStopCoroutine());
    }
    IEnumerator HitStopCoroutine()
    {
        float timeScale = Time.timeScale;
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(_hitStopDuration);

        Time.timeScale = timeScale;
        _isHitStopped = false;
    }
}
