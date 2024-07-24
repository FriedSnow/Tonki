using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerTankController : MonoBehaviour
{
    public GameObject tank;
    public Transform turret;
    public Transform firePoint;
    public Transform tankTransform;
    public GameObject[] projectilePrefabs; // Снаряды для различных типов
    public Text restartingText; // Ссылка на текстовый объект UI
    public Material grayMaterial;
    public GameObject burningParticlesPrefab;
    public GameObject explosionParticlesPrefab;
    public GameObject shootParticlesPrefab;
    public float turretRotateSpeed = 5f;
    public float moveSpeed = 5f;
    public float rotateSpeed = 100f;
    public float fireRate = 1f; // Время перезарядки в секундах
    public float projectileSpeed = 200f;
    public float APSpeed = 200f;
    public float HEATSpeed = 200f;
    public float HESpeed = 100f;
    public int health = 100;
    private float nextFireTime = 0f;
    private int currentProjectileIndex = 0;
    private bool isDestroyed = false;
    private bool canBeDestroyed = true;
    void Update()
    {
        if (!isDestroyed)
        {
            MoveTank();
            // RotateTurret();
            HandleShooting();
            HandleProjectileSwitching();
            Vector3 targetPosition = GetMouseWorldPosition();
            Vector3 direction = targetPosition - turret.position;
            direction = tankTransform.InverseTransformDirection(direction); // Переводим направление в локальные координаты танка
            direction.y = 0; // Игнорируем высоту для вращения

            Quaternion targetRotation = Quaternion.LookRotation(tankTransform.TransformDirection(direction), tankTransform.up);
            turret.rotation = Quaternion.Lerp(turret.rotation, targetRotation, 5 * Time.deltaTime);
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
        Plane plane = new Plane(Vector3.up, tankTransform.position.y); // Плоскость на уровне танка
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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 targetPosition = hit.point;
            Vector3 direction = targetPosition - transform.position; // transform.position - позиция корпуса танка
            direction = transform.InverseTransformDirection(direction); // Переводим направление в локальные координаты танка
            direction.y = 0; // Игнорируем высоту для вращения

            Quaternion targetRotation = Quaternion.LookRotation(transform.TransformDirection(direction), Vector3.up);
            turret.rotation = Quaternion.Slerp(turret.rotation, targetRotation, Time.deltaTime * turretRotateSpeed);
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
            Destroy(projectile, 5f); // Уничтожаем снаряд через 5 секунд, чтобы не засорять сцену
            if (shootParticlesPrefab != null)
            {
                GameObject shootParticles = Instantiate(shootParticlesPrefab, firePoint.position, firePoint.rotation);
                Destroy(shootParticles, 3f); // Уничтожение частиц через 3 секунды
            }
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
        if (canBeDestroyed)
        {

            Rigidbody turretRb = turret.gameObject.AddComponent<Rigidbody>();
            if (turretRb != null)
            {
                turretRb.AddForce(Vector3.up * 5f); // Отбрасываем башню вверх
            }
            ShowRestartingMessage();
            Invoke(nameof(RestartScene), 3f); // Перезапуск сцены через 5 секунд (уменьшено время)
            Invoke(nameof(RemoveTankModel), 3f); // Удаление модели через 5 секунд
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                if (renderer != null)
                {
                    renderer.material = grayMaterial;
                }
            }
            // Создание частиц горения
            if (burningParticlesPrefab != null)
            {
                Instantiate(burningParticlesPrefab, transform.position, Quaternion.identity);
            }
            if (explosionParticlesPrefab != null)
            {
                GameObject explosionParticles = Instantiate(explosionParticlesPrefab, transform.position, Quaternion.identity);
                Destroy(explosionParticles, 3f); // Уничтожение частиц через 3 секунды
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