using UnityEngine;

public class TankShooting : MonoBehaviour
{
    public GameObject armorPiercingBulletPrefab;
    public GameObject highExplosiveBulletPrefab;
    public Transform firePoint;
    public Rigidbody tankRigidbody;
    public float bulletSpeed = 300f;
    public float APSpeed = 300f;
    public float HESpeed = 100f;
    public float fireRate = 4f; // Время перезарядки в секундах
    private float nextFireTime = 0f;
    public float recoilForce = 10f; // Сила отдачи
    public float APrecoilForce = 10f; // Сила отдачи
    public float HErecoilForce = 50f; // Сила отдачи

    private AmmoManager ammoManager;

    void Start()
    {
        tankRigidbody = GetComponentInParent<Rigidbody>();
        ammoManager = FindObjectOfType<AmmoManager>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        GameObject bulletPrefab;

        switch (ammoManager.currentAmmoType)
        {
            case AmmoManager.AmmoType.ArmorPiercing:
                bulletPrefab = armorPiercingBulletPrefab;
                bulletSpeed = APSpeed;
                recoilForce = APrecoilForce;
                break;
            case AmmoManager.AmmoType.HighExplosive:
                bulletPrefab = highExplosiveBulletPrefab;
                bulletSpeed = HESpeed;
                recoilForce = HErecoilForce;
                break;
            default:
                bulletPrefab = armorPiercingBulletPrefab;
                break;
        }

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = firePoint.forward * bulletSpeed;

        // Применить отдачу
        if (tankRigidbody != null)
        {
            tankRigidbody.AddForce(-firePoint.forward * recoilForce * 10f, ForceMode.Impulse);
        }
    }
}