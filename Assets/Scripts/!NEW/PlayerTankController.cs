using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerTankController : MonoBehaviour
{
    public GameObject tank;
    public Transform turret;
    public Transform firePoint;
    public Transform tankTransform;
    public Transform machineGun;          // Добавлен новый объект для зенитного пулемета
    public Transform machineGunFirePoint; // Точка выстрела для зенитного пулемета
    public GameObject[] projectilePrefabs; 
    public GameObject mgProjectilePrefab; // Снаряд для зенитного пулемета
    public Text restartingText; 
    public Material grayMaterial;
    public GameObject burningParticlesPrefab;
    public GameObject explosionParticlesPrefab;
    public GameObject shootParticlesPrefab;
    public Rigidbody tankRigidbody;
    public float turretRotateSpeed = 5f;
    public float mgRotateSpeed = 10f;    // Скорость вращения зенитного пулемета
    public float moveSpeed = 5f;
    public float rotateSpeed = 100f;
    public float fireRate = 1f; 
    public float projectileSpeed = 200f;
    public float APSpeed = 200f;
    public float HEATSpeed = 200f;
    public float HESpeed = 100f;
    public float mgFireRate = 0.1f;      // Скорострельность зенитного пулемета
    public float mgProjectileSpeed = 300f; // Скорость снаряда зенитного пулемета
    public int health = 100;
    public float recoilForce = 100f;
    private float nextFireTime = 0f;
    private float nextMGFireTime = 0f;   // Таймер для зенитного пулемета
    private int currentProjectileIndex = 0;
    private bool isDestroyed = false;
    private bool canBeDestroyed = true;

    void Update()
    {
        if (!isDestroyed)
        {
            MoveTank();
            RotateTurret();
            RotateMachineGun();
            HandleShooting();
            HandleProjectileSwitching();
        }
    }

    void MoveTank()
    {
        float move = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        float rotate = Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime;

        transform.Translate(0, 0, move);
        transform.Rotate(0, rotate, 0);
    }

    Vector3 GetMouseWorldPosition()
    {
        Plane plane = new Plane(Vector3.up, tankTransform.position.y);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;
        if (plane.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance);
        }
        return Vector3.zero;
    }

    void RotateTurret()
    {
        Vector3 targetPosition = GetMouseWorldPosition();
        Vector3 direction = targetPosition - turret.position;
        direction = tankTransform.InverseTransformDirection(direction);
        direction.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(tankTransform.TransformDirection(direction), tankTransform.up);
        turret.rotation = Quaternion.Lerp(turret.rotation, targetRotation, Time.deltaTime * turretRotateSpeed);
    }

    void RotateMachineGun()
    {
        Vector3 targetPosition = GetMouseWorldPosition();
        Vector3 direction = targetPosition - machineGun.position;
        direction = tankTransform.InverseTransformDirection(direction);
        direction.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(tankTransform.TransformDirection(direction), tankTransform.up);
        machineGun.rotation = Quaternion.Lerp(machineGun.rotation, targetRotation, Time.deltaTime * mgRotateSpeed);
    }

    void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
        {
            FireProjectile();
            nextFireTime = Time.time + fireRate;
        }

        if (Input.GetKey(KeyCode.Space) && Time.time >= nextMGFireTime)
        {
            FireMGProjectile();
            nextMGFireTime = Time.time + mgFireRate;
        }
    }

    void HandleProjectileSwitching()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentProjectileIndex = 0;
            projectileSpeed = APSpeed;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentProjectileIndex = 1;
            projectileSpeed = HEATSpeed;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentProjectileIndex = 2;
            projectileSpeed = HESpeed;
        }
    }

    void FireProjectile()
    {
        if (currentProjectileIndex >= 0 && currentProjectileIndex < projectilePrefabs.Length)
        {
            GameObject projectile = Instantiate(projectilePrefabs[currentProjectileIndex], firePoint.position, firePoint.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = firePoint.forward * projectileSpeed;
            }
            Destroy(projectile, 5f);
            if (shootParticlesPrefab != null)
            {
                GameObject shootParticles = Instantiate(shootParticlesPrefab, firePoint.position, firePoint.rotation);
                Destroy(shootParticles, 3f);
            }
            if (tankRigidbody != null)
            {
                tankRigidbody.AddForce(-firePoint.forward * recoilForce * 10f, ForceMode.Impulse);
            }
        }
    }

    void FireMGProjectile()
    {
        GameObject mgProjectile = Instantiate(mgProjectilePrefab, machineGunFirePoint.position, machineGunFirePoint.rotation);
        Rigidbody rb = mgProjectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = machineGunFirePoint.forward * mgProjectileSpeed;
        }
        Destroy(mgProjectile, 3f);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDestroyed = true;
        if (canBeDestroyed)
        {
            Rigidbody turretRb = turret.gameObject.AddComponent<Rigidbody>();
            if (turretRb != null)
            {
                turretRb.AddForce(Vector3.up * 5f);
            }
            ShowRestartingMessage();
            Invoke(nameof(RestartScene), 3f);
            Invoke(nameof(RemoveTankModel), 3f);
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                if (renderer != null)
                {
                    renderer.material = grayMaterial;
                }
            }
            if (burningParticlesPrefab != null)
            {
                Instantiate(burningParticlesPrefab, transform.position, Quaternion.identity);
            }
            if (explosionParticlesPrefab != null)
            {
                GameObject explosionParticles = Instantiate(explosionParticlesPrefab, transform.position, Quaternion.identity);
                Destroy(explosionParticles, 3f);
            }

            canBeDestroyed = false;
        }
    }

    void ShowRestartingMessage()
    {
        if (restartingText != null)
        {
            restartingText.text = "Restart...";
        }
    }

    void RemoveTankModel()
    {
        Destroy(tank);
    }

    void RestartScene()
    {
        Debug.Log("Перезапуск сцены...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public bool IsDestroyed()
    {
        return isDestroyed;
    }
}