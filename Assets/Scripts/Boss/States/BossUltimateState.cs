using UnityEngine;
public class BossUltimateState : State
{
    private BossStateMachine bossContext;
    private LineRenderer[] slashes;
    private float slashLifetime = 1f;
    private float slashTimer = 0f;
    public BossUltimateState(BossStateMachine currentContext) : base(currentContext)
    {
        bossContext = currentContext;
        isBaseState = true;
    }
    public override void EnterState()
    {
        Debug.Log("Boss Ultimate entered");

        // Params for the slashes
        int numSlashes = 8;
        float minLength = 3f;
        float maxLength = 40f;
        float angleSpread = 270f;
        Vector3 bossOrigin = bossContext.transform.position + Vector3.up * 2f; // Slightly above boss

        slashes = new LineRenderer[numSlashes];
        slashTimer = 0f;

        // todo: boss teleport to player ish?

        float originRadius = 2.5f; // How far from boss the slash can start (change later)

        for (int i = 0; i < numSlashes; i++)
        {
            GameObject slashObj = new GameObject($"UltimateSlash_{i}");
            var lr = slashObj.AddComponent<LineRenderer>();
            lr.positionCount = 2;
            lr.widthMultiplier = 0.2f;
            // Set material/color here if desired
            float angle = Random.Range(0f, angleSpread);
            float length = Random.Range(minLength, maxLength);
            Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.right;

            // Random offset for origin
            float offsetAngle = Random.Range(0f, 360f);
            float offsetDist = Random.Range(0f, originRadius);
            Vector3 offset = Quaternion.Euler(0, 0, offsetAngle) * Vector3.right * offsetDist;
            Vector3 origin = bossOrigin + offset;

            lr.SetPosition(0, origin);
            lr.SetPosition(1, origin + dir * length);
            slashes[i] = lr;
        }
    }
    public override void UpdateState()
    {
        // Persist slashes for slashLifetime seconds
        slashTimer += Time.deltaTime;
        if (slashTimer >= slashLifetime)
        {
            // Destroy all slashes and clean up
            if (slashes != null)
            {
                foreach (var lr in slashes)
                {
                    if (lr != null)
                        Object.Destroy(lr.gameObject);
                }
                slashes = null;
            }
        }
        CheckSwitchStates();
    }
    public override void ExitState()
    {
        Debug.Log("Boss Ultimate exited");
        // Cleanup logic if needed
    }
    public override void CheckSwitchStates()
    {
        // Transition back to idle if slashes are gone
        if (slashTimer >= slashLifetime && slashes == null)
        {
            SwitchState(new BossTransitionState(bossContext));
        }
    }
}
