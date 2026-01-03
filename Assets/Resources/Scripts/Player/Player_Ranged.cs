using UnityEngine;

public class Player_Ranged : Weapon
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 10f;

    protected override void Init()
    {
        weilder = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Shoot()
    {
        if (projectilePrefab == null || firePoint == null) return;

        // Calculate direction based on the sprite's localScale
        Transform sprite = weilder.Find("sprite");

        float facing = Mathf.Sign(sprite.localScale.x); 
        Vector2 shootDirection = new Vector2(facing, 0);

        GameObject bulletObj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        
        if (bulletObj.TryGetComponent(out Bullet bullet))
        {
            bullet.Initialize(shootDirection, bulletSpeed);
        }
    }
    }