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
    private float slashTimer = 0f;

    // Slash configuration constants
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
        slashTimer = 0f;

        chain = bossContext.GetComponentInChildren<LineRenderer>(true);

        // Create slashes gameobject with chain linerenderer
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
    }

    public override void UpdateState()
    {
        slashTimer += Time.deltaTime;

        // Animate outward for the full slash lifetime
        if (slashes != null)
        {
            float t = Mathf.Clamp01(slashTimer / Mathf.Max(slashLifetime, 0.0001f));
            for (int i = 0; i < slashes.Length; i++)
            {
                if (slashes[i] != null && slashes[i].LineRenderer != null)
                    slashes[i].LineRenderer.SetPosition(1, Vector3.Lerp(slashes[i].Origin, slashes[i].End, t));
            }
        }
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Debug.Log("Boss Ultimate exited");
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
        if (slashTimer >= slashLifetime)
            SwitchState(new BossTransitionState(bossContext));
    }
}