using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Weapons : MonoBehaviour
{
    public float fireRate;
    public float damage;
    public LayerMask whatToHit;
    public Transform bulletTrailPrefab;
    public float effectSpawnRate;
    public Transform muzzleFlashPrefab;


    private float timeToFire = 0;
    private float timeToSpawnEffect = 0;
    private Transform firePoint;
    void Awake()
    {
        firePoint = transform.Find("FirePoint");
        Debug.Assert(firePoint != null);
    }

    void Update()
    {
        if (fireRate == 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1") && Time.time > timeToFire)
            {
                timeToFire = Time.time + (1 / fireRate);
                shoot();
            }
        }
    }

    private void shoot()
    {
        if (Time.time >= timeToSpawnEffect)
        {
            Vector2 mousePosition = new Vector2(
                Camera.main.ScreenToWorldPoint(Input.mousePosition).x, 
                Camera.main.ScreenToWorldPoint(Input.mousePosition).y
            );
            Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);

            RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mousePosition - firePointPosition, 100, whatToHit);
            showEffect();
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
            if (hit.collider != null)
            {
                Debug.DrawLine(firePointPosition, hit.point, Color.red, 0.5f);
                Debug.Log($"We Hit {hit.collider.name}");
            }
        }
        
        //Debug.DrawLine(firePointPosition, (mousePosition-firePointPosition) * 10, Color.yellow, 0.2f);
        
    }

    private void showEffect()
    {
        Instantiate(bulletTrailPrefab, firePoint.position, firePoint.rotation);
        

        // TODO : 나중에 여기 그냥 이미지 껏다켰다로 바꾸는 걸로 구현해보자. 이건 무슨 낭비냐.
        Transform muzzleInstance = Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation) as Transform;
        muzzleInstance.parent = firePoint;
        float size = Random.Range(0.6f, 0.9f);
        muzzleInstance.localScale = new Vector3(size, size, size);
        Destroy(muzzleInstance.gameObject, 0.1f);
    }
}
