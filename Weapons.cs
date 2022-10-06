using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Weapons : MonoBehaviour
{
    public float fireRate;
    public int damage;
    public LayerMask whatToHit;
    public Transform bulletTrailPrefab;
    public Transform muzzleFlashPrefab;
    public Transform hitPrefab;

    // Camera Shake Section
    CameraShaker camShaker;
    public float camShakeAmount;
    private float camShakeLength = 0.1f;

    private float timeToFire = 0;
    private float timeToSpawnEffect = 0;
    private Transform firePoint;
    private const int raycastDistance = 100;
    [SerializeField]
    private float shootDelayPerSecond;

    [SerializeField]
    private string weaponShootSound;

    void Awake()
    {
        firePoint = transform.Find("FirePoint");
        Debug.Assert(firePoint != null);
    }

    private void Start()
    {
        camShaker = GameMaster.gm.GetComponent<CameraShaker>();
        Debug.Assert(camShaker != null);
        Debug.Assert(weaponShootSound != null);
        Debug.Assert(shootDelayPerSecond > float.Epsilon);
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

            RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mousePosition - firePointPosition, raycastDistance, whatToHit);
 
            if (hit.collider != null)
            {
                // Debug.DrawLine(firePointPosition, hit.point, Color.red, 0.5f);
                
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(damage);
                    // Debug.Log($"We Hit {hit.collider.name} It Damaged {damage}");
                }
            }
            Vector3 hitPosition;
            Vector3 hitNormal;
            bool isHit = false;
            if (hit.collider == null)
            {
                hitPosition = (mousePosition - firePointPosition) * 30;
                hitNormal = Vector3.zero;
            }
            else
            {
                hitPosition = hit.point;
                hitNormal = hit.normal;
                isHit = true;
            }

            showEffect(hitPosition, hitNormal, isHit);
            timeToSpawnEffect = Time.time + shootDelayPerSecond;
        }
        
        // Debug.DrawLine(firePointPosition, (mousePosition-firePointPosition) * 10, Color.yellow, 0.2f);
        
    }

    private void showEffect(Vector3 hitPosition, Vector3 hitNormal, bool isHit)
    {
        // Bullet Trail Section
        Transform bulletTrail = Instantiate(bulletTrailPrefab, firePoint.position, firePoint.rotation) as Transform;
        LineRenderer lineRenderer = bulletTrail.GetComponent<LineRenderer>();
        Debug.Assert(lineRenderer != null);
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, hitPosition);

        Destroy(bulletTrail.gameObject, 0.4f);

        if (isHit)
        {
            Transform hitEffect = Instantiate(hitPrefab, hitPosition, Quaternion.FromToRotation(Vector3.right, hitNormal)) as Transform;
            Destroy(hitEffect.gameObject, 0.4f);
            camShaker.Shake(camShakeAmount, camShakeLength);
        }


        // TODO : 나중에 여기 그냥 이미지 껏다켰다로 바꾸는 걸로 구현해보자. 이건 무슨 낭비냐.
        // MuzzleFlash Section
        Transform muzzleInstance = Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation) as Transform;
        muzzleInstance.parent = firePoint;
        float size = Random.Range(0.6f, 0.9f);
        muzzleInstance.localScale = new Vector3(size, size, size);
        Destroy(muzzleInstance.gameObject, 0.1f);

        // Play Shoot Sound
        AudioManager.instance.PlaySound(weaponShootSound);

    }
}
