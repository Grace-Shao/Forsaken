using UnityEngine;
using System.Collections;

public class BossBeginUltimateState : State
{
    private BossStateMachine bossContext;
    private LineRenderer lineRenderer;
    private Transform chainStart;
    private Transform hitBox;

    public BossBeginUltimateState(BossStateMachine currentContext) : base(currentContext)
    {
        bossContext = currentContext;
    }

    public override void EnterState()
    {
        bossContext.Chain.SetActive(true);
        bossContext.GrapplingFinished = 0;
        bossContext.Anim.SetTrigger("final");

        lineRenderer = bossContext.Chain.GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.Log("LineRenderer component not found on boss GameObject");
            SwitchState(new BossTransitionState(bossContext));
            return;
        }
        lineRenderer.gameObject.SetActive(true);
        chainStart = lineRenderer.transform;
        hitBox = bossContext.Sprite.Find("ChainHitbox");

        bossContext.StartCoroutine(AnimateGrapple());
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        bossContext.Anim.ResetTrigger("final");
        lineRenderer.gameObject.SetActive(false);
        bossContext.Chain.SetActive(false);
    }

    public override void CheckSwitchStates()
    {
        if (bossContext.GrapplingFinished == 1)
        {
            SwitchState(new BossUltimateState(bossContext));
        }
    }

    private IEnumerator AnimateGrapple()
    {
        float elapsed = 0f;
        float duration = bossContext.GrappleDuration * (bossContext.IsParryStunned ? 0.5f : 1f);
        float stopDistance = 0f;

        // The throwing of the chain
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float percent = elapsed / duration;

            Vector3 chainTip = Vector3.Lerp(chainStart.position, bossContext.CenterPos.position, percent);

            lineRenderer.SetPosition(0, chainStart.position);
            lineRenderer.SetPosition(1, chainTip);

            Vector3 direction = chainTip - chainStart.position;
            float length = direction.magnitude;

            hitBox.position = chainStart.position + direction * 0.5f; 
            
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            hitBox.rotation = Quaternion.Euler(0f, 0f, angle); 

            hitBox.GetComponent<BoxCollider2D>().size = new Vector2(length / Mathf.Abs(hitBox.lossyScale.x), 0.1f / Mathf.Abs(hitBox.lossyScale.y));
            hitBox.GetComponent<BoxCollider2D>().enabled = true;
            yield return null;
        }

        // The pulling of the boss towards the grapple target
        while (Vector3.Distance(bossContext.transform.position, bossContext.CenterPos.position) > stopDistance)
        {
            lineRenderer.SetPosition(0, bossContext.GetComponent<Collider2D>().bounds.center);
            lineRenderer.SetPosition(1, bossContext.CenterPos.position);
            bossContext.transform.position = Vector3.MoveTowards(bossContext.transform.position, bossContext.CenterPos.position, bossContext.GrappleSpeed * Time.deltaTime);
            Vector3 direction = bossContext.Player.transform.position- bossContext.GetComponent<Collider2D>().bounds.center;
            float length = direction.magnitude;

            hitBox.position = bossContext.GetComponent<Collider2D>().bounds.center + direction * 0.5f; 
            
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            hitBox.rotation = Quaternion.Euler(0f, 0f, angle); 

            hitBox.GetComponent<BoxCollider2D>().size = new Vector2(length / Mathf.Abs(hitBox.lossyScale.x), 0.1f / Mathf.Abs(hitBox.lossyScale.y));
            hitBox.GetComponent<BoxCollider2D>().enabled = true;
            yield return null;
        }
        bossContext.GrapplingFinished = 1;
    }

}