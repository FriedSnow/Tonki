using System.Collections;
using UnityEngine;

public class MissileController : MonoBehaviour
{
    public float initialUpwardForce = 20f;
    public float turnDelay = 0.5f;
    public float speed = 10f;
    public float rotationSpeed = 5f;
    public float damage = 50f;
    public float explosionRadius = 5f;
    public float explosionForce = 10f;
    public GameObject explosionParticlesPrefab;

    private Vector3 targetPosition;
    private bool isTurning = false;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Устанавливаем начальную силу для взлета вверх
        rb.AddForce(Vector3.up * initialUpwardForce, ForceMode.Impulse);
        // Запускаем корутину для начала поворота через задержку
        StartCoroutine(StartTurningAfterDelay());
    }

    IEnumerator StartTurningAfterDelay()
    {
        yield return new WaitForSeconds(turnDelay);
        isTurning = true;
    }

    public void SetTarget(Vector3 target)
    {
        targetPosition = target;
    }

    void FixedUpdate()
    {
        if (isTurning)
        {
            // Вектор направления к цели
            Vector3 direction = (targetPosition - transform.position).normalized;
            // Поворот снаряда в сторону цели
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, rotationSpeed * Time.fixedDeltaTime, 0.0f);
            rb.MoveRotation(Quaternion.LookRotation(newDirection));
            // Движение снаряда вперед
            rb.velocity = transform.forward * speed;
        }
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
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }

            // if (enemy != null)
            // {
            //     enemy.TakeDamage(damage);
            // }
        }
    }
}
