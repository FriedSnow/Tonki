using UnityEngine;

public class ContinuousMissileController : MonoBehaviour
{
    public float speed = 10f;
    public float rotationSpeed = 5f;
    public GameObject explosionParticlesPrefab;
    public float damage = 50f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Получаем текущую позицию объекта курсора
        Vector3 targetPosition = CursorObjectController.Instance.GetCursorPosition();

        // Вектор направления к цели
        Vector3 direction = (targetPosition - transform.position).normalized;
        // Поворот снаряда в сторону цели
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, rotationSpeed * Time.fixedDeltaTime, 0.0f);
        rb.MoveRotation(Quaternion.LookRotation(newDirection));
        // Движение снаряда вперед
        rb.velocity = transform.forward * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Обработка урона
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        // Уничтожить снаряд после попадания
        Destroy(gameObject);
        if (explosionParticlesPrefab != null)
        {
            GameObject explosionParticles = Instantiate(explosionParticlesPrefab, transform.position, Quaternion.identity);
            Destroy(explosionParticles, 3f); // Уничтожение частиц через 3 секунды
        }
    }
}