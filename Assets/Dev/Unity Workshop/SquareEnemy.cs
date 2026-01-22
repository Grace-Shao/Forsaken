using UnityEngine;

public class SquareEnemy : Enemy
{
    public Transform firingPoint;
    public GameObject bullet;
    public float bulletSpeed;

    public float fireRate;
    private float timeToFire = 0f;
    public override void Attack()
    {
        if (timeToFire <= 0f)
        {
           GameObject b = Instantiate(bullet, firingPoint.position, gameObject.transform.rotation);
           Vector2 dir = new Vector2(gameObject.transform.localScale.x, 0f);
           b.GetComponent<EnemyBullet>().Initialize(dir, bulletSpeed);
           timeToFire = fireRate; 
        } else
        {
            timeToFire -= Time.deltaTime;
        }
    }
}
