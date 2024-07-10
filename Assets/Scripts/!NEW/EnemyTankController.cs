using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class EnemyTankController : MonoBehaviour
{
    public GameObject tank;
    public Transform turret;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public Transform player;
    public Material grayMaterial;
    public GameObject burningParticlesPrefab;
    public float moveSpeed = 5f;
    public float rotateSpeed = 2f;
    public float turretRotateSpeed = 5f;
    public float fireInterval = 3f;
    public float projectileSpeed = 20f;
    public float desiredDistance = 20f; // Желаемое расстояние от игрока
    public int health = 100;
    private Vector3 targetPosition;
    private PlayerTankController playerTankController;
    public LayerMask obstacleMask; // Маска для препятствий
    private float firingAngleThreshold = 145f; // Допустимый угол отклонения прицела
    private bool isDestroyed = false;
    private bool canBeDestroyed = true;
    

    void Start()
    {
        playerTankController = player.GetComponent<PlayerTankController>();
        EnemyManager.instance.RegisterEnemy();
        StartCoroutine(FireRoutine());
    }

    void Update()
    {
        if (playerTankController != null && !playerTankController.IsDestroyed() && !isDestroyed)
        {
            if (IsPlayerInSight())
            {
                MoveTank();
                RotateTurretTowardsPlayer();
            }
        }
    }

    void MoveTank()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > desiredDistance)
        {
            // Двигаться к игроку
            targetPosition = player.position;
        }
        else if (distanceToPlayer < desiredDistance - 5f)
        {
            // Отступать от игрока
            targetPosition = transform.position - (player.position - transform.position).normalized * desiredDistance;
        }
        else
        {
            // Оставаться на месте
            targetPosition = transform.position;
        }

        Vector3 direction = (targetPosition - transform.position).normalized;

        // Проверка на нулевой вектор
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }

        if (distanceToPlayer > desiredDistance || distanceToPlayer < desiredDistance - 5f)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }

    void RotateTurretTowardsPlayer()
    {
        Vector3 direction = (player.position - turret.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            turret.rotation = Quaternion.Slerp(turret.rotation, lookRotation, Time.deltaTime * turretRotateSpeed);
        }
    }

    IEnumerator FireRoutine()
    {
        // Добавляем случайную задержку перед началом стрельбы
        yield return new WaitForSeconds(Random.Range(0, fireInterval));

        while (true)
        {
            yield return new WaitForSeconds(fireInterval);
            if (!playerTankController.IsDestroyed() && IsPlayerInSight())
            {
                FireProjectile();
            }
        }
    }

    bool IsPlayerInSight()
    {
        Vector3 directionToPlayer = (player.position - firePoint.position).normalized;
        float angle = Vector3.Angle(firePoint.forward, directionToPlayer);

        if (angle < firingAngleThreshold)
        {
            RaycastHit hit;
            if (Physics.Raycast(firePoint.position, directionToPlayer, out hit, Mathf.Infinity, obstacleMask))
            {
                // Проверяем, является ли обнаруженный объект игроком
                if (hit.transform == player)
                {
                    Debug.Log("Player in sight and no obstacles.");
                    return true;
                }
                else
                {
                    Debug.Log("Obstacle detected: " + hit.transform.name);
                }
            }
            else
            {
                Debug.Log("No obstacles detected.");
                return true;
            }
        }
        else
        {
            Debug.Log("Player out of angle range: " + angle);
        }

        return false;
    }

    void FireProjectile()
    {
        Debug.Log("Firing projectile.");
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = firePoint.forward * projectileSpeed;
        }
        Destroy(projectile, 5f); // Уничтожаем снаряд через 5 секунд, чтобы не засорять сцену
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

        if (canBeDestroyed){

        Rigidbody turretRb = turret.gameObject.AddComponent<Rigidbody>();
        if (turretRb != null)
        {
            turretRb.AddForce(Vector3.up * 5f); // Отбрасываем башню вверх
        }
        StopAllCoroutines();
        Invoke(nameof(RemoveTankModel), 3f); // Удаление модели через 3 секунды
        EnemyManager.instance.UnregisterEnemy(); // Уменьшаем количество врагов

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

        canBeDestroyed = false;
        }
    }

    void RemoveTankModel()
    {
        // Destroy(tank); // Удаляем модель танка
        // Destroy(turret); // Удаляем модель танка
    }
}
