using System.Collections;
using UnityEngine;
public class BossUltimateState : State
{
    private class SlashInfo
    {
        public LineRenderer LineRenderer;
        public Vector3 Origin;
        public Vector3 End;
        public SlashInfo(LineRenderer lr, Vector3 origin, Vector3 end)
        {
            LineRenderer = lr;
            Origin = origin;
            End = end;
        }
    }

    private BossStateMachine bossContext;
    private SlashInfo[] slashes;
    private LineRenderer chain;
    private float slashLifetime = 1.5f;
    private Coroutine ultimateCoroutine;

    // Slash config constants
    private int numSlashes = 8;
    private float minLength = 50f;
    private float maxLength = 80f;
    private float angleSpread = 270f;
    private float originRadius = 2.5f;

    public BossUltimateState(BossStateMachine currentContext) : base(currentContext)
    {
        bossContext = currentContext;
        isBaseState = true;
    }

    public override void EnterState()
    {
        Debug.Log("Boss Ultimate entered");
        
        bossContext.AppliedMovementX = 0;
        bossContext.AppliedMovementY = 0;

        Vector3 bossOrigin = bossContext.transform.position + Vector3.up * 2f;
        slashes = new SlashInfo[numSlashes];

        chain = bossContext.GetComponentInChildren<LineRenderer>(true);

        // Create slashes gameobject with HUE's chain linerenderer
        for (int i = 0; i < numSlashes; i++)
        {
            LineRenderer lr = null;
            if (chain != null)
            {
                lr = Object.Instantiate(chain);
                lr.transform.SetParent(null);
                lr.gameObject.name = $"UltimateSlash_{i}";
                lr.gameObject.SetActive(true);
            }
            else
            {
                GameObject slashObj = new GameObject($"UltimateSlash_{i}");
                lr = slashObj.AddComponent<LineRenderer>();
                lr.widthMultiplier = 0.2f;
            }
            lr.positionCount = 2;

            float angle = Random.Range(0f, angleSpread);
            float length = Random.Range(minLength, maxLength);
            Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.right;

            float offsetAngle = Random.Range(0f, 360f);
            float offsetDist = Random.Range(0f, originRadius);
            Vector3 offset = Quaternion.Euler(0, 0, offsetAngle) * Vector3.right * offsetDist;
            Vector3 origin = bossOrigin + offset;
            Vector3 end = origin + dir * length;

            // Start both points at origin, animate outward in UpdateState
            lr.SetPosition(0, origin);
            lr.SetPosition(1, origin);
            slashes[i] = new SlashInfo(lr, origin, end);
        }

        // animate the chains expanding
        ultimateCoroutine = bossContext.StartCoroutine(RunUltimate());
    }

    public override void UpdateState()
    {
    }

    private IEnumerator RunUltimate()
    {
        float elapsed = 0f;
        while (elapsed < slashLifetime)
        {
            elapsed += Time.deltaTime;
            // t = normalized progress from 0 to 1, divide by 0.0001 to avoid divide by zero
            float t = Mathf.Clamp01(elapsed / Mathf.Max(slashLifetime, 0.0001f));

            // diving by 0.2 means last 20% of the animation time is chains fading out
            // adjust 0.2 if needed
            float alphaMultiplier = Mathf.Clamp01((1f - t) / 0.2f);

            // for each slash, lerp the end point from origin to end, and fade out over time
            for (int i = 0; i < slashes.Length; i++)
            {
                if (slashes[i] != null && slashes[i].LineRenderer != null)
                {
                    slashes[i].LineRenderer.SetPosition(1, Vector3.Lerp(slashes[i].Origin, slashes[i].End, t));

                    // grab the line render colors and apply the fade
                    Color startColor = slashes[i].LineRenderer.startColor;
                    Color endColor = slashes[i].LineRenderer.endColor;
                    startColor.a = alphaMultiplier;
                    endColor.a = alphaMultiplier;
                    slashes[i].LineRenderer.startColor = startColor;
                    slashes[i].LineRenderer.endColor = endColor;
                }
            }
            yield return null;
        }
        

        ultimateCoroutine = null;

        // automatically exit
        SwitchState(new BossTransitionState(bossContext));
    }

    public override void ExitState()
    {
        Debug.Log("Boss Ultimate exited");
        if (ultimateCoroutine != null)
        {
            bossContext.StopCoroutine(ultimateCoroutine);
            ultimateCoroutine = null;
        }

        // Clean up slashes
        if (slashes != null)
        {
            foreach (var slash in slashes)
                if (slash != null && slash.LineRenderer != null) Object.Destroy(slash.LineRenderer.gameObject);
            slashes = null;
        }
    }

    public override void CheckSwitchStates()
    {
    }
}