using UnityEngine;
using System.Collections;

public class BossGrappleState : State
{
    private BossStateMachine bossContext;
    private LineRenderer lineRenderer;
    private Transform chainStart;
    private Transform hitBox;
    private Rigidbody2D bossBody;

    public BossGrappleState(BossStateMachine currentContext) : base(currentContext)
    {
        bossContext = currentContext;
    }

    public override void EnterState()
    {
        bossContext.TimeInState = 0f;
        bossBody = bossContext.RB;
        bossContext.Chain.SetActive(true);
        bossContext.GrapplingFinished = 0;
        bossContext.Anim.SetTrigger("grapple");

        lineRenderer = bossContext.Chain.GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.Log("LineRenderer component not found on boss GameObject");
            SwitchState(new BossTransitionState(bossContext));
            return;
        }
        //lineRenderer.gameObject.SetActive(true);
        chainStart = lineRenderer.transform;
        
        hitBox = bossContext.Sprite.Find("ChainHitbox");
        bossContext.StartCoroutine(AnimateGrapple());
    }

    public override void UpdateState()
    {
        bossContext.TimeInState += Time.deltaTime;
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        bossContext.GrapplingFinished = 0;
        bossContext.Anim.ResetTrigger("grapple");
        lineRenderer.gameObject.SetActive(false);
        bossContext.Chain.SetActive(false);
        lineRenderer.enabled = false;
    }

    public override void CheckSwitchStates()
    {
        if (bossContext.StateTimedOut || bossContext.GrapplingFinished == 1)
        {
            SwitchState(new BossIdleState(bossContext));
        }
    }

    private IEnumerator AnimateGrapple()
    {
        float elapsed = 0f;
        float duration = bossContext.GrappleDuration * (bossContext.IsParryStunned ? 0.5f : 1f);
        float speed = bossContext.GrappleSpeed;
        if (bossContext.IsParryStunned)
        {
            duration *= 2f;
            speed /= 2f;
        }
        float stopDistance = 2f;

        // Jump up before throwing the chain
        float jumpHeight = 5f;
        Vector2 jumpTarget = bossBody.position + Vector2.up * jumpHeight;
        while (Vector2.Distance(bossBody.position, jumpTarget) > 0.1f)
        {
            bossBody.MovePosition(Vector2.MoveTowards(bossBody.position, jumpTarget, speed * Time.fixedDeltaTime));
            yield return new WaitForFixedUpdate();
        }

        lineRenderer.enabled = true;

        // The throwing of the chain
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float percent = elapsed / duration;

            Vector3 chainTip = Vector3.Lerp(chainStart.position, bossContext.Player.transform.position, percent);

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
        while (Vector2.Distance(bossBody.position, bossContext.Player.transform.position) > stopDistance)
        {
            lineRenderer.SetPosition(0, bossContext.GetComponent<Collider2D>().bounds.center);
            lineRenderer.SetPosition(1, bossContext.Player.transform.position);
            bossBody.MovePosition(Vector2.MoveTowards(bossBody.position, bossContext.Player.transform.position, speed * Time.fixedDeltaTime));
            
            Vector3 direction = bossContext.Player.transform.position- bossContext.GetComponent<Collider2D>().bounds.center;
            float length = direction.magnitude;

            hitBox.position = bossContext.GetComponent<Collider2D>().bounds.center + direction * 0.5f; 
            
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            hitBox.rotation = Quaternion.Euler(0f, 0f, angle); 

            hitBox.GetComponent<BoxCollider2D>().size = new Vector2(length / Mathf.Abs(hitBox.lossyScale.x), 0.1f / Mathf.Abs(hitBox.lossyScale.y));
            hitBox.GetComponent<BoxCollider2D>().enabled = true;
            yield return new WaitForFixedUpdate();
        }
        bossContext.GrapplingFinished = 1;
    }

}