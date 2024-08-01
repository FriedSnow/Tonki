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
    public Material grayMaterial;
    public GameObject ammoBox;
    public GameObject hpBox;
    public GameObject burningParticlesPrefab;
    public GameObject explosionParticlesPrefab;
    public GameObject shootParticlesPrefab;
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
    public float firingAngleThreshold = 180f; // Допустимый угол отклонения прицела
    private bool isDestroyed = false;
    private bool canBeDestroyed = true;
    private Transform player; // Теперь ищем игрока по тегу

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform; // Ищем игрока по тегу
        if (player != null)
        {
            playerTankController = player.GetComponent<PlayerTankController>();
        }
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
        if (player == null) return;

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
        //if (player == null) return;

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
            if (playerTankController != null && !playerTankController.IsDestroyed() && IsPlayerInSight())
            {
                FireProjectile();
            }
        }
    }

    bool IsPlayerInSight()
    {
        if (player == null) return false;

        Vector3 directionToPlayer = (player.position - firePoint.position).normalized;
        float angle = Vector3.Angle(firePoint.forward, directionToPlayer);



        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, directionToPlayer, out hit, Mathf.Infinity, obstacleMask))
        {
            // Проверяем, является ли обнаруженный объект игроком
            if (hit.transform == player)
            {
                // Debug.Log("Player in sight and no obstacles.");
                return true;
            }
            else
            {
                // Debug.Log("Obstacle detected: " + hit.transform.name);
            }
        }
        else
        {
            // Debug.Log("No obstacles detected.");
            return true;
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
        if (shootParticlesPrefab != null)
        {
            GameObject shootParticles = Instantiate(shootParticlesPrefab, firePoint.position, firePoint.rotation);
            Destroy(shootParticles, 3f); // Уничтожение частиц через 3 секунды
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
        float rnd = UnityEngine.Random.value;
        isDestroyed = true;
        if (canBeDestroyed)
        {
            EnemyManager.instance.UnregisterEnemy(); // Уменьшаем количество врагов
            Rigidbody turretRb = turret.gameObject.AddComponent<Rigidbody>();
            turretRb.mass = 40f;
            // if (turretRb != null)
            // {
            //     turretRb.AddForce(Vector3.up * 5f); // Отбрасываем башню вверх
            // }
            StopAllCoroutines();
            Invoke(nameof(RemoveTankModel), 3f); // Удаление модели через 3 секунды

            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                if (renderer != null)
                {
                    renderer.material = grayMaterial;
                }
            }
            // Создание частиц горения
            // if (burningParticlesPrefab != null)
            // {
            //     Instantiate(burningParticlesPrefab, transform.position, Quaternion.identity);
            // }
            if (explosionParticlesPrefab != null)
            {
                GameObject explosionParticles = Instantiate(explosionParticlesPrefab, transform.position, Quaternion.identity);
                Destroy(explosionParticles, 3f); // Уничтожение частиц через 3 секунды
            }
            Vector3 offset = new Vector3(0, 0, -5);
            if (ammoBox != null && rnd >.2)
            {
                Instantiate(ammoBox, transform.position + offset, Quaternion.identity);
            }
            else if (hpBox != null)
            {
                Instantiate(hpBox, transform.position + offset, Quaternion.identity);
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