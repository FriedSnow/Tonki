using UnityEngine;

public class BMPCannon : MonoBehaviour
{
    public GameObject cannonProjectilePrefab;
    public Transform cannonLaunchPoint;
    public float cannonFireRate = 0.5f; // Скорострельность пушки
    public float cannonProjectileSpeed = 20f;

    private float nextCannonFireTime = 0f;

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextCannonFireTime)
        {
            nextCannonFireTime = Time.time + cannonFireRate;
            FireCannon();
        }
    }

    void FireCannon()
    {
        GameObject projectile = Instantiate(cannonProjectilePrefab, cannonLaunchPoint.position, cannonLaunchPoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = cannonLaunchPoint.forward * cannonProjectileSpeed;
        }
    }
}
