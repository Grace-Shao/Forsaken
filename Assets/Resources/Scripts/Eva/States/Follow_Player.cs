using UnityEngine;

public class Follow_Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float stoppingDist = 2f;
    private Transform target;
    private bool isFlipped = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target == null) return;
        FacePlayer();
        float currentDist = Vector2.Distance(transform.position, target.position);
        if (currentDist > stoppingDist)
        {
            transform.position = Vector2.MoveTowards(
                transform.position, 
                target.position, 
                moveSpeed * Time.fixedDeltaTime
            );
        }
    }

    public void FacePlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.x *= -1f;
        if (transform.position.x < target.position.x && isFlipped)
        {
            transform.localScale = flipped;
            isFlipped = false;
        } else if (transform.position.x > target.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            isFlipped = true;
        }
    }
}
