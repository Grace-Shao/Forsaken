using UnityEngine;
using System.Collections;

public class BossGrappleState : State
{
    private BossStateMachine bossContext;
    private LineRenderer lineRenderer;
    private Transform chainStart;

    public BossGrappleState(BossStateMachine currentContext) : base(currentContext)
    {
        bossContext = currentContext;
    }

    public override void EnterState()
    {
        bossContext.Anim.Play("Grapple");

        lineRenderer = bossContext.GetComponentInChildren<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer component not found on boss GameObject");
            SwitchState(new BossStunState(bossContext));
            return;
        }
        lineRenderer.enabled = true;
        chainStart = lineRenderer.transform;

        bossContext.StartCoroutine(AnimateGrapple());
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
        if (bossContext.GrapplingFinished == 1)
        {
            SwitchState(new BossAttackState(bossContext));
        }
    }

    private IEnumerator AnimateGrapple()
    {
        float elapsed = 0f;
        float duration = bossContext.GrappleDuration;
        float stopDistance = 2f;
        Vector3 playerCenter;

        // The throwing of the chain
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float percent = elapsed / duration;

            // bossCenter = dogContext.GetComponent<Collider2D>().bounds.center;
            playerCenter = bossContext.Player.GetComponent<Collider2D>().bounds.center;

            Vector3 chainTip = Vector3.Lerp(chainStart.position, playerCenter, percent);

            lineRenderer.SetPosition(0, chainStart.position);
            lineRenderer.SetPosition(1, chainTip);

            yield return null;
        }

        // The pulling of the boss towards the player
        while (Vector3.Distance(bossContext.transform.position, bossContext.Player.transform.position) > stopDistance)
        {
            lineRenderer.SetPosition(0, bossContext.GetComponent<Collider2D>().bounds.center);
            lineRenderer.SetPosition(1, bossContext.Player.GetComponent<Collider2D>().bounds.center);
            bossContext.transform.position = Vector3.MoveTowards(bossContext.transform.position, bossContext.Player.transform.position, bossContext.GrappleSpeed * Time.deltaTime);
            yield return null;
        }

        lineRenderer.enabled = false;
        bossContext.GrapplingFinished = 1;
    }

}