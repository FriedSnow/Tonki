using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EnemyTankController : MonoBehaviour
{
    public GameObject tank;
    public Transform turret;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public Transform player;
    public float moveSpeed = 5f;
    public float rotateSpeed = 2f;
    public float turretRotateSpeed = 5f;
    public float fireInterval = 3f;
    public float projectileSpeed = 20f;
    public float desiredDistance = 20f; // Желаемое расстояние от игрока
    public int health = 100;
    private bool isDestroyed = false;
    private Vector3 targetPosition;
    private PlayerTankController playerTankController;

    void Start()
    {
        playerTankController = player.GetComponent<PlayerTankController>();
        StartCoroutine(FireRoutine());
    }

    void Update()
    {
        if (playerTankController != null && !playerTankController.IsDestroyed() && !isDestroyed)
        {
            MoveTank();
            RotateTurretTowardsPlayer();
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
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

        if (distanceToPlayer > desiredDistance || distanceToPlayer < desiredDistance - 5f)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }

    void RotateTurretTowardsPlayer()
    {
        Vector3 direction = (player.position - turret.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        turret.rotation = Quaternion.Slerp(turret.rotation, lookRotation, Time.deltaTime * turretRotateSpeed);
    }

    IEnumerator FireRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireInterval);
            if (!playerTankController.IsDestroyed())
            {
                FireProjectile();
            }
        }
    }

    void FireProjectile()
    {
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
        Rigidbody turretRb = turret.gameObject.AddComponent<Rigidbody>();
        turretRb.AddForce(Vector3.up * 5f, ForceMode.Impulse); // Отбрасываем башню вверх
        StopAllCoroutines();
        Invoke(nameof(RemoveTankModel), 5f); // Удаление модели через 5 секунд
        Invoke(nameof(RestartScene), 5f); // Перезапуск сцены через 5 секунд (уменьшено время)
    }

    void RemoveTankModel()
    {
        Destroy(tank); // Удаляем модель танка
        Destroy(turret); // Удаляем модель танка
    }

    void RestartScene()
    {
        Debug.Log("Перезапуск сцены..."); // Вывод сообщения в консоль для отладки
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Перезапускаем сцену
    }
}
