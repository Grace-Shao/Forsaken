using UnityEngine;
using System.Collections;

public class BossBeginUltimateState : State
{
    private BossStateMachine bossContext;
    private LineRenderer lineRenderer;
    private Transform chainStart;

    public BossBeginUltimateState(BossStateMachine currentContext) : base(currentContext)
    {
        bossContext = currentContext;
    }

    public override void EnterState()
    {
        bossContext.GrapplingFinished = 0;
        bossContext.Anim.SetTrigger("final");

        lineRenderer = bossContext.GetComponentInChildren<LineRenderer>(true);
        if (lineRenderer == null)
        {
            Debug.Log("LineRenderer component not found on boss GameObject");
            SwitchState(new BossTransitionState(bossContext));
            return;
        }
        lineRenderer.gameObject.SetActive(true);
        chainStart = lineRenderer.transform;

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
            yield return null;
        }

        // The pulling of the boss towards the grapple target
        while (Vector3.Distance(bossContext.transform.position, bossContext.CenterPos.position) > stopDistance)
        {
            lineRenderer.SetPosition(0, bossContext.GetComponent<Collider2D>().bounds.center);
            lineRenderer.SetPosition(1, bossContext.CenterPos.position);
            bossContext.transform.position = Vector3.MoveTowards(bossContext.transform.position, bossContext.CenterPos.position, bossContext.GrappleSpeed * Time.deltaTime);
            yield return null;
        }
        bossContext.GrapplingFinished = 1;
    }

}