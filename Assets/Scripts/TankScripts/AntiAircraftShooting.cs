using UnityEngine;

public class AntiAircraftShooting : MonoBehaviour
{
    public Transform[] firePoints; // Массив из двух стволов
    public GameObject projectilePrefab; // Префаб снаряда
    public Rigidbody tankRigidbody;
    public AudioClip shotSound; // Звук выстрела
    public float fireRate = 0.25f; // Время перезарядки между выстрелами
    public float projectileSpeed = 200f; // Скорость снаряда
    public float recoilForce = 10f; // Сила отдачи

    private float nextFireTime = 0f;
    private int currentFirePoint = 0;

    void Start()
    {
        tankRigidbody = GetComponentInParent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(projectilePrefab, firePoints[currentFirePoint].position, firePoints[currentFirePoint].rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        rb.velocity = firePoints[currentFirePoint].forward * projectileSpeed;

        // Переключаемся на следующий ствол
        currentFirePoint = (currentFirePoint + 1) % firePoints.Length;
        // Recoil
        if (tankRigidbody != null)
        {
            tankRigidbody.AddForce(-firePoints[currentFirePoint].forward * recoilForce, ForceMode.Impulse);
        }
    }
}