using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerTankController : MonoBehaviour
{
    public GameObject tank;
    public Transform turret;
    public Transform firePoint;
    public GameObject[] projectilePrefabs; // Снаряды для различных типов
    public float turretRotateSpeed = 5f;
    public float moveSpeed = 5f;
    public float rotateSpeed = 100f;
    public int health = 100;
    public float fireRate = 1f; // Время перезарядки в секундах
    public float projectileSpeed = 100f;
    private int currentProjectileIndex = 0;
    private bool isDestroyed = false;
    private float nextFireTime = 0f;

    void Update()
    {
        if (!isDestroyed)
        {
            MoveTank();
            RotateTurret();
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

    void RotateTurret()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 targetPosition = hit.point;
            Vector3 direction = (targetPosition - turret.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            turret.rotation = Quaternion.Slerp(turret.rotation, lookRotation, Time.deltaTime * turretRotateSpeed);
        }
    }

    void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
        {
            FireProjectile();
            nextFireTime = Time.time + fireRate; // Устанавливаем время следующего выстрела
        }
    }

    void HandleProjectileSwitching()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentProjectileIndex = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentProjectileIndex = 1;
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
            Destroy(projectile, 5f); // Уничтожаем снаряд через 5 секунд, чтобы не засорять сцену
        }
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
        Rigidbody turretRb = turret.gameObject.AddComponent<Rigidbody>();
        turretRb.AddForce(Vector3.up * 5f, ForceMode.Impulse); // Отбрасываем башню вверх
        // Дополнительная логика уничтожения танка

        Invoke(nameof(RestartScene), 5f); // Перезапуск сцены через 5 секунд (уменьшено время)
        Invoke(nameof(RemoveTankModel), 5f); // Удаление модели через 5 секунд
    }

    void RemoveTankModel()
    {
        Destroy(tank); // Удаляем модель танка
    }

    void RestartScene()
    {
        Debug.Log("Перезапуск сцены..."); // Вывод сообщения в консоль для отладки
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Перезапускаем сцену
    }

    public bool IsDestroyed()
    {
        return isDestroyed;
    }
}